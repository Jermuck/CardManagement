namespace App

open CardManagement.API.SharedTypes
 
module Server = 
    open Fable.Remoting.Client
    
    let private builder typeName methodName =
       sprintf "http://localhost:5000/api/%s/%s" typeName methodName
    
    let privateStore =
        Remoting.createApi()
        |> Remoting.withRouteBuilder builder
        |> Remoting.buildProxy<IPrivateStore>
        
    let userStore = 
        Remoting.createApi()
        |> Remoting.withRouteBuilder builder
        |> Remoting.buildProxy<IUsersStore>
    