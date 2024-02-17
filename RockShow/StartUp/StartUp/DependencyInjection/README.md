##### Create another File for your DI code based on the name of your Interface

For example:
- IAppleService > AConfigureServices
- IUserSerivce > BConfigureServices
- IVegtableService > CConfigureServices
> The naming convention of the files here is not appropriate but we will make an exception

#### Implement Interface

The `public` class should implement this Interface: 'IConfigureDependencyInjection'

#### Provide Code 
Add code to this method that you have to add to this class
```csharp
         public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
             //still order these alphabetically by Interface name 
             services.AddSingleton<IMyService, MyService>();
        }
```
