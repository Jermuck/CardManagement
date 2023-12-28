module Main

open Feliz
open App.Routes
open Browser.Dom
let root = ReactDOM.createRoot(document.getElementById "feliz-app")
root.render(Router())