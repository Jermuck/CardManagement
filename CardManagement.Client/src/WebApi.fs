module CardManagement.Client.WebApi

open CardManagement.Shared.Core
open CardManagement.Shared.Utils
open Fable.Remoting.Client

let inline CreateApiProxy<'a> () =
    let token = "Bearer " + Browser.WebStorage.localStorage.getItem("token")
    Remoting.createApi()
    |> Remoting.withAuthorizationHeader token
    |> Remoting.withBaseUrl "http://localhost:5123"
    |> Remoting.withRouteBuilder BuildApiRoute 
    |> Remoting.buildProxy<'a>

let createUserStore() = CreateApiProxy<IUsersStore>()    
let createProfileStore() = CreateApiProxy<IProfileStore>()
let createCardsStore() = CreateApiProxy<ICardsStore>()
let createChartStore() = CreateApiProxy<IChartStore>()