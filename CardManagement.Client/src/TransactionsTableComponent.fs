module CardManagement.Client.TransactionsTableComponent

open System
open Feliz
open Feliz.Bulma
open Feliz.DaisyUI
open Feliz.UseDeferred
open CardManagement.Client
open CardManagement.Shared
open WebApi
open Types
open Utils

[<ReactComponent>]
let StatusTransactionComponent isMoneyIn =
    Bulma.tag [
        match isMoneyIn with
        | true ->
            Bulma.color.isPrimary
            prop.text "Money In" 
        | false ->
            Bulma.color.isDanger
            prop.text "Money Out" 
    ]

[<ReactComponent>]
let TransactionsTableComponent cardId =
    let getTransactions() = async {
        try
            let! result = cardsStore.GetTransactions cardId
            match result with
            | Error _ -> return Seq.empty
            | Ok value -> return value
        with
            | ex -> printfn "%A" ex; return Seq.empty
    }
    
    let data = React.useDeferred(getTransactions(), [|box cardId|])
    
    let convertDate (date: DateTime) =
        let day = date.Day.ToString()
        let month = getStringMonth date
        let year = date.Year.ToString()
        day + " " + month + " " + year
        
    let convertSumTransaction isMoneyIn sum=
        match isMoneyIn with
        | true -> "+$" + sum.ToString() + ",00"
        | false -> "-$" + sum.ToString() + ",00"
    
    let convertMessage (message: string) =
        if message.Length > 10 then message[0..12] + "..."
        else if message.Length = 0 then "None"
        else message
    
    let table =
        match data with
        | Deferred.Resolved transactions ->
            let result = transactions
                          |> Seq.sortBy (fun v -> v.CreateDate)
                          |> Seq.indexed
            match Seq.isEmpty result with
            | true ->
                Html.div [
                    prop.style [
                        style.width (length.perc 100)
                        style.height (length.perc 100)
                        style.display.flex
                        style.justifyContent.center
                        style.alignItems.center
                    ]
                    prop.children [
                        Html.h1 "Empty table"
                    ]
                ] 
            | false -> 
                Daisy.table [
                    prop.style [
                        style.backgroundColor "#FAFAFB"
                        style.width (length.perc 100)
                        style.borderRadius 12
                    ]
                    prop.children [
                        Html.thead [
                            Html.tr [
                                Html.th "Id"
                                Html.th "Amount"
                                Html.th "Status"
                                Html.th "Date"
                                Html.th "Message"
                            ]
                        ]
                        Html.tbody [
                            for index, transaction in result do
                                Html.tr [
                                    prop.children [
                                        Html.td (index+1)
                                        Html.td (convertSumTransaction (cardId <> transaction.CardId) transaction.Sum )
                                        Html.td [StatusTransactionComponent (cardId <> transaction.CardId)]
                                        Html.td (convertDate transaction.CreateDate)
                                        Html.td (convertMessage transaction.Message)
                                    ]
                                ]    
                        ]
                    ]
                ]
        | _ -> Html.none
        
    Html.div [
        prop.style [
            style.width (length.perc 95)
            style.height (length.perc 100)
            style.backgroundColor "#FAFAFB"
            style.borderRadius 12
            style.margin 20
        ]
        prop.children [
            table
        ]
    ]