# MoonrakerRestApiSharp (former KlipperRestApiSharp)
A C# based library to communicate with the Moonraker WebApi, available with Fluidd and MainsailOS.

# Support me
If you want to support me, you can order over following affilate links (I'll get a small share from your purchase from the corresponding store).

- Prusa: http://www.prusa3d.com/#a_aid=AndreasReitberger *
- Jake3D: https://tidd.ly/3x9JOBp * 
- Amazon: https://amzn.to/2Z8PrDu *
- Coinbase: https://advanced.coinbase.com/join/KTKSEBP * (10€ in BTC for you if you open an account)
- TradeRepublic: https://refnocode.trade.re/wfnk80zm * (10€ in stocks for you open an account)

(*) Affiliate link
Thank you very much for supporting me!

# Important!
With the upcoming version, starting from `1.1.0`, `KlipperClient` become `MoonrakerClient`. also the namespaces will changed and generalized with our other print server api nugets.

| Old                             | New                              |
| ------------------------------- |:--------------------------------:|
| `AndreasReitberger`             | `AndreasReitberger.API.Moonraker`|
| `KlipperClient`                 | `MoonrakerClient`                |
| `Klipper...`                    | `Moonraker...`                   |


# Nuget MoonrakerRestApiSharp
[![NuGet](https://img.shields.io/nuget/v/MoonrakerSharpWebApi.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/MoonrakerSharpWebApi/)
[![NuGet](https://img.shields.io/nuget/dt/MoonrakerSharpWebApi.svg)](https://www.nuget.org/packages/MoonrakerSharpWebApi)

# Nuget KlipperRestApiSharp (deprecated)
[![NuGet](https://img.shields.io/nuget/v/KlipperSharpWebApi.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/KlipperSharpWebApi/)
[![NuGet](https://img.shields.io/nuget/dt/KlipperSharpWebApi.svg)](https://www.nuget.org/packages/KlipperSharpWebApi)

# Platform specific setup

## Android

On `Android` you need to allow local connections in the `AndroidManifest.xml`.
For this, create a new xml file and link to it in your manifest at `android:networkSecurityConfig`

Content of the `network_security_config.xml` file
```
<?xml version="1.0" encoding="utf-8" ?>
<network-security-config>
	<base-config cleartextTrafficPermitted="true" />
</network-security-config>

```

The manifest
```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest
	xmlns:android="http://schemas.android.com/apk/res/android"
	android:versionName="1.0.0"
	android:versionCode="1"
	package="com.company.app"
	>
	<application
		android:label="App Name"
		android:allowBackup="true"
		android:icon="@mipmap/appicon" 
		android:roundIcon="@mipmap/appicon_round"
		android:supportsRtl="true"
		android:networkSecurityConfig="@xml/network_security_config"
		>
	</application>
	<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
	<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
	<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
	<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
	<uses-permission android:name="android.permission.WAKE_LOCK" />
	<uses-permission android:name="android.permission.INTERNET" />
</manifest>
```

# Moonrakers Documentation
This C# library wraps the available Web API functions listed in the
Moonraker's documentation below. 
https://moonraker.readthedocs.io/en/latest/web_api/

You can see the current migration progress in the following table.

## Supported OS
Basically all OS using the Moonraker WebApi are theoretically supported. 

### Tested with our app
- [x] MainsailOS
- [x] FluiddPi

# App's using this library

![FeaturedImage](https://andreas-reitberger.de/wp-content/uploads/2022/01/feature-graphic_modified.png)

I also created an iOS app to interact with a moonraker based OS.
The app is available for testing here:<br>
TestFlight: https://testflight.apple.com/join/TyguKzt9

## Note for Android
There is an android version in development, however due to the poor 
performance using Xamarin.Android, I decided to wait till .NET MAUI 
is available.

# WebAPI migration status
Following you'll find the list of migrated functions from the WebAPI.

## Printer Administration

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Get Klippy host information         | ✅   | ✅      |
| Emergency Stop                      | ✅   | ✅      |
| Host Restart                        | ✅   | ✅      |
| Firmware Restart                    | ✅   | ✅      |

## Printer Status

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| List available printer objects      | ✅   | ✅      |
| Query printer object status         | ✅   | ✅      |
| Subscribe to printer object status  | ✅   | ✅      |
| Query Endstops                      | ✅   | ✅      |
| Query Server Info                   | ✅   | ✅      |
| Get Server Configuration            | ✅   | ✅      |
| Request Cached Temperature Data     | ✅   | ✅      |
| Request Cached GCode Responses      | ✅   | ✅      |
| Restart Server                      | ✅   | ✅      |
| Get Websocket ID                    | ✅   | ✅      |

## GCode APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Run a gcode                         | ✅   | ✅      |
| Get GCode Help                      | ✅   | ✅      |

## Print Management

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Print a file                        | ✅   | ✅      |
| Pause a print                       | ✅   | ✅      |
| Resume a print                      | ✅   | ✅      |
| Cancel a print                      | ✅   | ✅      |

## Machine Commands

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Get System Info                     | ✅   | ✅      |
| Shutdown the Operating System       | ✅   | ✅      |
| Reboot the Operating System         | ✅   | ✅      |
| Restart a system service            | ✅   | No      |
| Stop a system service               | ✅   | No      |
| Start a system service              | ✅   | No      |
| Get Moonraker Process Stats         | ✅   | ✅      |

## File Operations

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| List available files                | ✅   | ✅      |
| Get gcode metadata                  | ✅   | ✅      |
| Get directory information           | ✅   | ✅      |
| Create directory                    | ✅   | ✅      |
| Delete directory                    | ✅   | ✅      |
| Move a file or directory            | ✅   | ✅      |
| Copy a file or directory            | ✅   | ✅      |
| File download                       | ✅   | ✅      |
| File upload                         | ✅   | ✅      |
| File delete                         | ✅   | ✅      |
| Download klippy.log                 | ✅   | ✅      |
| Download moonraker.log              | ✅   | ✅      |


## Authorization

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Login User                          | ✅   | ✅      |
| Logout Current User                 | ✅   | ✅      |
| Get Current User                    | ✅   | ✅      |
| Create User                         | ✅   | ✅      |
| Delete User                         | ✅   | ✅      |
| List Available Users                | ✅   | ✅      |
| Reset User Password                 | ✅   | ✅      |
| Refresh JSON Web Token              | ✅   | ✅      |
| Generate a Oneshot Token            | ✅   | ✅      |
| Get the Current API Key             | ✅   | ✅      |
| Generate a New API Key              | ✅   | ✅      |

## Database APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| List namespaces                     | ✅   | ✅      |
| Get Database Item                   | ✅   | ✅      |
| Add Database Item                   | ✅   | ✅      |
| Delete Database Item                | ✅   | ✅      |

## Job Queue APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Retrieve the job queue status       | ✅   | ✅      |
| Enqueue a job                       | ✅   | ✅      |
| Remove a Job                        | ✅   | ✅      |
| Pause the job queue                 | ✅   | ✅      |
| Start the job queue                 | ✅   | ✅      |

## Update Manager APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Get update status                   | ✅    | ✅     |
| Perform a full update               | ✅    | ✅     |
| Update Moonraker                    | ✅    | No      |
| Update Klipper                      | ✅    | No      |
| Update Client                       | ✅    | No      |
| Update System Packages              | ✅    | No      |
| Recover a corrupt repo              | ✅    | No      |

## Power APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Get Device List                     | ✅    | ✅     |
| Get Device Status                   | ✅    | ✅     |
| Set Device State                    | ✅    | ✅     |
| Get Batch Device Status             | ✅    | ✅     |
| Batch Power On Devices              | ✅    | ✅     |
| Batch Power Off Devices             | ✅    | ✅     |

## Octoprint API emulation

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Version information                    | ✅    | ✅     |
| Server status                          | ✅    | ✅     |
| Login verification & User information  | ✅    | No     |
| Get settings                           | ✅    | ✅     |
| Octoprint File Upload                  | ✅    | ✅     |
| Get Job status                         | ✅    | ✅     |
| Get Printer status                     | ✅    | ✅     |
| Send GCode command                     | ✅    | ✅     |
| List Printer profiles                  | ✅    | ✅     |

## History APIs

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Get job list                           | ✅    | ✅     |
| Get job totals                         | ✅    | ✅     |
| Reset totals                           | ✅    | ✅     |
| Get a single job                       | ✅    | ✅     |
| Delete job                             | ✅    | ✅     |

## MQTT APIs

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Publish a topic                        | No    | No      |
| Subscribe to a topic                   | No    | No      |

## Websocket notifications
Not implemented yet.

# Usage
You can check the Test project for more code examples.

## Initialize the client
This initialize a new `KlipperClient` object. Always check if the
client is reachable before using it.

```csharp
private readonly string _host = "192.168.10.113";
private readonly int _port = 80;
private readonly string _api = "";
private readonly bool _ssl = false;

// Note, the api key is not mandatory
KlipperClient _server = new(_host, _port, _ssl);
await _server.CheckOnlineAsync();
if (_server.IsOnline)
{
    await _server.RefreshAllAsync();
    Assert.IsTrue(_server.InitialDataFetched);

    //var token = await _server.GetOneshotTokenAsync();
    KlipperAccessTokenResult token2 = await _server.GetApiKeyAsync();
    Assert.IsTrue(!string.IsNullOrEmpty(token2.Result));
}
```

## WebSocket
It's recommended to `StartListening()` to the WebSocket of your Klipper server. 
An example is shown below.

```csharp
Dictionary<DateTime, string> websocketMessages = new();
KlipperClient _server = new(_host, _api, _port, _ssl);

await _server.CheckOnlineAsync();
Assert.IsTrue(_server.IsOnline);

_server.StartListening();

// Once the Id has been received, subscribe to the printer status objects
_server.WebSocketConnectionIdChanged += (o, args) =>
{
    Assert.IsNotNull(args.ConnectionId);
    Assert.IsTrue(args.ConnectionId > 0);
    Task.Run(async () =>
    {
        string subResult = await _server.SubscribeAllPrinterObjectStatusAsync(args.ConnectionId);
    });
};

_server.Error += (o, args) =>
{
    Assert.Fail(args.ToString());
};
_server.ServerWentOffline += (o, args) =>
{
    Assert.Fail(args.ToString());
};

_server.WebSocketDataReceived += (o, args) =>
{
    if (!string.IsNullOrEmpty(args.Message))
    {
        websocketMessages.Add(DateTime.Now, args.Message);
        Debug.WriteLine($"WebSocket Data: {args.Message} (Total: {websocketMessages.Count})");
    }
};

_server.WebSocketError += (o, args) =>
{
    Assert.Fail($"Websocket closed due to an error: {args}");
};

// Wait 10 minutes
CancellationTokenSource cts = new(new TimeSpan(0, 10, 0));
_server.WebSocketDisconnected += (o, args) =>
{
    if (!cts.IsCancellationRequested)
        Assert.Fail($"Websocket unexpectly closed: {args}");
};

do
{
    await Task.Delay(10000);
    await _server.CheckOnlineAsync();
} while (_server.IsOnline && !cts.IsCancellationRequested);
_server.StopListening();


Assert.IsTrue(cts.IsCancellationRequested);
```

## User auth
If your Klipper server is protected with a login, please call the `LoginUserAsync` method first.
Alternatively you can provide the API key instead.

```csharp
string username = "TestUser";
string password = "TestPassword";

KlipperUserActionResult userCreated = await _server.CreateUserAsync(username, password);
Assert.IsNotNull(userCreated);

List<KlipperUser> users = await _server.ListAvailableUsersAsync();
Assert.IsTrue(users?.Count > 0);

KlipperUserActionResult login = await _server.LoginUserAsync(username, password);
Assert.IsNotNull(login);
Assert.IsTrue(login.Username == username);

KlipperUser currentUser = await _server.GetCurrentUserAsync();
Assert.IsNotNull(currentUser);

KlipperUserActionResult newTokenResult = await _server.RefreshJSONWebTokenAsync();
Assert.IsNotNull(newTokenResult);
Assert.IsTrue(_server.UserToken == newTokenResult.Token);

string newPassword = "TestPasswordChanged";
KlipperUserActionResult refreshPassword = await _server.ResetUserPasswordAsync(password, newPassword);
Assert.IsNotNull(refreshPassword);

KlipperUserActionResult logout = await _server.LogoutCurrentUserAsync();
Assert.IsNotNull(logout);

login = await _server.LoginUserAsync(username, newPassword);
Assert.IsNotNull(login);
Assert.IsTrue(login.Username == username);

logout = await _server.LogoutCurrentUserAsync();
Assert.IsNotNull(logout);

KlipperUserActionResult userDeleted = await _server.DeleteUserAsync(username);
Assert.IsNotNull(userDeleted);
```
