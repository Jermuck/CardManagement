namespace CardManagement.Data

module Settings =
    open Microsoft.Extensions.Configuration
    
    let configService = ConfigurationBuilder()
                            .SetBasePath("/Users/irinabardugova/Desktop/CardManagement/CardManagement.Data/Properties")
                            .AddJsonFile("appsettings.json", true, true)
                            .Build()
    
    let connectionString = configService.GetSection("AppSettings:ConnectionString").Value