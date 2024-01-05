module CardManagement.Client.Main

open Feliz
open Routes
open Browser.Dom

let root = ReactDOM.createRoot(document.getElementById "feliz-app")
root.render(Router())