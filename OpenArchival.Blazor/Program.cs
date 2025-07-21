using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using OpenArchival.Blazor.Components;
using OpenArchival.Blazor.Components.Account;
using OpenArchival.Blazor.Data;
using OpenArchival.Database;
using Dapper;
using Npgsql;
using OpenArchival.Database.Category;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var postgresOptions = builder.Configuration
                             .GetSection(PostgresConnectionOptions.Key)
                             .Get<PostgresConnectionOptions>();
if (postgresOptions == null || string.IsNullOrEmpty(postgresOptions.ConnectionString)) throw new InvalidOperationException("Postgres connection options are not configured properly.");

builder.Services.AddNpgsqlDataSource(postgresOptions.ConnectionString);

// Add options
builder.Services.AddOptions<PostgresConnectionOptions>().Bind(builder.Configuration.GetSection(PostgresConnectionOptions.Key));

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<ICategoryProvider, CategoryProvider>();

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

    logger.LogInformation("Initializing database...");

    var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();

    await using var connection = await dataSource.OpenConnectionAsync();

    await connection.ExecuteAsync(CategoryProvider.TableCreationQuery);

    var categoryProvider = serviceProvider.GetRequiredService<ICategoryProvider>();

    await categoryProvider.InsertCategoryAsync(new Category() {CategoryName="Pictures", FieldSeparator="-", FieldNames=new string[]{"one", "two"},FieldDescriptions=new string[] { "one", "two"} });
    await categoryProvider.InsertCategoryAsync(new Category() {CategoryName="Yearbooks", FieldSeparator="-", FieldNames=new string[]{"one", "two"},FieldDescriptions=new string[] { "one", "two"}});
    await categoryProvider.InsertCategoryAsync(new Category() {CategoryName="Books", FieldSeparator="-", FieldNames=new string[]{"one", "two"}, FieldDescriptions = new string[] { "one", "two" } });
    await categoryProvider.InsertCategoryAsync(new Category() {CategoryName="Newspapers", FieldSeparator="-", FieldNames=new string[]{"one", "two"}, FieldDescriptions = new string[] { "one", "two" }});
    await categoryProvider.InsertCategoryAsync(new Category() {CategoryName="Letters", FieldSeparator="-", FieldNames=new string[]{"one", "two"}, FieldDescriptions = new string[] { "one", "two" } });

    logger.LogInformation("Database initialization complete.");
}

app.Run();
