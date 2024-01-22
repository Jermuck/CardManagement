module CardManagement.Client.WebApi

open Fable.Remoting.Client
open CardManagement.Shared
open Utils
open Core

let inline CreateApiProxy<'a> () =
    let token = "Bearer " + Browser.WebStorage.localStorage.getItem("token")
    Remoting.createApi()
    |> Remoting.withAuthorizationHeader token
    |> Remoting.withBaseUrl "http://localhost:5123"
    |> Remoting.withRouteBuilder buildApiRoute 
    |> Remoting.buildProxy<'a>

let userStore = CreateApiProxy<IUsersStore>()    
let profileStore = CreateApiProxy<IProfileStore>()
let cardsStore = CreateApiProxy<ICardsStore>()
let chartStore = CreateApiProxy<IChartStore>()