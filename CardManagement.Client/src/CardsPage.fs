module CardManagement.Client.Pages.CardsPage

open System
open CardManagement.Client.CardFormComponent
open CardManagement.Client.CardComponent
open CardManagement.Client.ErrorComponent
open CardManagement.Client.HomeHeaderComponent
open Feliz
open Feliz.Router
open CardManagement.Shared.Types
open Fable.Core.JS
open CardManagement.Client.WebApi
open CardManagement.Client.Types

let priorityCardText = "The Priority Bank Card is a premium offering designed for customers who value exclusive benefits and personalized services. With this card, you can enjoy a range of privileges such as access to airport lounges, concierge services, travel insurance, and higher cashback or reward points on your purchases. The Priority Bank Card is tailored for individuals who frequently travel, dine out, or engage in luxury experiences. It offers enhanced security features and provides a higher credit limit to meet your financial needs. Experience the convenience and prestige of the Priority Bank Card."
let basicCardText = "The Basic Bank Card is a simple and straightforward option suitable for everyday banking needs. It provides essential features and functionality without any frills or additional perks. With this card, you can make purchases at various merchants, withdraw cash from ATMs, and manage your finances conveniently. The Basic Bank Card is ideal for individuals who prefer a no-nonsense approach to banking and want a reliable payment method for their day-to-day transactions. It offers ease of use, affordability, and peace of mind for your basic banking requirements."

[<ReactComponent>]
let CardsPage() =
    let error, setError = React.useState<IMessage option> None
    
    let basicCard: Card = {
        Id = Guid.NewGuid()
        Code = 1234
        CVV = 123
        UserId = Guid.NewGuid()
        TypeCard = Basic
        Balance = 1000
        Transactions = [] 
        LifeTime = DateTime.Now
        Status = Activate
    }
    
    let priorityCard = { basicCard with TypeCard = Priority; Balance = 100000 }
    
    let timeoutCallback id =
        Router.formatPath("home", [ "id", id.ToString() ])
        |> Router.navigatePath
        
    let createCard (typeCard: TypeOfCard) = async {
        try
            setError None
            let! newCard = createCardsStore().Create typeCard
            match newCard with
            | Error error ->
                { Message = error.Message; Color = "#f14668" } |> Some |> setError
                setTimeout (fun _ -> setError None) 2000 |> ignore
            | Ok v ->
                { Message = "Card was success create. Thanks!"; Color = "#00d1b2" } |> Some |> setError
                setTimeout(fun _ -> timeoutCallback v.Id) 2000 |> ignore
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
                    CardFormComponent {
                        CardElement = (CardComponent basicCard)
                        TypeCard = Basic
                        TagText = "Basic Bank Card"
                        Content = basicCardText
                        onClick = (fun v -> createCard v |> Async.StartImmediate)
                    } 
                    CardFormComponent {
                        CardElement = (CardComponent priorityCard)
                        TypeCard = Priority
                        TagText = "Priority Bank Card"
                        Content =  priorityCardText
                        onClick = (fun v -> createCard v |> Async.StartImmediate)
                    }
                ]
            ]
        ]
    ]