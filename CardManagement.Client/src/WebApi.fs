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

let userStore = CreateApiProxy<IUsersStore>()    
let profileStore = CreateApiProxy<IProfileStore>()
let cardsStore = CreateApiProxy<ICardsStore>()
let chartStore = CreateApiProxy<IChartStore>()