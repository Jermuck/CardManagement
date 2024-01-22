module CardManagement.Client.Routes

open System
open Feliz
open Feliz.Router
open Feliz.UseDeferred
open CardManagement.Client
open CardManagement.Shared
open Pages.HomePage
open Pages.AuthPage
open Pages.LoadingPage
open Pages.CardsPage
open Pages.SettingsPage
open WebApi
open Types

type Page =
    | Settings
    | HomeWithoutArgs 
    | Home of cardId: Guid
    | Auth
    | NotFound
    | Cards

let private parseUrl = function
    | [ "settings" ] -> Page.Settings
    | [ "home" ] -> Page.HomeWithoutArgs
    | [ "home"; Route.Query [ "id", Route.Guid cardId ]  ] -> Page.Home cardId
    | [ "authorization" ] -> Page.Auth
    | [ "cards"; "create" ] -> Page.Cards
    | _ -> NotFound

let private getPrivateRoutes pageUrl =
    match pageUrl with
    | Settings -> SettingsPage()
    | HomeWithoutArgs -> HomeWithoutArgsPage()
    | Home cardId -> HomePage cardId
    | Cards -> CardsPage()
    | _ -> Html.h1 "Not found"

let private getPublicRoutes pageUrl =
    match pageUrl with
    | Auth -> AuthPage()
    | _ -> Html.h1 "Not found"
    
[<ReactComponent>]
let Router() =
    let pageUrl, updateUrl = React.useState(parseUrl(Router.currentUrl()))
    
    let getRoutesCallback() = async {
        try
            let! profile = profileStore.GetMyProfile()
            match profile with
            | Ok _ -> return getPrivateRoutes
            | Error _-> return getPublicRoutes
        with
            | ex -> printfn "%A" ex; return getPublicRoutes
    }
    
    let data = React.useDeferred(getRoutesCallback(), [||])
    
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