using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database
{
    public class PostgresConnectionOptions
    {
        /// <summary>
        /// The name of the configuration section for Postgres connection options.
        /// </summary>
        public static string Key = "PostgresConnectionOptions"; 
        public string Host { get; set; } 
        public int Port { get; set; } 
        public string Database { get; set; } 
        public string Username { get; set; } 
        public string Password { get; set; } 
        public string ConnectionString => $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
    }
}
