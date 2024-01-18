module CardManagement.Client.Pages.HomePage

open CardManagement.Client.HomeHeaderComponent
open CardManagement.Client.TimeLineComponent
open CardManagement.Shared.Types
open Feliz
open CardManagement.Client.WebApi
open CardManagement.Client.Pages.LoadingPage
open CardManagement.Client.HomeBodyComponent

[<ReactComponent>]
let HomePage cardId =
    let content, setContent = React.useState<Fable.React.ReactElement> Html.none
    
    let getComponent (data: Card seq) =
        match Seq.isEmpty data with
        | true -> TimeLineComponent() |> setContent
        | false ->
            data
            |> Seq.find (fun v -> v.Id = cardId)
            |> HomeBodyComponent data
            |> setContent
    
    let getCards() = async {
        try
            let! result = createCardsStore().Get()
            match result with
            | Error _ -> LoadingPage() |> setContent
            | Ok data -> getComponent data
        with
            | ex -> printfn "%A" ex; LoadingPage() |> setContent
    }
    
    React.useEffect((fun _ -> getCards() |> Async.StartImmediate), [|box cardId|])
    
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
