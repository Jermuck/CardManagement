namespace CardManagement.Data

open Microsoft.Extensions.Configuration

module Settings =
    let configService = ConfigurationBuilder()
                            .SetBasePath("./Properties")
                            .AddJsonFile("appsettings.json", true, true)
                            .Build()
    
    let private pathSectionDatabase = "AppSettings:Database:"
    
    let username = configService.GetSection(pathSectionDatabase + "Name").Value
    let password = configService.GetSection(pathSectionDatabase + "Password").Value
    