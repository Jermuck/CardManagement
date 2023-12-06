namespace CardManagement.Data

open System.Diagnostics
open System.Reflection

module Settings =
    open Microsoft.Extensions.Configuration
    open System.IO
    
    let configService = ConfigurationBuilder()
                            .SetBasePath("/Users/kirillbardugov/Desktop/Projects/f#/CardManagement/CardManagement.Data/Properties")
                            .AddJsonFile("appsettings.json", true, true)
                            .Build()
    
    let connectionString = configService.GetSection("AppSettings:ConnectionString").Value