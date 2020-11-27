# React Elmish Fable Game Of Life

This is an F# / Elmish implementation of Conway's Game of Life.

This was an exercise in learning the libraries around Fable, rather than an in-depth study of Conway's Life, so this is me trying to see how UIs are implemented in Fable Elmish React.

The UI is using MaterialUI in form of `Feliz.MaterialUI`. This was a challenge in itself. The libraries themselves are excellent, but there's Feliz-style to learn, and then I threw MaterialUI
into the mix too. There was no need to do this, I could have just stuck with regular React format and Bootstrap (or even Bulma in the form of Fulma).

The simulation is generating a whole new world on each tick. I'm interested in finding the best idiomatic and functional way of implementing programs like this. One approach I want to try is to have a single world, and then allow the simulation to generate a list of "Toggle Cell" actions on each iteration, that are then applied by a top-level function, mutating the world according to the actions. The main simulation remains pure for each tick, and mutation is applied in a controlled manner, emulating Haskell's IO monad to a degree. My hope would be that this would give a speed improvement, and reduce the amount of memory turnover.