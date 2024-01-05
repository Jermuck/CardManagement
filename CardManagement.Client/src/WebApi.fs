module CardManagement.Client.WebApi

open CardManagement.Shared.Core
open CardManagement.Shared.RouteBuilders
open Fable.Remoting.Client

let inline CreateApiProxy<'a> () =
    Remoting.createApi()
    |> Remoting.withBaseUrl "http://localhost:5000"
    |> Remoting.withRouteBuilder BuildApiRoute 
    |> Remoting.buildProxy<'a>

let privateStore = CreateApiProxy<IPrivateStore>()
let userStore = CreateApiProxy<IUsersStore>()    
