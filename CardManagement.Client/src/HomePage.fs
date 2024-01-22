module CardManagement.Client.Pages.HomePage

open Feliz
open Feliz.Router
open CardManagement.Client
open CardManagement.Shared
open TimeLineComponent
open HomeHeaderComponent
open Types
open WebApi
open HomeBodyComponent
open Utils

let getCards() = async {
    try
        let! result = cardsStore.GetCards()
        match result with
        | Error _ -> return Seq.empty
        | Ok data -> return data
    with
        | ex -> printfn "%A" ex; return Seq.empty
}

[<ReactComponent>]
let HomeWithoutArgsPage() =
    let content, setContent = React.useState Html.none
    
    let getCardId() = async {
        let! cards = getCards()
        match Seq.isEmpty cards with
        | true -> TimeLineComponent() |> setContent
        | false ->
            let card = Seq.item 0 cards
            let route = Router.formatPath("home", [ "id", card.Id.ToString() ])
            Router.navigatePath route
    }
    
    React.useEffect((fun _ -> getCardId() |> Async.StartImmediate), [||])
    
    Html.div [
        prop.style [
            style.display.flex
            style.flexDirection.column
            style.height (length.vh 100)
        ]
        prop.children [
            HomeHeaderComponent()
            content
        ]
    ]
    
[<ReactComponent>]
let HomePage cardId =
    let content, setContent = React.useState Html.none
    
    let getComponent (data: Card seq) =
        match Seq.isEmpty data with
        | true -> navigate ["cards"; "create"]
        | false ->
            let isFind = data |> Seq.tryFind (fun v -> v.Id = cardId)
            match isFind with
            | None -> navigate [ "notfound" ]
            | Some v -> HomeBodyComponent data v |> setContent 
    
    let effect() = async {
        let! result = getCards()
        getComponent result
    }
    
    React.useEffect((fun _ -> effect() |> Async.StartImmediate), [|box cardId|])
    
    Html.div [
        prop.style [
            style.display.flex
            style.flexDirection.column
            style.height (length.vh 100)
        ]
        prop.children [
            HomeHeaderComponent()
            content
        ]
    ]