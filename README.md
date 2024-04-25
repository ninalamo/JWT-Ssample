# auth-server
### Project Dependency and Workflow
#### Website (MVC Client)
Note that we need the following setup in the MVC Client
```C#
//Program.cs
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

app.UseSession();
```
#### Authentication (Identity) Server
```json
//appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=auth;User Id=sa;Password=someThingComplicated1234;TrustServerCertificate=True;MultiSubnetFailover=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft": "Warning"
    },
    "Debug": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.Hosting": "Trace"
      }
    },
    "EventSource": {
      "LogLevel": {
        "Default": "Warning"
      }
    }
  },
  "AllowedHosts": "*",
  "JWT": {
    "ValidAudience": "authserver",
    "ValidIssuer": "authserver",
    "Secret": "17CF4AC5-4DC6-4567-BCCE-BB6B668873B3"
  }
}



```
```mermaid
flowchart LR
    Website -.-> |1. Login - Fetch Token| IdentityServer
    IdentityServer -.-> |2. Store Token to Session / LocalStorage | Website
    Website --> |3. Send HTTP Request with JWT| Protected
    
    subgraph Protected
    WeatherForeCastApi
    end
    subgraph IdentityServer

    end

```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=authdb;User Id=sa;Password=someThingComplicated1234;TrustServerCertificate=True;MultiSubnetFailover=True;",
    "DevConnection": "DataSource=app.db;Cache=Shared"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft": "Warning"
    },
    "Debug": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.Hosting": "Trace"
      }
    },
    "EventSource": {
      "LogLevel": {
        "Default": "Warning"
      }
    }
  },
  "AllowedHosts": "*",
  "JWT":{
    "ValidAudience":"*",
    "ValidIssuer":"*",
    "Secret":"17CF4AC5-4DC6-4567-BCCE-BB6B668873B3"
  },
  "DefaultPassword": ""
}

```
