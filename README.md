
# Platzi-BlobConsole

applicacion de consola hecha en VS code para probar y hacer ejecicios de curso [Almacenamiento en Azure](https://platzi.com/cursos/almacenamiento-azure/)



----
## Nuget Packages

* dotnet add package Microsoft.Extensions.Configuration --version 7.0.0
* dotnet add package Microsoft.Extensions.Configuration.FileExtensions --version 7.0.0
* dotnet add package Microsoft.Extensions.Configuration.Json --version 7.0.0
* dotnet add package Microsoft.Azure.Storage.Blob --version 11.2.3
* dotnet add package Microsoft.Azure.Cosmos --version 3.32.2
* dotnet add package Azure.Data.Tables --version 12.8.0
* dotnet add package Microsoft.Extensions.Configuration.Binder --version 8.0.0
* dotnet add package Azure.Messaging.ServiceBus --version 7.17.1
* 

Microsoft.Extensions.Configuration.Binder is for use Get Extentsion method after IcConfiguration.GetSection()

```
var test = config.GetSection("CommonMIMETypes").Get<Dictionary<string, string>>();
```



----



**Comandos consola dotnet**
|Comando |Descripción |
|:---- |:---- |
|`dotnet new console -n NombrePryecto` |Crea el proyecto de tipo consola con el nombre. |

**Common MIME types**

MIME types list got from [here](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types)



## Git Ignore

|line |description |
|---:|:---|
|*.jpg|image ext.|
|*.gif|image ext.|
|appsettings.Local.json|local configuration|
|platzi-blobConsole.csproj.nuget.dgspec.json|config file for nuget|
|platzi-blobConsole.csproj.nuget.g.props|config file for nuget|
|platzi-blobConsole.csproj.nuget.g.targets|config file for nuget|
|project.nuget.cache|config file for nuget|
