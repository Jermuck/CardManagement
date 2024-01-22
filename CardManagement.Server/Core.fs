module CardManagement.Server.Core

open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open CardManagement.Server
open ServerConfiguration

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