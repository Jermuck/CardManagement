module CardManagement.Client.TransactionsTableComponent

open Feliz
open Feliz.DaisyUI

[<ReactComponent>]
let TransactionsTableComponent() =
    Html.div [
        prop.style [
            style.width (length.perc 95)
            style.height (length.perc 100)
            style.backgroundColor "#FAFAFB"
            style.borderRadius 12
            style.margin 20
        ]
        prop.children [
            Daisy.table [
                prop.style [
                    style.backgroundColor "#FAFAFB"
                    style.width (length.perc 100)
                    style.borderRadius 12
                ]
                prop.children [
                    Html.thead [
                        Html.tr [
                            Html.th "Card code"
                            Html.th "Id"
                            Html.th "Amount"
                            Html.th "Status"
                            Html.th "Date"
                        ]
                    ]
                    Html.tbody [
                        Html.tr [
                            prop.style [
                                style.height 20
                            ]
                            for a in 1..20 do
                                prop.children [
                                    Html.td "1"
                                    Html.td "Cy Ganderton"
                                    Html.td "Quality Control Specialist"
                                    Html.td "Active"
                                    Html.td "Blue"
                                ]
                        ]
                    ]
                ]
            ]
        ]
    ]