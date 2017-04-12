module Program

open Suave
open SampleSuaveLib.App

[<EntryPoint>]
let main _ =
    startWebServer defaultConfig app
    0