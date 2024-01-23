module CardManagement.Client.Utils

open Feliz.Router

let navigate (path: string list) =
    path |> Router.formatPath |> Router.navigatePath