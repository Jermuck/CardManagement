namespace App

module Routes = 
    open Feliz
    open Feliz.Router
    open CardManagement.API.SharedTypes
    open App.Server
    open App.Components
    
    type Page =
        | Home
        | Auth
        | NotFound

    let parseUrl = function
        | [ ] ->  Page.Home
        | [ "auth" ] -> Page.Auth
        | _ -> NotFound
        
        
    [<ReactComponent>]
    let Router() =
        let (pageUrl, updateUrl) = React.useState(parseUrl(Router.currentUrl()))
        let currentPage =
            match pageUrl with
            | Home -> Html.h1 "Home"
            | Auth -> Pages.Auth()
            | NotFound -> Html.h1 "Not Found"

        React.router [
            router.pathMode
            router.onUrlChanged (parseUrl >> updateUrl)
            router.children currentPage
        ]