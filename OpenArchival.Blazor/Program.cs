using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using OpenArchival.Blazor;
using OpenArchival.Blazor.Components;
using OpenArchival.Blazor.Components.Account;
using OpenArchival.DataAccess;
using OpenArchival.Blazor.Components.Account;
using MyAppName.WebApp.Components.Account;

var builder = WebApplication.CreateBuilder(args);

// --- UI Services ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();

// --- Database & Identity Configuration ---
// Get the single connection string for your PostgreSQL database.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Use AddDbContextFactory for Blazor Server. This is safer for managing DbContext lifecycles.
// This single ApplicationDbContext will handle both Identity and your application data.
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity to use ApplicationDbContext, which is now pointed at PostgreSQL.
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

// --- File Upload Configuration ---
builder.Services.AddOptions<FileUploadOptions>().Bind(builder.Configuration.GetSection(FileUploadOptions.Key));
var uploadSettings = builder.Configuration.GetSection(FileUploadOptions.Key).Get<FileUploadOptions>() ?? throw new ArgumentNullException("FileUploadOptions");
builder.Services.AddServerSideBlazor().AddHubOptions(options =>
{
    options.MaximumReceiveMessageSize = uploadSettings.MaxUploadSizeBytes;
});

// --- Custom Application Services ---
builder.Services.AddScoped<IArchiveCategoryProvider, ArchiveCategoryProvider>();
builder.Services.AddScoped<IFilePathListingProvider, FilePathListingProvider>();
builder.Services.AddScoped<IArtifactStorageLocationProvider, ArtifactStorageLocationProvider>();
builder.Services.AddScoped<IArtifactDefectProvider, ArtifactDefectProvider>();
builder.Services.AddScoped<IArtifactTypeProvider, ArtifactTypeProvider>();
builder.Services.AddScoped<IArchiveEntryTagProvider, ArchiveEntryTagProvider>();
builder.Services.AddScoped<IListedNameProvider, ListedNameProvider>();
builder.Services.AddScoped<ArtifactEntrySharedHelpers>();
builder.Services.AddScoped<IArtifactGroupingProvider, ArtifactGroupingProvider>();

builder.Services.AddLogging();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        await IdentityDataSeeder.SeedRolesAndAdminUserAsync(serviceProvider);
    }
    catch (Exception ex)
    {
        // Log errors or handle them as needed
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();