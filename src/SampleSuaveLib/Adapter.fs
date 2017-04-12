namespace SampleSuaveLib

open System.Web
open Suave.AspNetHttpModule
open App

type Handler() =
    interface IHttpModule with
        member this.Init(application: HttpApplication) =
            application.BeginRequest.Add(fun _ ->
                if tryRunWebPartFromContext app application.Context then
                    application.Context.Response.End()

            )

        member this.Dispose() = ignore()