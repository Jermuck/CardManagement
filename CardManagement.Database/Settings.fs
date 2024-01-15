module CardManagement.Database.Settings

open System.IO
open Microsoft.Extensions.Configuration

let currentDirectory = Directory.GetCurrentDirectory()
let path = Path.GetFullPath(currentDirectory).Replace("Server", "Database")

let configService = ConfigurationBuilder()
                        .SetBasePath(path + "/Properties")
                        .AddJsonFile("appsettings.json", true, true)
                        .Build()

let connectionString = configService.GetSection("AppSettings:ConnectionString").Value