namespace CardManagement.Data

module DatabaseConfiguration =
    open System.IO
    open Microsoft.Extensions.Configuration
    
    let configService = ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", true, true)
                            .Build()
    let databaseConnectionString = configService.GetSection("AppSettings:ConnectionString").Value
    