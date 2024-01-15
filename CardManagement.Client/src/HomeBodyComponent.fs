module CardManagement.Client.HomeBodyComponent

open CardManagement.Client.Types
open Feliz
open Feliz.Bulma
open CardManagement.Client.CardsDashboardComponent
open CardManagement.Client.CardComponent
open CardManagement.Client.CardActionButtonsComponent
open CardManagement.Client.ChartComponent
open CardManagement.Shared.Types
open CardManagement.Client.TransactionsTableComponent
open CardManagement.Client.TransactionModalComponent
open CardManagement.Client.WebApi

[<ReactComponent>]
let HomeBodyComponent (cards: Card seq) =
    let card, setCard = React.useState<Card>(Seq.item 0 cards)
    let modalState, toggleState = React.useState(false)
    
    let changeCard id =
        cards
        |> Seq.find (fun currentCard -> currentCard.Id = id)
        |> setCard
    
    let createTransaction (transactionInput: ITransactionModalComponent) = async {
        try
            let code = int64(transactionInput.Code)
            let newTransactionInput = {
                Message = transactionInput.Message
                Amount = transactionInput.Amount
                Code = code
                CardIdSender = transactionInput.CardIdSender
            }
            let! result = cardsStore.CreateTransaction newTransactionInput
            printfn "%A" result
        with
            | ex -> printfn "%A" ex; 
    }
        
    Html.div [
        prop.style [
            style.flexGrow 1
            style.display.flex
        ]
        prop.children [
            Bulma.modal [
                prop.id "modal-sample"
                if modalState then modal.isActive
                prop.children [
                    Bulma.modalBackground []
                    TransactionModalComponent card.Id (CardComponent card) (fun v -> createTransaction v |> Async.StartImmediate)
                    Bulma.modalClose [
                        prop.onClick (fun _ -> toggleState(false))
                    ]
                ]
            ]
            CardsDashboardComponent cards changeCard
            Html.div [
                prop.style [
                    style.width (length.perc 100)
                    style.display.flex
                    style.flexDirection.column
                    style.alignItems.center
                ]
                prop.children [
                    Html.div [
                        prop.style [
                            style.width (length.perc 95)
                            style.height.maxContent
                            style.display.flex
                            style.justifyContent.spaceBetween
                            style.marginTop 30
                        ]
                        prop.children [
                            Html.div [
                                prop.style [
                                    style.display.flex
                                    style.flexDirection.column
                                    style.justifyContent.spaceBetween
                                ]
                                prop.children [
                                    CardComponent card
                                    CardActionButtonsComponent (fun _ -> toggleState true)
                                ]
                            ]
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
                                            style.marginTop 10
                                            style.marginLeft 20
                                        ]
                                        prop.text "Insights"
                                    ]
                                    ChartComponent 650 250
                                ]
                            ]
                        ]
                    ]
                    TransactionsTableComponent()
                ]
            ]
        ]
    ]