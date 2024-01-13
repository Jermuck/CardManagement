module CardManagement.Client.Inputs

open Feliz
open Feliz.Bulma
open CardManagement.Client.Types

[<ReactComponent>]
let InputText (placeholder: string) (label: string) (type': InputType) (onInput: string -> unit) =
    Bulma.field.div [
        Bulma.label label
        Bulma.input.text [
            prop.type' (type'.ToString().ToLower())
            prop.placeholder placeholder
            prop.onChange onInput
        ]
    ]

[<ReactComponent>]
let InputNumber (placeholder: string) (label: string) (onInput: int -> unit) =
     Bulma.field.div [
        Bulma.label label
        Bulma.input.text [
            prop.min 0
            prop.type'.number
            prop.placeholder placeholder
            prop.onChange onInput
        ]
    ]