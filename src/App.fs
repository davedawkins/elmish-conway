module App

open Elmish
open Elmish.React

Program.mkProgram GameOfLife.State.init GameOfLife.State.update GameOfLife.View.view
|> Program.withReactSynchronous "elmish-app"
|> Program.run