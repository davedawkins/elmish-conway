module GameOfLife.View

open Elmish
open Fable.React
open Feliz.MaterialUI
open Feliz
open Browser.Event

open Types
open GameOfLife.State

let private useStyles = Styles.makeStyles(fun styles theme ->
  let size = 1
  let wh = 12 // (theme.spacing size)
  {|
    description = styles.create [
        style.marginBottom 12
    ]
    cellAlive = styles.create [
        style.width wh
        style.height wh
        style.backgroundColor "green"
        style.borderTop(1,borderStyle.dotted,"#cccccc")
        style.borderRight(1,borderStyle.dotted,"#cccccc")
    ]
    cellDead = styles.create [
        style.width wh
        style.height wh
        style.backgroundColor "white"
        style.borderTop(1,borderStyle.dotted,"#cccccc")
        style.borderRight(1,borderStyle.dotted,"#cccccc")
    ]
  |}
)

let viewCell dispatch cell =
    let c = useStyles()
    Mui.grid [
        grid.item true
        grid.children [
            Html.div [
                prop.className (if cell.Alive then c.cellAlive else c.cellDead)
                prop.onMouseDown (fun e -> ToggleCell cell |> dispatch)
                prop.onMouseEnter (fun e -> if (e.buttons > 0.0) then ToggleCell cell |> dispatch)
            ]
        ]
    ]

let viewRow model dispatch y =
    let elements = [| 0 .. (model.Size-1) |]
                    |> Array.map (fun x -> cellAt model x y |> viewCell dispatch)
    Mui.grid [
        grid.container true
        grid.item true
        grid.spacing._0
        grid.children elements
        grid.direction.row
    ]

let app = FunctionComponent.Of( (fun (model,dispatch) ->
    let rows = [| 0 .. (model.Size-1) |]
                |> Array.map (viewRow model dispatch)
    Mui.grid [
        grid.container true
        grid.direction.column
        grid.spacing._2
        grid.children [
            Mui.grid [
                grid.container true
                grid.item true
                grid.children rows
                grid.spacing._0
                grid.direction.column
            ]
            Mui.grid [
                grid.item true
                grid.children [
                    Mui.toolbar [
                        toolbar.disableGutters true
                        toolbar.children [
                            Mui.button [
                                prop.style [ style.marginRight 14 ]
                                button.variant.text
                                button.size.small
                                button.children (if isRunning model then  "Stop" else "Start")
                                prop.onClick
                                                (if isRunning model then
                                                    (fun e -> dispatch Stop)
                                                else
                                                    (fun e -> dispatch Start))
                            ]
                            Mui.button [
                                prop.style [ style.marginRight 14 ]
                                button.variant.text
                                button.size.small
                                button.children "Reset"
                                prop.onClick (fun e -> dispatch Reset)
                            ]
                            (*
                            Mui.button [
                                prop.style [ style.marginRight 14 ]
                                button.variant.text
                                button.size.small
                                button.children "Back"
                                prop.onClick (fun e -> dispatch Back)
                            ]
                            Mui.button [
                                prop.style [ style.marginRight 14 ]
                                button.variant.text
                                button.size.small
                                button.children "Next"
                                prop.onClick (fun e -> dispatch <| NextRound true)
                            ]
                            Mui.button [
                                prop.style [ style.marginRight 14 ]
                                button.variant.text
                                button.size.small
                                button.children "Fill"
                                prop.onClick (fun e -> dispatch Fill)
                            ]
                            *)
                        ]
                    ]
                ]
            ]
        ]
    ]), "GameOfLifeView", memoEqualsButFunctions
)

let viewContainer model dispatch =
    Mui.container [
        container.maxWidth.sm
        container.children [
            Mui.typography [
                prop.style [
                    style.marginTop 30
                    style.marginBottom 24
                ]
                typography.children [
                    Mui.typography [
                        typography.component' "h5"
                        typography.variant.h5
                        typography.children "Conway's Game of Life"
                    ]
                    Mui.typography [
                        typography.component' "body2"
                        typography.variant.body2
                        typography.children "An F# Elmish implementation of Life."
                    ]
                    Mui.typography [
                        typography.component' "body2"
                        typography.variant.body2
                        typography.children "Click or drag in the grid to create an initial state."
                    ]
                ]
            ]
            app(model, dispatch)
        ]
    ]

let view model dispatch =
    viewContainer model dispatch