module CardManagement.Client.CardComponent

open CardManagement.Shared.Types
open Feliz

let convertCardToPoint (code: string) =
    "•••• •••• •••• " + code[-4..]

[<ReactComponent>]
let CardComponent (status: TypeOfCard) =
    
    let className =
        match status with
        | Basic -> "card_basic"
        | Priority -> "card_priority"
    
    Html.div [
        prop.className className
        prop.style [
            style.position.relative
        ]
        prop.children [
            Html.div [
                prop.style [
                    style.position.absolute
                    style.left 24
                    style.top 44
                ]
                prop.children [
                    Html.h1 [
                        prop.text "Salary card"
                        prop.style [
                            style.color "#A2C8FB"
                        ]
                    ]
                    Html.h1 [
                        prop.text "100$"
                        prop.style [
                            style.color "#FFF"
                        ]
                    ]
                ]
            ]
            Html.img [
                prop.src "/img/Icon_Visa.svg"
                prop.style [
                    style.position.absolute
                    style.bottom 24
                    style.left 24
                ]
            ]
            Html.div [
                prop.className "rectangle"
                prop.style [
                    match status with
                    | Basic -> style.backgroundColor "#49B1FF"
                    | Priority -> style.backgroundColor "gold"
                ]
                prop.children [
                    Html.h1 [
                        prop.text "../.."
                        prop.style [
                            style.position.absolute
                            style.right 24
                            style.bottom 49
                            style.color "#FFF"
                        ]
                    ]
                    Html.h1 [
                        prop.text (convertCardToPoint "4444444444444444")
                        prop.style [
                            style.position.absolute
                            style.right 24
                            style.bottom 20
                            style.color "#FFF" 
                        ]
                    ]
                ]
            ]
        ]
    ]
