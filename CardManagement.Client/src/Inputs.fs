module CardManagement.Client.Inputs

open Feliz
open Feliz.Bulma
open CardManagement.Client.Types

[<ReactComponent>]
let InputText (placeholder: string) (label: string) (type': InputType) (onInput: string -> unit) (value: string option) =
    Bulma.field.div [
        Bulma.label label
        Bulma.input.text [
            prop.type' (type'.ToString().ToLower())
            if value.IsSome then prop.value value.Value
            prop.placeholder placeholder
            prop.onChange onInput
        ]
    ]

[<ReactComponent>]
let InputNumber (placeholder: string) (label: string) (onInput: int -> unit) (value: int option) =
     Bulma.field.div [
        Bulma.label label
        Bulma.input.text [
            prop.min 0
            prop.type'.number
            if value.IsSome then prop.value value.Value
            prop.placeholder placeholder
            prop.onChange onInput
        ]
    ]