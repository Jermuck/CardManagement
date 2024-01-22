module CardManagement.Client.Pages.LoadingPage

open Feliz
open Feliz.Bulma

[<ReactComponent>]
let LoadingPage() =
    Bulma.column [
        prop.style [
            style.height (length.vh 100)
            style.display.flex
            style.justifyContent.center
            style.alignItems.center
        ]
        prop.children [
            Html.div [
                prop.style [
                    style.width 300
                ]
                prop.children [
                    Html.h1 "Loading..."
                    Bulma.progress [
                        Bulma.color.isPrimary
                        prop.max 100
                    ]
                ]
            ]
        ]
    ]