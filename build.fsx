#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

let buildDir = "./Build/"
let version = "0.0.1"

Target "Clean" (fun _ ->
    CleanDirs [ buildDir; "src/SuaveAspNet/bin" ]
)

Target "AssemblyInfo" (fun _ ->
    let attributes =
        [ Attribute.Version version
          Attribute.FileVersion version
          Attribute.Title "Suave.AspNetHttpModule" ]

    CreateFSharpAssemblyInfo ("src/SuaveAspNet" </> "AssemblyInfo.fs") attributes
)

Target "Build" (fun _ ->
    !! "src/SuaveAspNet/Suave.AspNetHttpModule.fsproj"
    |> MSBuildRelease "" "Build"
    |> Log "Build-Output: "
)

Target "NuGet" (fun _ ->
    Paket.Pack(fun p ->
        { p with
            OutputPath = buildDir
            Version = version })
)

Target "Default" (fun _ ->
    ignore()
)

"Clean"
    ==> "AssemblyInfo"
    ==> "Build"
    ==> "NuGet"
    ==> "Default"


RunTargetOrDefault "Default"