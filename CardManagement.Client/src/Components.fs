module CardManagement.Client.Components

open Feliz.Bulma
open Feliz

let InputText (placeholder: string) (label: string) (onInput: string -> unit) =
    Bulma.field.div [
        Bulma.label label
        Bulma.input.text [
            prop.placeholder placeholder
            prop.onChange onInput
        ]
    ]

let InputNumber (placeholder: string) (label: string) (onInput: int -> unit) =
    Bulma.field.div [
        Bulma.label label
        Bulma.input.number [
            prop.min 0
            prop.placeholder placeholder
            prop.onChange onInput
        ]
    ]
