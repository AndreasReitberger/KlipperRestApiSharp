# KlipperRestApiSharp
A C# based library to communicate with a klipper using MainsailOS.

# Moonrakers Documentation
This C# library wraps the available Web API functions listed in the
Moonraker's documentation below. 
https://moonraker.readthedocs.io/en/latest/web_api/

You can see the current migration progress in the following table.

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
| Resume a print                      | ✅   | No      |
| Cancel a print                      | ✅   | No      |

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
| File download                       | ✅   | No      |
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
Implementation not planned at the moment.

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
| Get update status                   | ✅    | No      |
| Perform a full update               | ✅    | No      |
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
