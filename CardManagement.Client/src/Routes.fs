module CardManagement.Client.Routes

open Feliz
open Feliz.Router
open CardManagement.Client.WebApi
open CardManagement.Shared.Types
open CardManagement.Client.Pages.HomePage
open CardManagement.Client.Pages.AuthPage
open CardManagement.Client.Pages.LoadingPage
open CardManagement.Client.Pages.CardsPage
open Feliz.UseDeferred

type Page =
    | Home
    | Auth
    | NotFound
    | Cards

let private parseUrl = function
    | [ "home" ] -> Page.Home
    | [ "authorization" ] -> Page.Auth
    | [ "cards"; "create" ] -> Page.Cards
    | _ -> NotFound

let private getPrivateRoutes pageUrl =
    match pageUrl with
    | Home -> HomePage()
    | Cards -> CardsPage()
    | _ -> Html.h1 "Not found"

let private getPublicRoutes pageUrl =
    match pageUrl with
    | Auth -> AuthPage()
    | _ -> Html.h1 "Not found"
    
[<ReactComponent>]
let Router() =
    let pageUrl, updateUrl = React.useState(parseUrl(Router.currentUrl()))
    
    let getIsAuth() = async {
        try
            let! profile = profileStore.GetMyProfile()
            match profile with
            | Ok _ -> return getPrivateRoutes
            | Error _-> return getPublicRoutes
        with
            | ex -> printfn "%A" ex; return getPublicRoutes
    }
    
    let data = React.useDeferred(getIsAuth(), [||])
    
    let currentPage =
        match data with
        | Deferred.HasNotStartedYet -> Html.h1 "Started"
        | Deferred.InProgress -> LoadingPage()
        | Deferred.Failed error -> Html.div error.Message
        | Deferred.Resolved routeBuilderCallback -> routeBuilderCallback pageUrl
    
    
    React.router [
        router.pathMode
        router.onUrlChanged (parseUrl >> updateUrl)
        router.children currentPage 
    ]