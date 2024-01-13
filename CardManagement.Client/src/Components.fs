module CardManagement.Client.Components

open CardManagement.Shared.Types
open Feliz.Bulma
open Feliz.Router
open Feliz
open CardManagement.Client.Types
    
let navigate (path : string list) =
    path |> Router.formatPath |> Router.navigatePath

let convertCardToPoint (code: string) =
    "•••• •••• •••• " + code[-4..]

[<ReactComponent>]
let InputText (placeholder: string) (label: string) (type': InputType) (onInput: string -> unit) =
    Bulma.field.div [
        Bulma.label label
        Bulma.input.text [
            prop.type' (type'.ToString().ToLower())
            prop.placeholder placeholder
            prop.onChange onInput
        ]
    ]
    
let InputNumber (placeholder: string) (label: string) (onInput: int -> unit) =
     Bulma.field.div [
        Bulma.label label
        Bulma.input.text [
            prop.min 0
            prop.type'.number
            prop.placeholder placeholder
            prop.onChange onInput
        ]
    ]

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

    
    

