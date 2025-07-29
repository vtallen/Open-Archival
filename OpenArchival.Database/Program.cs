using Microsoft.Extensions.Logging;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenArchival.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Use Host.CreateDefaultBuilder to get config from appsettings.json
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // 1. Get the options from appsettings.json
                var postgresOptions = context.Configuration
                                             .GetSection(PostgresConnectionOptions.Key)
                                             .Get<PostgresConnectionOptions>();

                if (postgresOptions is null || string.IsNullOrEmpty(postgresOptions.ConnectionString))
                {
                    throw new InvalidOperationException("PostgresConnectionOptions not configured properly.");
                }

                // 2. Register the NpgsqlDataSource as a singleton
                services.AddNpgsqlDataSource(postgresOptions.ConnectionString);

                // 3. Register your provider and its interface
                services.AddScoped<ICategoryProvider, CategoryProvider>();
                services.AddOptions<PostgresConnectionOptions>().BindConfiguration("PostgresConnectionOptions");
            })
            .Build();

        // Create a scope to resolve and use your services
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            // A. Initialize the database schema
            logger.LogInformation("Initializing database schema...");
            var dataSource = services.GetRequiredService<NpgsqlDataSource>();
            await using var connection = await dataSource.OpenConnectionAsync();

            await connection.ExecuteAsync(Tables.CategoryTable);
            await connection.ExecuteAsync(Tables.ArtifactAssociatedNamesTable);
            await connection.ExecuteAsync(Tables.ArtifactTypesTable);

            logger.LogInformation("Schema initialized successfully.");

            // B. Get the provider service
            var provider = services.GetRequiredService<ICategoryProvider>();

            // C. Create a new category to insert
            var newCategory = new Category
            {
                CategoryName = "Invoices",
                FieldSeparator = ",",
                FieldNames = new string[] { "InvoiceNumber", "Amount", "DueDate" },
                FieldDescriptions = new string[] {"The number of the invoice", "The amount", "The date it was created"}
            };

            // D. Insert the category
            logger.LogInformation("Inserting new category: {CategoryName}", newCategory.CategoryName);
            await provider.InsertCategoryAsync(newCategory);
            logger.LogInformation("Insert successful.");

            // E. Retrieve the category to verify it was inserted
            logger.LogInformation("Retrieving category: {CategoryName}", newCategory.CategoryName);
            var retrievedCategory = await provider.GetCategoryAsync("Invoices");

            if (retrievedCategory != null)
            {
                logger.LogInformation("Successfully retrieved category '{CategoryName}' with {FieldCount} fields.",
                    retrievedCategory.CategoryName, retrievedCategory.FieldNames.Length);
            }
            else
            {
                logger.LogError("Failed to retrieve category.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while running the application.");
            Environment.Exit(1);
        }
    }
}
