module CardManagement.Client.HomeBodyComponent

open Feliz
open CardManagement.Client.CardsDashboardComponent
open CardManagement.Client.CardComponent
open CardManagement.Shared.Types

[<ReactComponent>]
let HomeBodyComponent cards =
    Html.div [
        prop.style [
            style.flexGrow 1
            style.display.flex
        ]
        prop.children [
            CardsDashboardComponent cards
            CardComponent Basic
        ]
    ]