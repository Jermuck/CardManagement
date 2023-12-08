open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open CardManagement.API.ServerConfiguration

[<EntryPoint>]
let main _ =
     WebHostBuilder()
        .UseKestrel()
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .Build()
        .Run()
     0