using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.DataAccess;
static class Program
{
    public static void Main(string[] args)
    {
        // In Program.cs
        var builder = WebApplication.CreateBuilder(args);

        // Retrieve the connection string from appsettings.json
        var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
        
        // Add the DbContext to the dependency injection container
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        var app = builder.Build();

        app.Run();
    }    
};
