namespace CardManagement.API

module ConfigService =
    open Microsoft.Extensions.Configuration
    open System.IO
    
    let currentDirectory = Directory.GetCurrentDirectory()
    
    let configService = ConfigurationBuilder()
                            .SetBasePath(Path.GetFullPath(currentDirectory) + "/Properties")
                            .AddJsonFile("appsettings.json", true, true)
                            .Build()
    
    let getValue path =
        configService.GetSection(path).Value

