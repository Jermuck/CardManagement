module CardManagement.Server.Core

open Microsoft.AspNetCore.Hosting
open CardManagement.Server.ServerConfiguration
open Microsoft.Extensions.Hosting

let ConfigureWebHost (webHostBuilder: IWebHostBuilder) =
    webHostBuilder
        .Configure(configureApp)
        .ConfigureServices(configureServices)
        |> ignore

[<EntryPoint>]
let Main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(ConfigureWebHost)
        .Build()
        .Run()
    0