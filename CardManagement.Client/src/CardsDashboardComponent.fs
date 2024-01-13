module CardManagement.Client.CardsDashboardComponent

open System
open CardManagement.Client.Types
open CardManagement.Shared.Types
open CardManagement.Client.CardComponent
open Feliz.Bulma
open Feliz

[<ReactComponent>]
let CardsDashboardComponent (data: Card seq) =
    let cards, setCards = React.useState data
    let headers, setHeader = React.useState(seq [
        { Id = Guid.NewGuid(); Text = "All"; ClassName = "is-active"; Type = All }
        { Id = Guid.NewGuid(); Text = "Basic Cards"; ClassName = ""; Type = BasicCard }
        { Id = Guid.NewGuid(); Text = "Priority Cards"; ClassName = ""; Type = PriorityCard }
    ])
    let currentHeader, setCurrentHeader = React.useState(Seq.item 0 headers)
    let input, setInput = React.useState ""
        
    let sortHeaders header =
        setInput ""
        setCurrentHeader header
        
        let changeClassName (currentHeader: IHeadersTabs) =
            match currentHeader.Id = header.Id with
            | true -> { currentHeader with ClassName = "is-active" }
            | false -> { currentHeader with ClassName = "" }
        
        let sortCards (card: Card) =
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
        |> Seq.filter sortCards
        |> setCards
    
    let search (value: string) =
        setInput value
        if value.Length > 0 then
            cards
            |> Seq.filter (fun card -> card.Code.ToString().Contains value)
            |> setCards
        else
            sortHeaders currentHeader
    
    Bulma.panel [
        prop.style [
            style.width (length.perc 30)
            style.height (length.perc 100)
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
                        Bulma.input.text [
                            prop.value input
                            prop.onChange search
                            prop.placeholder "Search"
                        ]
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