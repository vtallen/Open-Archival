using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using OpenArchival.Blazor.Components;
using OpenArchival.Blazor.Components.Account;
using OpenArchival.Blazor.Data;
using Dapper;
using Npgsql;
using OpenArchival.DataAccess;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddMudExtensions();

var postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContextFactory<ArchiveDbContext>
    (
    options => options.UseNpgsql(postgresConnectionString)
    );

builder.Services.AddOptions<FileUploadOptions>().Bind(builder.Configuration.GetSection(FileUploadOptions.Key));

var uploadSettings = builder.Configuration.GetSection(FileUploadOptions.Key).Get<FileUploadOptions>();
if (uploadSettings is null)
{
    throw new ArgumentNullException(nameof(uploadSettings), $"The provided {nameof(FileUploadOptions)} did not have a max upload size.");
}

builder.Services.AddServerSideBlazor().AddHubOptions(options =>
{
    options.MaximumReceiveMessageSize = uploadSettings.MaxUploadSizeBytes; 
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<IArchiveCategoryProvider, ArchiveCategoryProvider>();
builder.Services.AddScoped<IFilePathListingProvider, FilePathListingProvider>();
builder.Services.AddScoped<IArtifactStorageLocationProvider, ArtifactStorageLocationProvider>();
builder.Services.AddScoped<IArtifactDefectProvider, ArtifactDefectProvider>();
builder.Services.AddScoped<IArtifactTypeProvider, ArtifactTypeProvider>();
builder.Services.AddScoped<IArchiveEntryTagProvider, ArchiveEntryTagProvider>();
builder.Services.AddScoped<IListedNameProvider, ListedNameProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await InitializeDatabaseAsync(app.Services);

async Task InitializeDatabaseAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

    var categoryProvider = serviceProvider.GetRequiredService<IArchiveCategoryProvider>(); 

    // await categoryProvider.CreateCategoryAsync(new ArchiveCategory {Name="Yearbooks", Description="This is a description", FieldSeparator="-", FieldNames = [], FieldDescriptions = [] });
}

app.Run();

//TODO: Periodic clean of the uploaded files to prune ones not in the database



