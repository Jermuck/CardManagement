module CardManagement.Client.Pages.Home

open CardManagement.Shared.Types
open Feliz
open Feliz.Bulma
open CardManagement.Client.Components
open CardManagement.Client.WebApi
open Feliz.UseDeferred
open CardManagement.Client.Pages.Loading

[<ReactComponent>]
let HomeHeader() =
    let logout (e: Browser.Types.MouseEvent) =
        e.preventDefault()
        Browser.WebStorage.localStorage.removeItem "token"
        navigate [ "authorization" ]
        Browser.Dom.window.location.reload()
    
    Bulma.navbar [
        Bulma.color.isLink
        prop.style [
            style.backgroundColor "#3D70FF"
        ]
        prop.children [
            Bulma.navbarBrand.div [
                Bulma.navbarItem.div [
                     Html.h1 [
                        prop.text "Bank"
                        prop.style [
                            style.color "white"
                            style.fontWeight 400
                            style.fontSize 26
                            style.marginLeft 10
                        ]
                    ]
                ]
            ]
            Bulma.navbarMenu [
                prop.style [
                    style.marginLeft 40
                ]
                prop.children [
                    Bulma.navbarStart.div [
                        Bulma.navbarItem.a [ prop.text "Cards" ]
                        Bulma.navbarItem.a [ prop.text "Transactions" ]
                        Bulma.navbarItem.a [ prop.text "Settings" ]
                        Bulma.navbarItem.a [ prop.text "About" ]
                    ]
                ]
            ]
            Bulma.navbarEnd.div [
                Bulma.navbarItem.a [
                    prop.onClick logout
                    prop.text "Logout"
                ]
            ]
        ]
    ]
    

[<ReactComponent>]
let HomePage() =
    let getComponent cards =
        match Seq.isEmpty cards with
        | true -> TimeLineBank()
        | false -> Html.h1 "Your cards"
    
    let getCards() = async {
        try
            let! result = cardsStore.Get()
            match result with
            | Error _ -> return LoadingPage()
            | Ok cards -> return getComponent cards
        with
            | ex -> printfn "%A" ex; return LoadingPage()
    }
    
    let data = React.useDeferred(getCards(), [| |])
    
    let content =
        match data with
        | Deferred.Failed error -> Html.div error.Message
        | Deferred.Resolved result -> result
        | _ -> LoadingPage()
    
    Html.div [
        prop.style [
            style.height (length.vh 100)
            style.display.flex
            style.flexDirection.column
        ]
        prop.children [
            HomeHeader()
            content
        ]
    ]
