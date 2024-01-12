module CardManagement.Client.Pages.CreatingCard

open Feliz
open CardManagement.Client.Components
open CardManagement.Shared.Types
open CardManagement.Client.Pages.Home
open Feliz.Bulma
open Fable.Core.JS
open CardManagement.Client.WebApi

let priorityCardText = "The Priority Bank Card is a premium offering designed for customers who value exclusive benefits and personalized services. With this card, you can enjoy a range of privileges such as access to airport lounges, concierge services, travel insurance, and higher cashback or reward points on your purchases. The Priority Bank Card is tailored for individuals who frequently travel, dine out, or engage in luxury experiences. It offers enhanced security features and provides a higher credit limit to meet your financial needs. Experience the convenience and prestige of the Priority Bank Card."
let basicCardText = "The Basic Bank Card is a simple and straightforward option suitable for everyday banking needs. It provides essential features and functionality without any frills or additional perks. With this card, you can make purchases at various merchants, withdraw cash from ATMs, and manage your finances conveniently. The Basic Bank Card is ideal for individuals who prefer a no-nonsense approach to banking and want a reliable payment method for their day-to-day transactions. It offers ease of use, affordability, and peace of mind for your basic banking requirements."

type ICardForm = {
    CardElement: ReactElement
    TypeCard: TypeOfCard
    TagText: string
    Content: string
    onClick: TypeOfCard -> unit
}

type IMessage = {
    Message: string
    Color: string
}

let CardForm (props: ICardForm) =
    Bulma.card [
        prop.className "card_form"
        prop.onClick (fun _ -> props.onClick props.TypeCard)
        prop.style [
            style.width (length.perc 26)
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

[<ReactComponent>]
let CreateCardsPage() =
    let error, setError = React.useState<IMessage option> None
    
    let timeoutCallback _ =
        setError None
        navigate [ "home" ]
        
    let createCard (typeCard: TypeOfCard) = async {
        try
            setError None
            let! newCard = cardsStore.Create typeCard
            match newCard with
            | Error error ->
                { Message = error.Message; Color = "#f14668" } |> Some |> setError
                setTimeout (fun _ -> setError None) 2000 |> ignore
            | Ok _ ->
                { Message = "Card was success create. Thanks!"; Color = "#00d1b2" } |> Some |> setError
                setTimeout timeoutCallback 2000 |> ignore
        with
            | ex -> printfn "%A" ex; { Message = "Server error"; Color = "#f14668" } |> Some |> setError
    }
    
    Html.div [
        prop.style [
            style.display.flex
            style.flexDirection.column
            style.height (length.vh 100)
            style.position.relative
        ]
        prop.children [
            HomeHeader()
            Html.div [
                prop.style [
                    style.width (length.perc 100)
                    style.display.flex
                    style.justifyContent.spaceEvenly
                    style.alignItems.center
                    style.flexGrow 3
                ]
                prop.children [
                    match error with
                    | Some error -> ErrorComponent error.Message 20 80 error.Color
                    | None -> Html.none
                    CardForm {
                        CardElement = (CardComponent Basic)
                        TypeCard = Basic
                        TagText = "Basic Bank Card"
                        Content = basicCardText
                        onClick = (fun v -> createCard v |> Async.StartImmediate)
                    } 
                    CardForm {
                        CardElement = (CardComponent Priority)
                        TypeCard = Priority
                        TagText = "Priority Bank Card"
                        Content =  priorityCardText
                        onClick = (fun v -> createCard v |> Async.StartImmediate)
                    }
                ]
            ]
        ]
    ]