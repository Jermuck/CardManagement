namespace App


module Routes = 
    open Feliz
    open Feliz.Router
    open CardManagement.Infrastructure.InputTypes
    open CardManagement.API.SharedTypes
    open App.Server
    open Feliz.Bulma

    type Page =
        | Home
        | Auth
        | NotFound

    let parseUrl = function
        | [ ] ->  Page.Home
        | ["auth"] -> Page.Auth
        | _ -> NotFound
        
        
    [<ReactComponent>]
    let Auth() =
        let (user, setUser) = React.useState { Name = ""; Age = 0; Email = ""; Salary = 0; Surname = ""; Patronymic = "" }
        let fetch _ = async {
            let api = createApiProxy()
            let! t = api.Register user
            match t with
            | Error msg -> printfn "Message %A" msg
            | Ok (a, b) -> printfn "User %A Token %A" a b
        }
        Html.div [
            Html.h1 "Auth"
            Html.input [
                prop.placeholder "Email"
                prop.onChange (fun e -> setUser { user with Email = e })
            ]
            Html.input [
                prop.placeholder "Name"
                prop.onChange (fun e -> setUser { user with Name = e })
            ]
            Html.input [
                prop.placeholder "Surname"
                prop.onChange (fun e -> setUser { user with Surname =  e })
            ]
            Html.input [
                prop.placeholder "Patronymic"
                prop.onChange (fun e -> setUser { user with Patronymic =  e })
            ]
            Html.input [
                prop.placeholder "Age"
                prop.type' "number"
                prop.onChange (fun (e:int) -> setUser { user with Age = e })
            ]
            Html.input [
                prop.placeholder "Salary"
                prop.type' "number"
                prop.onChange (fun (e:int) -> setUser { user with Salary =  e })
            ]
            Bulma.button.a [
                color.isWarning
                prop.text "Amazing button, ain't it?"
            ]
        ]

    [<ReactComponent>]
    let Router() =
        let (pageUrl, updateUrl) = React.useState(parseUrl(Router.currentUrl()))
        let currentPage =
            match pageUrl with
            | Home -> Html.h1 "Home"
            | Auth -> Auth()
            | NotFound -> Html.h1 "Not Found"

        React.router [
            router.pathMode
            router.onUrlChanged (parseUrl >> updateUrl)
            router.children currentPage
        ]