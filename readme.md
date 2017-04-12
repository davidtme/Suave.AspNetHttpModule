Experimental ASP.net HttpModule wrapper for Suave.IO 
====================================================

This allows you to pass a request to Suave.IO and if suave doesn't handle it fall back to asp.net.

A lot of inspiration has come from https://github.com/tamizhvendan/Suave.Azure.Functions


Setup
-----

To install Suave.AspNetHttpModule, add the following to your paket.dependencies:

```
source https://nuget.org/api/v2
nuget Suave.AspNetHttpModule
```

Or you can use the legacy NuGet command line Package Manager Console:
```
PM> Install-Package Suave.AspNetHttpModule
```



Create a new type that will be visible to your asp.net application either in your Suave F# App

```F#
namespace SampleSuaveLib

open System.Web
open Suave.AspNetHttpModule

type Handler() =
    interface IHttpModule with
        member __.Init application = handleApplication App.app (* <-- your root web part *) application
        member __.Dispose() = ignore()
```

or in your asp.net app (Such as the App_Start folder):

```C#
using System.Web;

namespace SampleSuaveWeb.App_Start
{
    public class AppHandler : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            Suave.AspNetHttpModule.handleApplication(SampleSuaveLib.App.app, context);
        }

        public void Dispose() { }
    }
}

```

Then add the handler to the web config:

``` XML
<modules>
    ...
    <add name="SampleSuaveLib" type="SampleSuaveLib.Handler, SampleSuaveLib" />
    ...
</modules>
```