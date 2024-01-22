module CardManagement.Client.CardActionButtonsComponent

open Feliz
open Feliz.Bulma

[<ReactComponent>]
let CardActionButtonsComponent createTransaction blockCard =
    Html.div [
        prop.style [
            style.backgroundColor "#FAFAFB"
            style.borderRadius 12
            style.height.maxContent
            style.padding 20
            style.display.flex
            style.justifyContent.spaceBetween
        ]
        prop.children [
            Bulma.button.button [
                Bulma.color.isDanger
                prop.text "Block"
                prop.onClick blockCard
            ]
            Bulma.button.button [
                Bulma.color.isPrimary
                prop.text "Create transaction"
                prop.onClick createTransaction
            ]
        ]
    ]