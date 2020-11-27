module GameOfLife.State

open Types
open Elmish
open Browser

let startTick (ms : int) (handler : unit -> unit) : TickId =
    Some <| window.setInterval( handler, ms )

let stopTick (id : TickId) =
    match id with
    | Some n -> window.clearInterval(n)
    | _ -> ()

let isRunning model = model.Tick.IsSome

let initWith alive =
    let size = 32
    let mkcell x y = { Alive = alive; X=x; Y=y }
    {
        Tick = None
        Size = size
        History = []
        Cells = [|
            for y in 0 .. (size-1) do
            for x in 0 .. (size-1) do
                mkcell x y
            |]
        }, Cmd.none

let init() = initWith false

let sameCell c1 c2 =
    c1.X = c2.X && c1.Y = c2.Y

let updateCell model cell =
    { model with
        Cells = [| for c in model.Cells do if (sameCell c cell) then cell else c |]
        History = [] }

let iterateModel model keepHistory=
    let updateCell c = { c with Alive = newCellState model c }
    { model with
        History = if keepHistory then model.Cells :: model.History else  model.History
        Cells   = model.Cells |> Array.map updateCell
    }

let update message model =
    match message with
    | SetTickId id -> { model with Tick = id }, Cmd.none
    | Back ->
        match model.History with
        | [] -> model, Cmd.none
        | last :: tail -> { model with Cells = last; History = tail }, Cmd.none
    | Start ->
        let startTicking dispatch =
            let tickId = startTick 100 (fun () -> dispatch <| NextRound false)
            dispatch (SetTickId tickId)
        model, Cmd.ofSub startTicking
    | Stop ->
        let stopTicking dispatch =
            stopTick model.Tick
            dispatch (SetTickId None)
        model, Cmd.ofSub stopTicking
    | Reset ->
        let state,_ = init()
        { state with Tick = model.Tick }, Cmd.ofMsg Stop
    | Fill ->
        initWith true
    | NextRound keepHistory ->
        iterateModel model keepHistory, Cmd.none
    | ToggleCell cell ->
        updateCell model { cell with Alive = not cell.Alive }, Cmd.none
