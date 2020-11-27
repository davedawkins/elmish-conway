module GameOfLife.Types

type TickId = float option

type Cell = {
    X : int
    Y : int
    Alive : bool
}

type CellState =
    Cell array

type Model = {
    Tick : TickId
    Size : int
    Cells : CellState
    History : CellState list
}

type Msg =
    | SetTickId of TickId
    | Back
    | Start
    | Stop
    | NextRound of bool
    | ToggleCell of Cell
    | Reset
    | Fill

let cellAt model x y =
    //System.Console.WriteLine("x={0} y={1}", x, y)
    let index = y * model.Size + x
    model.Cells.[index]

let cellNeighbours model cell =
    let xy n = (n + model.Size) % model.Size
    [
        yield cellAt model (xy (cell.X-1)) (xy (cell.Y-1))
        yield cellAt model (xy (cell.X  )) (xy (cell.Y-1))
        yield cellAt model (xy (cell.X+1)) (xy (cell.Y-1))

        yield cellAt model (xy (cell.X-1)) (xy cell.Y)
        yield cellAt model (xy (cell.X+1)) (xy cell.Y)

        yield cellAt model (xy (cell.X-1)) (xy (cell.Y+1))
        yield cellAt model (xy (cell.X  )) (xy (cell.Y+1))
        yield cellAt model (xy (cell.X+1)) (xy (cell.Y+1))
    ]

let newCellState model cell =
    let alive = cellNeighbours model cell |> List.fold (fun n c -> n + if c.Alive then 1 else 0) 0
    //System.Console.WriteLine("Alive {0}", alive)
    if cell.Alive then
        alive >=2 && alive <= 3
    else
        alive = 3
