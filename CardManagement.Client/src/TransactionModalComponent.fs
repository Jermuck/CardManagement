module CardManagement.Client.TransactionModalComponent

open Feliz
open Feliz.Bulma
open Feliz.DaisyUI
open CardManagement.Client.Types

[<ReactComponent>]
let TransactionModalComponent cardId cardComponent (onClick: ITransactionModalComponent -> unit) =
    let code, setCode = React.useState ""
    let amount, setAmount = React.useState 0
    let message, setMessage = React.useState ""
    
    Bulma.modalContent [
        prop.style [
            style.width (length.perc 28)
            style.borderRadius 12
        ]
        prop.children [
            Bulma.box [
                prop.style [
                    style.display.flex
                    style.flexDirection.column
                    style.alignItems.center
                ]
                prop.children [
                    Html.h1 [
                        prop.style [
                            style.textAlign.center
                            style.fontSize 22
                        ]
                        prop.text "Translate"
                    ]
                    Html.div [
                        prop.style [
                            style.marginTop 20
                        ]
                        prop.children [
                            cardComponent
                            Bulma.field.div [
                                prop.style [
                                    style.marginTop 20
                                ]
                                prop.children [
                                    Bulma.label [
                                        prop.style [
                                            style.fontWeight 400
                                        ]
                                        prop.text "Card code"
                                    ]
                                    Bulma.control.div [
                                        Bulma.input.text [
                                            prop.onChange setCode
                                            prop.placeholder "•••• •••• •••• ••••"
                                        ]
                                    ]
                                    
                                ]
                            ]
                            Bulma.field.div [
                                prop.style [
                                    style.marginTop 20
                                ]
                                prop.children [
                                    Bulma.label [
                                        prop.style [
                                            style.fontWeight 400
                                        ]
                                        prop.text "Amount"
                                    ]
                                    Bulma.control.div [
                                        Bulma.input.number [
                                            prop.placeholder "from 10$ to 99 999$"
                                            prop.onChange setAmount
                                        ]
                                    ]
                                ]
                            ]
                            Daisy.formControl [
                                Daisy.label [
                                    Daisy.labelText [
                                        prop.style [
                                            style.fontWeight 400
                                        ]
                                        prop.text "Message"
                                    ]
                                ]
                                Daisy.textarea [
                                    prop.placeholder "Message to the recipient"
                                    prop.className "h-24"
                                    prop.onChange setMessage
                                    textarea.bordered
                                ]
                            ]
                            Bulma.button.button [
                                Bulma.color.isPrimary
                                prop.style [
                                    style.marginTop 20
                                    style.width (length.perc 100)
                                ]
                                prop.onClick (fun _ -> onClick {
                                                Message = message;
                                                Amount = amount;
                                                Code = code;
                                                CardIdSender = cardId
                                                })
                                prop.text "Send"
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]