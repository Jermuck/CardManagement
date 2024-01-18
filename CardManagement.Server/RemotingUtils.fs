module CardManagement.Server.RemotingUtils

open System
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open CardManagement.Shared.Utils
open Microsoft.AspNetCore.Http

let createPublicHandler controller = 
    Remoting.createApi()
    |> Remoting.withRouteBuilder BuildApiRoute
    |> Remoting.fromValue controller
    |> Remoting.buildHttpHandler

let createPrivateHandler securedService =
    Remoting.createApi()
    |> Remoting.withRouteBuilder BuildApiRoute
    |> Remoting.fromContext securedService // <-- we need context here
    |> Remoting.buildHttpHandler
    
let getUserIdFromHttpContext (ctx: HttpContext) =
    ctx.User.Claims
    |> Seq.find(fun x -> x.Type = "Id")
    |> (fun x -> Guid x.Value)