module SuaveTest

open Suave
open Suave.Filters
open Suave.Operators

let app = 
    choose [ path "/api/test" >=> Successful.OK "Hello world!" ]

    

