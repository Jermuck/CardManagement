module CardManagement.Client.HomeBodyComponent

open System
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
open CardManagement.Client.ErrorComponent
open Feliz.Router
open Fable.Core.JS

[<ReactComponent>]
let HomeBodyComponent (cards: Card seq) (currentCard: Card) =
    let modalState, toggleState = React.useState false
    let error, setError = React.useState<IMessage option> None
    
    let changeCard (id: Guid) =
        Router.formatPath("home", [ "id", id.ToString() ])
        |> Router.navigatePath
        
    let createTransaction (transactionInput: ITransactionModalComponent) = async {
        try
            let code = int64(transactionInput.Code)
            let newTransactionInput = {
                Message = transactionInput.Message
                Amount = transactionInput.Amount
                Code = code
                CardIdSender = transactionInput.CardIdSender
            }
            let! result = createCardsStore().CreateTransaction newTransactionInput
            match result with
            | Error error ->
                { Message = error.Message; Color = "#f14668" } |> Some |> setError
                setTimeout (fun _ -> setError None) 2000 |> ignore
            | Ok _ ->
                { Message = "Success transaction!"; Color = "#00d1b2" } |> Some |> setError
                setTimeout (fun _ -> setError None; Browser.Dom.window.location.reload()) 2000 |> ignore
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
                    Bulma.modalBackground [
                        prop.children [
                            if error.IsSome then ErrorComponent error.Value.Message 20 20 error.Value.Color
                        ]
                    ]
                    TransactionModalComponent currentCard.Id (CardComponent currentCard) (fun v -> createTransaction v |> Async.StartImmediate)
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
                                    CardComponent currentCard 
                                    CardActionButtonsComponent (fun _ -> toggleState true)
                                ]
                            ]
                            Html.div [
                                prop.style [
                                    style.backgroundColor "#FAFAFB"
                                    style.borderRadius 12
                                    style.width 650
                                    style.height 300
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
                                    ChartComponent 650 250 currentCard.Id
                                ]
                            ]
                        ]
                    ]
                    TransactionsTableComponent currentCard.Id
                ]
            ]
        ]
    ]