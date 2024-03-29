#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"

#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

[<AutoOpen>]
module Utils =
  let dotnet cmd arg =
    let res = DotNet.exec id cmd arg

    let msg = $"dotnet %s{cmd} %s{arg}"

    if res.OK then
      Trace.tracefn "Success '%s'" msg
    else
      failwithf "Failed '%s'" msg

  let (|LowerCase|_|) (x: string) (s: string) =
    if x.ToLower() = s.ToLower() then Some LowerCase else None

  let getConfiguration = function
    | Some (LowerCase("debug")) -> DotNet.BuildConfiguration.Debug
    | Some (LowerCase("release")) -> DotNet.BuildConfiguration.Release
    | Some (c) -> failwithf "Invalid configuration '%s'" c
    | _ -> DotNet.BuildConfiguration.Debug

Target.initEnvironment ()

let args = Target.getArguments()

Target.create "Format" (fun _ ->
  dotnet "format" "-v diag"
)

Target.create "Format.Check" (fun _ ->
  dotnet "format" "--verify-no-changes -v diag"
)

Target.create "Clean" (fun _ ->
  !! "src/**/bin"
  ++ "src/**/obj"
  ++ "tests/**/bin"
  ++ "tests/**/obj"
  ++ "example/**/bin"
  ++ "example/**/obj"
  |> Shell.cleanDirs
)

Target.create "Build" (fun _ ->
  let configuration =
    args
    |> Option.bind Array.tryHead
    |> getConfiguration

  !! "src/**/*.*proj"
  ++ "example/**/*.*proj"
  ++ "tests/**/*.*proj"
  |> Seq.iter (DotNet.build (fun p ->
    { p with
        Configuration = configuration
    }))
)

Target.create "Test" (fun _ ->
  !! "tests/**/*.*proj"
  |> Seq.iter(fun proj ->
    DotNet.test (fun p ->
      { p with
          Logger = Some "console;verbosity=detailed"
      }
    ) proj
  )
)

Target.create "None" ignore
Target.create "Default" ignore

"Build" ==> "Default"

Target.runOrDefaultWithArguments "Default"
