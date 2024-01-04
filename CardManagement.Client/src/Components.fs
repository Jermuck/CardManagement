namespace App

module Components =
    open Feliz.Bulma
    open Feliz
    open CardManagement.Infrastructure.DomainModels
    
    type InputType =
        | Number
        | Text
        | Email
    
    let rec Input (placeholder: string) (label: string) (type': InputType) onInput =
        Bulma.field.div [
            Bulma.label label
            Bulma.input.text [
                prop.type' (type'.ToString())
                prop.min 0
                prop.placeholder placeholder
                prop.onInput onInput
            ]
        ]
        

