module CardManagement.Client.HomeBodyComponent

open Fable.Import
open Feliz
open Feliz.Bulma
open Feliz.Router
open Fable.Core.JS
open CardManagement.Client
open CardManagement.Shared
open Types
open CardsDashboardComponent
open CardComponent
open CardActionButtonsComponent
open ChartComponent
open TransactionsTableComponent
open TransactionModalComponent
open WebApi
open ErrorComponent

[<ReactComponent>]
let HomeBodyComponent (cards: Card seq) (currentCard: Card) =
    let modalState, toggleState = React.useState false
    let error, setError = React.useState<IMessage option> None
    
    let changeCard id =
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
            let! result = cardsStore.CreateTransaction newTransactionInput
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
    
    let blockCard() = async {
        try
            let! result = cardsStore.BlockCard currentCard.Id
            match result with
            | Error _ -> ()
            | Ok _ ->
                Browser.Dom.window.location.reload()
        with
            | ex -> printfn "%A" ex; 
    }
        
    Html.div [
        prop.style [
            style.flexGrow 1
            style.display.flex
        ]
        prop.children [
            if error.IsSome then ErrorComponent error.Value.Message 20 20 error.Value.Color
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
                                    Html.div [
                                        if currentCard.Status = Deactivate
                                        then
                                            prop.style [
                                                style.opacity 0.2
                                                style.pointerEvents.none
                                            ]
                                        prop.children [
                                            CardComponent currentCard
                                        ]
                                    ]
                                    Html.div [
                                        if currentCard.Status = Deactivate
                                        then
                                            prop.style [
                                                style.opacity 0.2
                                                style.pointerEvents.none
                                            ]
                                        prop.children [
                                            CardActionButtonsComponent (fun _ -> toggleState true) (fun _ -> blockCard() |> Async.StartImmediate)
                                        ]
                                    ]
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