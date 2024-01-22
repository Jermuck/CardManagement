module CardManagement.Client.HomeHeaderComponent

open Feliz
open Feliz.Bulma
open CardManagement.Client.Utils

[<ReactComponent>]
let HomeHeaderComponent() =
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
                        Bulma.navbarItem.a [
                            prop.text "Home"
                            prop.onClick (fun _ -> navigate [ "home" ])
                        ]
                        Bulma.navbarItem.a [
                            prop.text "Create card"
                            prop.onClick (fun _ -> navigate [ "cards"; "create" ])
                        ]
                        Bulma.navbarItem.a [
                            prop.text "Settings"
                            prop.onClick (fun _ -> navigate [ "settings" ])
                        ]
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