namespace CardManagement.Data

module DatabaseConfiguration =
    open Microsoft.Extensions.Configuration
    let configService = ConfigurationBuilder()
                            .SetBasePath("/Users/kirillbardugov/Desktop/Projects/f#/CardManagement/CardManagement.Data")
                            .AddJsonFile("appsettings.json", true, true)
                            .Build()
    let databaseConnectionString = configService.GetSection("AppSettings:ConnectionString").Value
    