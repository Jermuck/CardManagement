module CardManagement.Server.RemotingUtils

open System
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Microsoft.AspNetCore.Http
open CardManagement.Shared.Utils

let createPublicHandler controller = 
    Remoting.createApi()
    |> Remoting.withRouteBuilder buildApiRoute
    |> Remoting.fromValue controller
    |> Remoting.buildHttpHandler

let createPrivateHandler securedService =
    Remoting.createApi()
    |> Remoting.withRouteBuilder buildApiRoute
    |> Remoting.fromContext securedService // <-- we need context here
    |> Remoting.buildHttpHandler
    
let getUserIdFromHttpContext (ctx: HttpContext) =
    ctx.User.Claims
    |> Seq.find(fun x -> x.Type = "Id")
    |> (fun x -> Guid x.Value)