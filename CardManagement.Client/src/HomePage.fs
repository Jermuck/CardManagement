module CardManagement.Client.Pages.HomePage

open CardManagement.Shared.Types
open Feliz
open CardManagement.Client.Components
open CardManagement.Client.WebApi
open CardManagement.Client.Pages.CardsPage
open Feliz.UseDeferred
open CardManagement.Client.Pages.LoadingPage

[<ReactComponent>]
let HomePage() =
    let getComponent data =
        match Seq.isEmpty data with
        | true -> TimeLineComponent()
        | false -> CardsDashboard data
    
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
            style.height (length.vh 100)
            style.display.flex
            style.flexDirection.column
        ]
        prop.children [
            HomeHeaderComponent()
            content
        ]
    ]
