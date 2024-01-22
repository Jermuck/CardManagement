module CardManagement.Client.Pages.SettingsPage

open Feliz
open Feliz.Bulma
open Fable.Core.JS
open CardManagement.Client
open CardManagement.Shared
open Pages.AuthPage
open HomeHeaderComponent
open Inputs
open Types
open WebApi
open ErrorComponent

[<ReactComponent>]
let SettingsPage() =
    let user, setUser = React.useState initialStateUser
    let error, setError = React.useState<IMessage option> None
    
    let getSettings() = async {
        let! result = profileStore.GetMyProfile()
        match result with
        | Error _ -> ()
        | Ok v ->
            {
                Name = v.Name
                Surname = v.Surname
                Patronymic = v.Patronymic
                Password = v.Password
                Age = v.Age
                Salary = v.Salary
                Email = v.Email
            } |> setUser
    }
    
    let update() = async {
        try
            let! result = profileStore.UpdateProfile user
            match result with
            | Error error ->
                { Message = error.Message; Color = "#f14668" } |> Some |> setError
                setTimeout (fun _ -> setError None) 2000 |> ignore
            | Ok msg ->
                { Message = msg; Color = "#00d1b2" } |> Some |> setError
                setTimeout(fun _ -> Browser.Dom.window.location.reload()) 2000 |> ignore
        with
            | ex -> printfn "%A" ex;
    }
    
    React.useEffect((fun _ -> getSettings() |> Async.StartImmediate), [||])
    
    Html.div [
        prop.style [
            style.display.flex
            style.flexDirection.column
            style.height (length.vh 100)
        ]
        prop.children [
            HomeHeaderComponent()
            Html.div [
                prop.style [
                    style.flexGrow 1
                    style.display.flex
                    style.flexDirection.column
                    style.alignItems.center
                    style.justifyContent.center
                ]
                prop.children [
                    match error with
                    | Some error -> ErrorComponent error.Message 20 80 error.Color
                    | None -> Html.none
                    Html.h1 [
                        prop.text "Settings"
                        prop.style [
                            style.textAlign.center
                            style.fontSize 26
                        ]
                    ]
                    Html.form [
                        prop.style [
                            style.width 457
                            style.fontSize 12
                        ]
                        prop.children [
                            InputText "Your name" "Name" Text (fun v -> setUser { user with Name = v }) (Some user.Name)
                            InputText "Your surname" "Surname" Text (fun v -> setUser { user with Surname = v }) (Some user.Surname)
                            InputText "Your patronymic" "Patronymic" Text (fun v -> setUser { user with Patronymic =  v }) (Some user.Patronymic)
                            InputText "Your email" "Email" Text (fun v -> setUser { user with Email = v }) (Some user.Email)
                            InputNumber "Your age" "Age" (fun v -> setUser {user with Age = v }) (Some user.Age)
                            InputNumber "Your salary" "Salary" (fun v -> setUser { user with Salary = v }) (Some user.Salary)
                            Bulma.button.button [
                                prop.text "Save changes"
                                prop.style [
                                    style.backgroundColor "#3D70FF"
                                    style.color "white"
                                    style.width (length.perc 100)
                                ]
                                prop.onClick (fun e -> e.preventDefault(); update() |> Async.StartImmediate)
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]