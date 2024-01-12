module CardManagement.Server.Core

open Microsoft.AspNetCore.Hosting
open CardManagement.Server.ServerConfiguration
open Microsoft.Extensions.Hosting

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(configureApp)
                .UseUrls("http://localhost:5123")
                .ConfigureServices(configureServices)
                |> ignore
            )
        .Build()
        .Run()
    0