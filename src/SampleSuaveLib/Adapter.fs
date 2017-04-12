namespace SampleSuaveLib

open System.Web
open Suave.AspNetHttpModule
open App

type Handler() =
    interface IHttpModule with
        member __.Init(application: HttpApplication) =
            application.BeginRequest.Add(fun _ ->
                if tryRunWebPartFromContext app application.Context then
                    application.Context.Response.End()

            )

        member __.Dispose() = ignore()