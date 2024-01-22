module CardManagement.Client.ErrorComponent

open Feliz

[<ReactComponent>]
let ErrorComponent (text: string) (right: int) (top: int) (color: string) =
    Html.div [
        prop.className "error"
        prop.text text
        prop.style [
            style.padding 10
            style.backgroundColor color
            style.color "white"
            style.borderRadius 10
            style.position.absolute
            style.right right
            style.top top
            style.zIndex 1000
        ]
    ]