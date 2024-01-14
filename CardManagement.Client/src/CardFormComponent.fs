module CardManagement.Client.CardFormComponent

open CardManagement.Client.Types
open Feliz.Bulma
open Feliz

[<ReactComponent>]
let CardFormComponent (props: ICardForm) =
    Bulma.card [
        prop.className "card_form"
        prop.onClick (fun _ -> props.onClick props.TypeCard)
        prop.style [
            style.width 370
            style.cursor.pointer
            style.boxSizing.contentBox
        ]
        prop.children [
            Bulma.cardImage [
                prop.style [
                    style.height 220
                    style.display.flex
                    style.justifyContent.center
                    style.alignItems.center
                ]
                prop.children [
                    props.CardElement
                ]
            ]
            Bulma.cardContent [
                Bulma.media [
                    Bulma.mediaLeft [
                        Bulma.tag [
                            Bulma.color.isPrimary
                            tag.isMedium
                            prop.text props.TagText
                        ]
                    ]
                ]
                Bulma.content props.Content
            ]
       ] 
    ]