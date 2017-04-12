namespace SampleSuaveLib

open System.Web
open Suave.AspNetHttpModule

type Handler() =
    interface IHttpModule with
        member __.Init application = handleApplication App.app (* <-- your root web part *) application
        member __.Dispose() = ignore()