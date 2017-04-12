module Suave.AspNetHttpModule

open System.Web
open System.Collections.Specialized

open Suave
open Suave.Http
open System

let suaveHttpMethod (method : string) =
    match method.ToUpperInvariant() with
    | "GET" -> HttpMethod.GET
    | "PUT" -> HttpMethod.PUT
    | "POST" -> HttpMethod.POST
    | "DELETE" -> HttpMethod.DELETE
    | "PATCH" -> HttpMethod.PATCH
    | "HEAD" -> HttpMethod.HEAD
    | "TRACE" -> HttpMethod.TRACE
    | "CONNECT" -> HttpMethod.CONNECT
    | "OPTIONS" -> HttpMethod.OPTIONS
    | x -> HttpMethod.OTHER x

let suaveHeaders (headers : NameValueCollection) =
    headers.AllKeys
    |> List.ofArray
    |> List.fold (fun result key ->
        let value = headers.[key]
        result @ [key, value]) []

let tryRunWebPartFromContext app (context : Web.HttpContext) =
    let req =
        { HttpRequest.empty with
            headers  = suaveHeaders context.Request.Headers
            url = context.Request.Url
            method = suaveHttpMethod context.Request.HttpMethod }

    let ctx = { HttpContext.empty with request = req }

    let result =
        async {
            return! app ctx
        } |> Async.RunSynchronously

    match result with
    | Some (resultCtx : HttpContext) ->
        let contents =
            match resultCtx.response.content with
            | Bytes b -> b
            | _ -> invalidOp "Not handled yet"

        context.Response.BinaryWrite(contents)
        true

    | _ ->
        false

let handleApplication app (application: HttpApplication) =
    application.BeginRequest.Add(fun _ ->
        if tryRunWebPartFromContext app application.Context then
            application.Context.Response.End()
    )