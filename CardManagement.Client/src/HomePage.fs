module CardManagement.Client.Pages.HomePage

open CardManagement.Client.HomeHeaderComponent
open CardManagement.Client.TimeLineComponent
open CardManagement.Shared.Types
open Feliz
open CardManagement.Client.WebApi
open Feliz.UseDeferred
open CardManagement.Client.Pages.LoadingPage
open CardManagement.Client.HomeBodyComponent

[<ReactComponent>]
let HomePage() =
    let getComponent data =
        match Seq.isEmpty data with
        | true -> TimeLineComponent()
        | false -> HomeBodyComponent data
    
    let getCards() = async {
        try
            let! result = cardsStore.Get()
            match result with
            | Error _ -> return LoadingPage()
            | Ok data -> return getComponent data
        with
            | ex -> printfn "%A" ex; return LoadingPage()
    }
    
    let data = React.useDeferred(getCards(), [| |])
    
    let content =
        match data with
        | Deferred.Failed error -> Html.div error.Message
        | Deferred.Resolved result -> result
        | _ -> LoadingPage()
    
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
