module CardManagement.Client.Pages

open CardManagement.Shared.Types
open CardManagement.Shared.Core
open Feliz
open Feliz.Bulma
open Components
open WebApi

[<ReactComponent>]
let Auth() =
    let user, setUser = React.useState { Name = ""; Surname = ""; Patronymic = ""; Password = ""; Age = 0; Salary = 0; Email = "" }
    let error, setError = React.useState ""
    
    let next (e:Browser.Types.MouseEvent) = async {
        e.preventDefault()
        setError ""
        let! result = userStore.Register user
        match result with
        | Ok (_, token) ->
            Browser.WebStorage.localStorage.setItem("token", token.Token)
        | Error err -> setError err.Message
    }
    
    let login (e:Browser.Types.MouseEvent) = async {
        e.preventDefault()
        let! t = privateStore.Get()
        printfn "%A" t
        ()
    }
    
    Html.div [
        prop.children [
            Html.header [
                prop.children [
                    Html.h1 [
                        prop.text "Bank"
                        prop.style [
                            style.color "white"
                            style.fontWeight 400
                            style.fontSize 26
                            style.marginLeft 10
                        ]
                    ]
                    Html.img [
                        prop.src "./img/Icon_Visa.svg"
                        prop.style [
                            style.marginRight 10
                        ]
                    ]
                    
                ]
                prop.style [
                    style.height 50
                    style.backgroundColor "#3D70FF"
                    style.display.flex
                    style.alignItems.center
                    style.justifyContent.spaceBetween
                ]
            ]
            Html.div [
                prop.style [
                    style.display.flex
                    style.flexDirection.column
                    style.justifyContent.center
                    style.alignItems.center
                    style.position.relative
                ]
                prop.children [
                    if error.Length > 0 then
                        Html.div [
                            prop.text error
                            prop.style [
                                style.padding 10
                                style.backgroundColor "#f14668"
                                style.color "white"
                                style.borderRadius 10
                                style.position.absolute
                                style.right 20
                                style.top 20
                            ]
                        ]
                    Html.h1 [
                        prop.text "Registration"
                        prop.style [
                            style.color "black"
                            style.marginTop 10
                            style.fontSize 25
                        ]
                    ]
                    Html.form [
                        prop.style [
                            style.width 457
                            style.fontSize 12
                        ]
                        prop.children [
                            InputText "Your name" "Name" (fun v -> setUser { user with Name = v }) 
                            InputText "Your surname" "Surname" (fun v -> setUser { user with Surname = v })
                            InputText "Your patronymic" "Patronymic" (fun v -> setUser { user with Patronymic =  v })
                            InputText "Your email" "Email" (fun v -> setUser { user with Email = v })
                            InputText "Your password" "Password" (fun v -> setUser { user with Password = v })
                            InputText "Repeat your password" "Repeat Password" (fun v -> setUser { user with Password =  v })
                            InputNumber "Your age" "Age" (fun v -> setUser {user with Age = v})
                            InputNumber "Your salary" "Salary" (fun _ -> ())
                            Html.div [
                                prop.style [
                                    style.display.flex
                                    style.justifyContent.spaceBetween
                                ]
                                prop.children [
                                    Bulma.button.button [
                                        prop.text "Next"
                                        prop.style [
                                            style.backgroundColor "#3D70FF"
                                            style.color "white"
                                        ]
                                        prop.onClick (fun e -> next e |> Async.StartImmediate)
                                    ]
                                    Bulma.button.button [
                                        prop.text "Login"
                                        prop.onClick (fun e -> login e |> Async.StartImmediate)
                                        prop.style [
                                            style.backgroundColor "#3D70FF"
                                            style.color "white"
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]    
    
