namespace App 
module Server = 
    open Fable.Remoting.Client
    open CardManagement.API.SharedTypes
    
    let private builder typeName methodName =
       sprintf "http://localhost:5000/api/%s/%s" typeName methodName
    
    let createApiProxy() = 
        Remoting.createApi()
        |> Remoting.withRouteBuilder builder
        |> Remoting.buildProxy<IUsersStore>

