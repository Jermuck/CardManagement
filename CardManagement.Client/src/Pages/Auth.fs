namespace App

module Pages =
    open Feliz
    open Components
    
    [<ReactComponent>]
    let Auth() =
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
                    ]
                    prop.style [
                        style.height 70
                        style.backgroundColor "#3D70FF"
                        style.display.flex
                        style.alignItems.center
                    ]
                ]
                Html.div [
                    prop.style [
                        style.display.flex
                        style.justifyContent.center
                        style.alignItems.center
                    ]
                    prop.children [
                        Html.form [
                            prop.style [
                                style.width 600
                                style.fontSize 12
                            ]
                            prop.children [
                                Input "Your name" "Name" Text (fun _ -> ()) 
                                Input "Your surname" "Surname" Text (fun _ -> ())
                                Input "Your patronymic" "Patronymic" Text (fun _ -> ())
                                Input "Your email" "Email" Email (fun _ -> ())
                                Input "Your password" "Password" Text (fun _ -> ())
                                Input "Repeat your password" "Repeat Password" Text (fun _ -> ())
                                Input "Your age" "Age" Number (fun _ -> ())
                                Input "Your salary" "Salary" Number (fun _ -> ())
                                Input "Your password" "Password" Text (fun _ -> ())
                            ]
                        ]
                    ]
                ]
            ]
        ]    
        
