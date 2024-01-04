namespace CardManagement.Data

module Settings =
    open Microsoft.Extensions.Configuration
    
    let configService = ConfigurationBuilder()
                            .SetBasePath("/Users/kirillbardugov/Desktop/Projects/fcs/CardManagement/CardManagement.Data/Properties")
                            .AddJsonFile("appsettings.json", true, true)
                            .Build()
    
    let connectionString = configService.GetSection("AppSettings:ConnectionString").Value