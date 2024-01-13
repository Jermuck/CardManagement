module CardManagement.Client.Pages.CardsPage

open System
open Feliz
open CardManagement.Client.Components
open CardManagement.Shared.Types
open Feliz.Bulma
open Fable.Core.JS
open CardManagement.Client.WebApi
open CardManagement.Client.Types

let priorityCardText = "The Priority Bank Card is a premium offering designed for customers who value exclusive benefits and personalized services. With this card, you can enjoy a range of privileges such as access to airport lounges, concierge services, travel insurance, and higher cashback or reward points on your purchases. The Priority Bank Card is tailored for individuals who frequently travel, dine out, or engage in luxury experiences. It offers enhanced security features and provides a higher credit limit to meet your financial needs. Experience the convenience and prestige of the Priority Bank Card."
let basicCardText = "The Basic Bank Card is a simple and straightforward option suitable for everyday banking needs. It provides essential features and functionality without any frills or additional perks. With this card, you can make purchases at various merchants, withdraw cash from ATMs, and manage your finances conveniently. The Basic Bank Card is ideal for individuals who prefer a no-nonsense approach to banking and want a reliable payment method for their day-to-day transactions. It offers ease of use, affordability, and peace of mind for your basic banking requirements."

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
let CardsPage() =
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
            HomeHeaderComponent()
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
    
[<ReactComponent>]
let CardsDashboard (data: Card seq) =
    let cards, setCards = React.useState data
    let headers, setHeader = React.useState(seq [
        { Id = Guid.NewGuid(); Text = "All"; ClassName = "is-active"; Type = All }
        { Id = Guid.NewGuid(); Text = "Basic Cards"; ClassName = ""; Type = BasicCard }
        { Id = Guid.NewGuid(); Text = "Priority Cards"; ClassName = ""; Type = PriorityCard }
    ])
    
    let sortHeaders header =
        let changeClassName (currentHeader: IHeadersTabs) =
            match currentHeader.Id = header.Id with
            | true -> { currentHeader with ClassName = "is-active" }
            | false -> { currentHeader with ClassName = "" }
        
        let test (card: Card) =
            let sortingArgs =
                match header.Type with
                | All -> seq [ Priority; Basic ]
                | BasicCard -> seq [ Basic ]
                | PriorityCard -> seq [ Priority ]
            sortingArgs |> Seq.contains card.TypeCard
            
        headers
        |> Seq.map changeClassName
        |> setHeader
        
        data
        |> Seq.filter test
        |> setCards
    
    Bulma.panel [
        prop.style [
            style.width (length.perc 30)
            style.height (length.vh 100)
            style.borderRadius 0
        ]
        prop.children [
            Bulma.panelHeading [
                prop.text "My cards"
                prop.style [
                    style.color "black"
                    style.fontWeight 400
                    style.backgroundColor "white"
                ]
            ]
            Bulma.panelBlock.div [
                Bulma.control.div [
                    prop.children [
                        Bulma.input.text [ prop.placeholder "Search" ]
                    ]
                ]
            ]
            Bulma.panelTabs [
                for header in headers do
                    Html.a [
                        prop.onClick (fun _ -> sortHeaders header)
                        prop.className header.ClassName
                        prop.text header.Text
                    ]
            ]
            for card in cards do
                Bulma.panelBlock.div [
                    prop.className "is-active"
                    prop.children [
                        Bulma.panelIcon [
                            prop.style [
                                style.width 32
                                style.height 20
                                style.display.flex
                                style.justifyContent.center
                                style.alignItems.center
                                style.backgroundColor "#F3F6FA"
                            ]
                            prop.children [
                                Html.img [
                                    prop.src ".././img/Small_Icon_Visa.svg"
                                ]
                            ]
                        ]
                        Html.span [
                            prop.text (convertCardToPoint(card.Code.ToString()))
                            prop.style [
                                style.width (length.perc 100)
                                style.marginLeft 10
                            ]
                        ]
                        Bulma.tag [
                            Bulma.color.isPrimary
                            Bulma.color.isLight
                            prop.text (card.TypeCard.ToString())
                        ]
                    ]
                ]
        ]
    ]