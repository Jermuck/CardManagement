module CardManagement.Client.TimeLineComponent

open Feliz
open Feliz.Bulma
open CardManagement.Client.Utils

[<ReactComponent>]
let TimeLineComponent() =
    let data = [
        "First", "Choose and create the bank card you need";
        "Second", "Replenish and transfer without commission";
        "Third", "Track your card stats";
        "Fourth", "The best bank for business";
        "Fifth", "Make your first transaction";
    ]
    
    Html.div [
        prop.style [
            style.width (length.perc 100)
            style.height (length.vh 100)
            style.display.flex
            style.flexDirection.column
            style.justifyContent.center
            style.alignItems.center
        ]
        prop.children [
            Timeline.timeline [
                timeline.isCentered
                prop.children [
                    Timeline.header [
                        Bulma.tag [
                            color.isPrimary; tag.isMedium; prop.text "Start"
                        ]
                    ]
                    for key, value in data do
                        Timeline.item [
                            Timeline.marker []
                            Timeline.content [
                                Timeline.content.header key
                                Timeline.content.content value
                            ]
                        ]
                    Timeline.header [
                        Bulma.tag [
                            color.isPrimary; tag.isMedium; prop.text "End"
                        ]
                    ]
                ]
            ]
            Html.div [
                prop.style [
                    style.width (length.perc 100)
                    style.display.flex
                    style.justifyContent.flexEnd
                ]
                prop.children [
                    Bulma.button.button [
                        Bulma.color.isPrimary
                        prop.text "Create Card"
                        prop.onClick (fun _ -> navigate ["cards"; "create"])
                        prop.style [
                            style.marginRight 30
                        ]
                    ]
                ]
            ]
        ]
    ]