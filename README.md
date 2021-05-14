# Pizza Microservices Sandbox

## Option #1 : .NET Services and Client - No Dockerfiles

* Start MongoDb container
  * `docker run -d --name mongo -p 27017:27017 mongo`
* Start Redis container
  * `docker run -d --name myRedis -p 6379:6379 redis`
  * Start each project with/without debugger connected

AppSettings are set to use appsettings.Development.json, user secrets, and Azure App Configuration Service is added only if the Hosting Environment is not "Development".

```csharp
 config.AddAzureAppConfiguration(connection);
 ```

 ## Option #2: .NET Services and Client w/ Docker & Compose

 ## Option #3: Tye (preferred)

 ```
 tye run
 ```

 To debug a specific service, add `--debug <service_name>` and attach to the process `name.exe` from Visual Studio or VS Code.
