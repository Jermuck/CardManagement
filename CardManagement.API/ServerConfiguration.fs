namespace CardManagement.API

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection

module ServerConfiguration =
    open Fable.Remoting.Server
    open Fable.Remoting.Giraffe
    open Giraffe
    
    
    type UsersStore = {
        create: unit -> Async<string>
    }
    
    let st: UsersStore = {
        create = fun _ -> async {
            return "hello"
        } 
    }
    
    let webApp =
        Remoting.createApi()
        |> Remoting.fromValue st
        |> Remoting.buildHttpHandler
    
    let configureApp (app : IApplicationBuilder) =
        app.UseGiraffe webApp

    let configureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore
    
