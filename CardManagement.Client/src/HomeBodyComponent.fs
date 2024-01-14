module CardManagement.Client.HomeBodyComponent

open Feliz
open CardManagement.Client.CardsDashboardComponent
open CardManagement.Client.CardComponent
open CardManagement.Client.Chart
open CardManagement.Shared.Types

[<ReactComponent>]
let HomeBodyComponent (cards: Card seq) =
    let card, setCard = React.useState<Card>(Seq.item 0 cards)
    
    let changeCard id =
        cards
        |> Seq.find (fun currentCard -> currentCard.Id = id)
        |> setCard
    
    Html.div [
        prop.style [
            style.flexGrow 1
            style.display.flex
        ]
        prop.children [
            CardsDashboardComponent cards changeCard
            Html.div [
                prop.style [
                    style.width (length.perc 80)
                    style.display.flex
                    style.justifyContent.spaceBetween
                    style.marginTop 20
                    style.marginLeft 20
                    style.marginRight 20
                ]
                prop.children [
                    CardComponent card
                    Html.div [
                        prop.style [
                            style.backgroundColor "#FAFAFB"
                            style.borderRadius 12
                            style.height.maxContent
                        ]
                        prop.children [
                            Html.h1 [
                                prop.style [
                                    style.fontSize 18
                                    style.fontWeight 500
                                    style.marginBottom 10
                                ]
                                prop.text "Insights"
                            ]
                            Chart 650 300
                        ]
                    ]
                ]
            ]
        ]
    ]