using AndreasReitberger.API.Moonraker;
using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Moonraker.Models.WebSocket;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace MoonrakerSharpWebApi.Test
{
    public partial class Tests
    {
        private readonly string _host = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").Ip ?? "";
        private readonly int _port = 80;
        private readonly string _api = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").ApiKey ?? "";
        private readonly string _user = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").Username ?? "";
        private readonly string _pwd = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").Password ?? "";
        private readonly bool _ssl = false;

        private readonly bool _skipOnlineTests = true;

        private string[] wsStartCommands =
        [ 
            // Needed to get the ConnectionId
            $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.info\",\"params\":{{}},\"id\":1}}",
            $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.websocket.id\",\"params\":{{}},\"id\":2}}",
        ];

        MoonrakerClient? client = null;

        [GeneratedRegex(@"^[A-Z][A-Za-z0-9]*$")]
        private static partial Regex MyRegex();

        [GeneratedRegex(@"(?<=\"").+?(?=\"")")]
        private static partial Regex MyRegex_Extract();

        [SetUp]
        public void Setup()
        {
            string host = $"{(_ssl ? "https://" : "http://")}{_host}:{_port}";
            string ws = $"{(_ssl ? "wss://" : "ws://")}{_host}:{_port}/websocket";
            client = new MoonrakerClient.MoonrakerConnectionBuilder()
                .WithServerAddress(host)
                .WithApiKey(_api)
                .WithWebSocket(ws)
                .WithTimeout(100)
                .Build(); 
            client.Error += (sender, args) =>
                {
                    if (!client.ReThrowOnError)
                    {
                        Assert.Fail($"Error: {args?.ToString()}");
                    }
                };
            client.RestApiError += (sender, args) =>
            {
                if (!client.ReThrowOnError)
                {
                    //Assert.Fail($"REST-Error: {args?.ToString()}");
                    Debug.WriteLine($"REST-Error: {args?.ToString()}");
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
        }

        [Test]
        public async Task BuildWitUserTokenAsync()
        {
            try
            {
                Assert.That(!string.IsNullOrEmpty(_user) && !string.IsNullOrEmpty(_pwd), "Provide a user and password in your secrets.json file first!");
                string host = $"{(_ssl ? "https://" : "http://")}{_host}:{_port}";

                var client = new MoonrakerClient.MoonrakerConnectionBuilder()
                    .WithName("Test")
                    .WithServerAddress(host)
                    .Build();
                string? apiKey = await client.LoginUserForApiKeyAsync(_user, _pwd);

                /*
                 * Set by the LoginUserAsync() method
                 * UserToken = queryResult?.Result?.Token ?? string.Empty;
                 * RefreshToken = queryResult?.Result?.RefreshToken ?? string.Empty;
                 */
                Assert.That(!string.IsNullOrEmpty(client.UserToken));
                Assert.That(!string.IsNullOrEmpty(client.RefreshToken));

                // This should be enough to authenticate.
                KlipperMachineInfo? info = await client.GetMachineSystemInfoAsync();
                Assert.That(info is not null);
                info = null;

                // Remove the api key to test if the UserToken works as well
                var lastHeader = client.AuthHeaders.Last();
                client.AuthHeaders.Remove(lastHeader.Key);

                info = await client.GetMachineSystemInfoAsync();
                Assert.That(info is not null);

                client.AuthHeaders.Clear();
                client.ApiKey = "";

                // Also try with the api key to verify
                client.ApiKey = apiKey ?? string.Empty;
                Assert.That(!string.IsNullOrEmpty(client.ApiKey));

                info = await client.GetMachineSystemInfoAsync();
                Assert.That(info is not null);

                await client.LogoutCurrentUserAsync();

                info = await client.GetMachineSystemInfoAsync();
                Assert.That(info is not null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void SerializeJsonTest()
        {
            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {
                string host = $"{(_ssl ? "https://" : "http://")}{_host}:{_port}";
                MoonrakerClient.Instance = new MoonrakerClient(host)
                {
                    FreeDiskSpace = 1523165212,
                    TotalDiskSpace = 65621361616161,
                    ServerName = "My moonraker server",
                };
                MoonrakerClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", "my_awesome_pwd", true);

                string serializedString = System.Text.Json.JsonSerializer.Serialize(MoonrakerClient.Instance, options: MoonrakerClient.DefaultJsonSerializerSettings);
                MoonrakerClient? serializedObject = System.Text.Json.JsonSerializer.Deserialize<MoonrakerClient>(serializedString, options: MoonrakerClient.DefaultJsonSerializerSettings);
                Assert.That(serializedObject is MoonrakerClient server && server != null);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public void SerializeJsonNewtonsoftTest()
        {

            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {
                string host = $"{(_ssl ? "https://" : "http://")}{_host}:{_port}";
                MoonrakerClient.Instance = new MoonrakerClient(host)
                {
                    FreeDiskSpace = 1523165212,
                    TotalDiskSpace = 65621361616161,
                    ServerName = "My moonraker server",
                };
                MoonrakerClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", "my_awesome_pwd", true);
                string serializedString = JsonConvert.SerializeObject(MoonrakerClient.Instance, Formatting.Indented, settings: MoonrakerClient.DefaultNewtonsoftJsonSerializerSettings);
                MoonrakerClient? serializedObject = JsonConvert.DeserializeObject<MoonrakerClient>(serializedString, settings: MoonrakerClient.DefaultNewtonsoftJsonSerializerSettings);
                Assert.That(serializedObject is MoonrakerClient server && server != null);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public void SerializeAllTypesWithJsonNewtonsoftTest()
        {
            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {
                List<Type> types = [.. AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && !t.Name.StartsWith("<") && t.Namespace?.StartsWith("AndreasReitberger.API.Moonraker") is true)]
                       ;
                Regex r = MyRegex();
                Regex extract = MyRegex_Extract();
                foreach (Type t in types)
                {
                    if (t == typeof(KlipperWebSocketStateRespone))
                    {

                    }
                    object? obj = null;
                    try
                    {
                        // Not possible for extensions classes, so catch this
                        obj = Activator.CreateInstance(t);
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine($"Exception while creating object from type `{t}`: {exc.Message}");
                    }
                    if (obj is null) continue;
                    string serializedString =
                        JsonConvert.SerializeObject(obj, Formatting.Indented, settings: MoonrakerClient.DefaultNewtonsoftJsonSerializerSettings);
                    if (serializedString == "{}") continue;

                    // Get all property infos
                    List<PropertyInfo> p = [.. t
                        .GetProperties()
                        .Where(prop => prop.GetCustomAttribute<JsonPropertyAttribute>(true) is not null)]
                        ;

                    // Get the property names from the json text
                    var splitString = serializedString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    bool skip = false;
                    StringBuilder sb = new();
                    // Cleanup from child nodes, those will be checked individually
                    foreach (var line in splitString)
                    {
                        if (line.Contains(": {") && !line.Contains("{}"))
                        {
                            skip = true;
                            sb.AppendLine(line.Replace(": {", ": null,"));
                        }
                        else if (line.StartsWith("},"))
                        {
                            skip = false;
                        }
                        else if (!skip)
                            sb.AppendLine(line.Trim());
                    }
                    // set to cleanuped string
                    serializedString = sb.ToString();
                    var splitted = serializedString.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    List<string> properties = [.. splitted.Select(row => extract.Match(row ?? "")?.Value ?? string.Empty)]
                        ;
                    /*
                    serializedString = string.Join(Environment.NewLine, splitString);
                    List<string> properties = serializedString.Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .ToList()
                        ;
                    */
                    foreach (string property in properties)
                    {
                        bool valid = r.IsMatch(property);
                        //string trimmed = extract.Match(property).Value;
                        if (!valid)
                        {
                            PropertyInfo? jsonAttribute = p.Where(prop =>
                                prop.CustomAttributes.Any(attr => attr.ConstructorArguments.Where(arg => arg.Value is string str && str == property).Count() == 1))
                                .ToList()
                                .FirstOrDefault()
                                ;

                            if (jsonAttribute is not null)
                            {
                                CustomAttributeData? ca = jsonAttribute.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(JsonPropertyAttribute));
                                if (ca is not null)
                                {
                                    CustomAttributeTypedArgument cap = ca.ConstructorArguments.FirstOrDefault();
                                    string propertyName = cap.Value?.ToString() ?? string.Empty;
                                    // If the property name is adjusted with the json attribute, it is ok to start with a lower case.
                                    valid = property == propertyName;
                                }
                            }
                        }
                        if (!valid)
                        {

                        }
                        string msg = $"Type: {t} => {property} is {(valid ? "valid" : "invalid")}";
                        Debug.WriteLine(msg);
                        Assert.That(valid, message: msg);
                    }
                }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public void SerializeTest()
        {

            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {
                XmlSerializer xmlSerializer = new(typeof(MoonrakerClient));
                using (var fileStream = new FileStream(serverConfig, FileMode.Create))
                {
                    string host = $"{(_ssl ? "https://" : "http://")}{_host}:{_port}";
                    MoonrakerClient.Instance = new MoonrakerClient(host)
                    {
                        UsedDiskSpace = 1523152132,
                        FreeDiskSpace = 1523165212,
                        TotalDiskSpace = 65621361616161,
                        ServerName = "My moonraker server",
                    };
                    MoonrakerClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", "my_awesome_pwd", true);

                    xmlSerializer.Serialize(fileStream, MoonrakerClient.Instance);
                    Assert.That(File.Exists(Path.Combine(dir, "server.xml")));
                }

                xmlSerializer = new XmlSerializer(typeof(MoonrakerClient));
                using (FileStream fileStream = new(serverConfig, FileMode.Open))
                {
                    MoonrakerClient instance = (MoonrakerClient)xmlSerializer.Deserialize(fileStream);
                    Assert.That(instance is MoonrakerClient server && server != null);
                }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task ServerInitTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperAccessTokenResult? token2 = await client.GetApiKeyAsync();
                    Assert.That(!string.IsNullOrEmpty(token2?.Result));

                    Dictionary<string, object> objectList = await client.QueryPrinterObjectStatusAsync(new() { { "gcode_move", "" } });
                    Assert.That(objectList?.Count > 0);

                    objectList = await client.QueryPrinterObjectStatusAsync(new() { { "toolhead", "position,status" } });
                    Assert.That(objectList?.Count > 0);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task RefreshTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    client.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args?.Message);
                    };

                    await client.RefreshAllAsync();
                    await Task.Delay(250);

                    Assert.That(client.InitialDataFetched);

                    Assert.That(client.IdleState is not null);
                    Assert.That(client.DisplayStatus is not null);
                    Assert.That(client.GcodeMove is not null);
                    Assert.That(client.GcodeMeta is not null);
                    Assert.That(client.VirtualSdCard is not null);

                    Assert.That(client.HeatedBeds?.Count > 0);
                    Assert.That(client.HeatedBeds?.FirstOrDefault().Value?.TempRead > 0);
                    Assert.That(client.ActiveHeatedBed?.TempRead > 0);

                    Assert.That(client.Toolheads?.Count > 0);
                    Assert.That(client.Toolheads?.FirstOrDefault().Value?.TempRead > 0);
                    Assert.That(client.ActiveToolhead?.TempRead > 0);

                    Assert.That(client.PrintStats is not null);

                    Assert.That(client.Files is not null);
                    Assert.That(client.Files?.Count > 0);

                    Assert.That(client.Fans is not null);
                    Assert.That(client.Fans?.Count > 0);
                    Assert.That(client.ActiveFan?.Speed > 0);
                    if (client.IsPrinting)
                    {
                        //Assert.That(client.ActiveJob is not null);
                    }
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        #region WebCam
        [Test]
        public async Task WebCamTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    client.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };

                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    List<IWebCamConfig>? webcamConfigs = await client.GetWebCamSettingsFromDatabaseAsync();
                    Assert.That(webcamConfigs?.Count > 0);

                    webcamConfigs = await client.GetWebCamSettingsAsync();
                    Assert.That(webcamConfigs?.Count > 0);

                    string webcamUri = await client.GetWebCamUriAsync(0, false);
                    Assert.That(client.WebCams?.Count > 0);
                    Assert.That(client.WebCams?.Count > 0);
                    Assert.That(!string.IsNullOrEmpty(webcamUri));
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        #endregion

        #region Printer Tests
        [Test]
        public async Task PrinterStatusTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    client.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };
                    await client.StartListeningAsync(commandsOnConnect: wsStartCommands);

                    Dictionary<string, KlipperStatusFilamentSensor> fSensors = await client.GetFilamentSensorsAsync();
                    Assert.That(fSensors?.Count > 0);

                    Dictionary<string, string> macros = [];
                    List<string> availableMacros = await client.GetPrinterObjectListAsync("gcode_macro");
                    for (int i = 0; i < availableMacros.Count; i++)
                    {
                        macros.Add(availableMacros[i], string.Empty);
                    }

                    Dictionary<string, KlipperGcodeMacro> gcMacros = await client.GetGcodeMacrosAsync();
                    Assert.That(gcMacros?.Count > 0);

                    List<string> objects = await client.GetPrinterObjectListAsync();
                    Assert.That(objects?.Count > 0);

                    KlipperEndstopQueryResult endstops = await client.QueryEndstopsAsync();
                    Assert.That(endstops is not null);

                    macros.Clear();
                    Dictionary<string, string> targets = [];
                    for (int i = 0; i < objects.Count; i++)
                    {
                        targets.Add(objects[i], string.Empty);
                        if (objects[i].StartsWith("gcode_macro"))
                            macros.Add(objects[i], string.Empty);
                    }
                    //targets.Add("gcode_move", "");
                    //targets.Add("toolhead", "");
                    //targets.Add("extruder", "target,temperature");

                    Dictionary<string, KlipperGcodeMacro> availableMarcros = await client.GetGcodeMacrosAsync();
                    Assert.That(availableMarcros?.Count > 0);

                    Dictionary<string, object> objectStates = await client.QueryPrinterObjectStatusAsync(targets);
                    Assert.That(objectStates?.Count > 0);

                    long? connectionId = client.WebSocketConnectionId; // ""; // Get from WebSocket?
                    string result = await client.SubscribePrinterObjectStatusAsync(connectionId, objects);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task GcodeMacroTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    client.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };

                    Dictionary<string, string> macros = [];
                    List<string> availableMacros = await client.GetPrinterObjectListAsync("gcode_macro", true);
                    for (int i = 0; i < availableMacros.Count; i++)
                    {
                        macros.Add(availableMacros[i], string.Empty);
                    }

                    macros.Clear();
                    List<string> objects = await client.GetPrinterObjectListAsync("gcode_macro");
                    for (int i = 0; i < objects.Count; i++)
                    {
                        macros.Add(objects[i], string.Empty);
                    }

                    Dictionary<string, KlipperGcodeMacro> gcMacros = await client.GetGcodeMacrosAsync();
                    Assert.That(gcMacros?.Count > 0);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task GcodeMetaTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                client.Error += (sender, e) =>
                {
                    Assert.Fail(e.ToString());
                };
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    client.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };

                    List<IGcode> models = await client.GetAvailableFilesAsync("gcodes", true);
                    //var childItems = models?.Where(model => model.Path.Contains("/")).ToList();
                    Assert.That(models?.Count > 0);
                    foreach (KlipperFile? gcodeFile in models?.Cast<KlipperFile>()?.Where(g => g?.Meta?.GcodeImages?.Count > 0))
                    {
                        byte[] thumbnail = await client.GetGcodeThumbnailImageAsync(gcodeFile?.Meta);
                        Assert.That(thumbnail is not null);
                    }
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task PrinterInfoTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperPrinterStateMessageResult info = await client.GetPrinterInfoAsync();
                    Assert.That(info is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task SendGcodeCommandTest()
        {
            try
            {
                if (_skipOnlineTests) return;

                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    int fanSpeed = 128;
                    string gcode = $"M106 S{fanSpeed}";
                    bool result = await client.RunGcodeScriptAsync(gcode);
                    Assert.That(result);

                    fanSpeed = 0;
                    gcode = $"M106 S{fanSpeed}";
                    result = await client.RunGcodeScriptAsync(gcode);
                    Assert.That(result);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task MovePrinterTest()
        {
            try
            {
                if (_skipOnlineTests) return;

                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    var homed = await client.HomeAxesAsync(true, true, true);
                    Assert.That(homed, "Not homed!");

                    // Move all 3 axes to ensure they all move correctly
                    var newX = 100;
                    var newY = 100;
                    var newZ = 100;
                    var moved = await client.MoveAxesAsync(
                        speed: 6000,
                        x: newX,
                        y: newY,
                        z: newZ,
                        relative: false);
                    var end = await client.GetToolHeadStatusAsync();
                    Assert.That(moved, "Did not move");
                    Assert.That(newX == end.Position[0], message: "X didn't move as expected");
                    Assert.That(newY == end.Position[1], message: "Y didn't move as expected");
                    Assert.That(newZ == end.Position[2], message: "Z didn't move as expected");

                    // Move all 3 axes independently to make sure they move as expected
                    newX = 50;
                    newY = 50;
                    newZ = 50;
                    await client.MoveAxesAsync(
                        speed: 6000,
                        x: newX);
                    await client.MoveAxesAsync(
                        speed: 6000,
                        y: newY);
                    await client.MoveAxesAsync(
                        speed: 6000,
                        z: newZ);
                    end = await client.GetToolHeadStatusAsync();
                    Assert.That(newX == end.Position[0], message: "X didn't move as expected");
                    Assert.That(newY == end.Position[1], message: "Y didn't move as expected");
                    Assert.That(newZ == end.Position[2], message: "Z didn't move as expected");
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        #endregion

        #region Print Management Tests
        [Test]
        public async Task PrinterManagementTest()
        {
            try
            {
                if (_skipOnlineTests) return;
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    bool fan = await client.SetFanSpeedTargetAsync(128, false);
                    fan = await client.SetFanSpeedTargetAsync(0, false);
                    fan = await client.SetFanSpeedTargetAsync(100, true);
                    fan = await client.SetFanSpeedTargetAsync(0, false);

                    bool extruder = await client.SetExtruderTargetAsync(50);
                    extruder = await client.SetExtruderTargetAsync(50, 1);
                    await Task.Delay(5000);
                    extruder = await client.SetExtruderTargetAsync(0);
                    extruder = await client.SetExtruderTargetAsync(0, 1);

                    bool heaterBed = await client.SetHeaterBedTargetAsync(50);
                    await Task.Delay(5000);
                    heaterBed = await client.SetHeaterBedTargetAsync(0);

                    string fileName = "test.gcode";
                    bool printStarted = await client.PrintFileAsync(fileName);
                    Assert.That(printStarted);

                    if (printStarted)
                    {
                        // Wait a minute
                        await Task.Delay(60 * 1000);
                        bool paused = await client.PausePrintAsync();
                        await Task.Delay(5000);
                        Assert.That(paused);

                        if (paused)
                        {
                            bool resumed = await client.ResumePrintAsync();
                            await Task.Delay(5000);
                            Assert.That(resumed);

                            if (resumed)
                            {
                                bool cancelled = await client.CancelPrintAsync();
                                await Task.Delay(5000);
                                Assert.That(cancelled);
                            }
                        }
                    }
                }
                else
                {
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
                }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        #endregion

        #region Server Tests
        [Test]
        public async Task ServerApiTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    //var token = await client.GetOneshotTokenAsync();
                    KlipperAccessTokenResult token2 = await client.GetApiKeyAsync();
                    Assert.That(!string.IsNullOrEmpty(token2.Result));
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task ServerConfigTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperServerConfig config = await client.GetServerConfigAsync();
                    Assert.That(config is not null);

                    Dictionary<string, KlipperTemperatureSensorHistory> tempData = await client.GetServerCachedTemperatureDataAsync();
                    Assert.That(tempData?.Count > 0);

                    List<KlipperGcode> cachedGcodes = await client.GetServerCachedGcodesAsync();
                    Assert.That(cachedGcodes?.Count > 0);

                    if (!_skipOnlineTests)
                    {
                        bool restart = await client.RestartServerAsync();
                        Assert.That(restart);
                    }
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task ServerGcodeApiTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    Dictionary<string, string> help = await client.GetGcodeHelpAsync();
                    Assert.That(help?.Count > 0);
                    if (!_skipOnlineTests)
                    {
                        bool succeed = await client.RunGcodeScriptAsync("G28");
                        Assert.That(succeed);
                    }
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task ServerMachineCommandTest()
        {
            try
            {
                if (_skipOnlineTests) return;
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperMachineInfo info = await client.GetMachineSystemInfoAsync();
                    Assert.That(info is not null);

                    bool restarted = await client.RestartSystemServiceAsync(KlipperServices.webcamd);
                    Assert.That(restarted);
                    await Task.Delay(10 * 1000);

                    bool stopped = await client.StopSystemServiceAsync(KlipperServices.webcamd);
                    Assert.That(stopped);
                    await Task.Delay(10 * 1000);

                    bool started = await client.StartSystemServiceAsync(KlipperServices.webcamd);
                    Assert.That(started);
                    await Task.Delay(10 * 1000);

                    bool rebooted = await client.MachineRebootAsync();
                    Assert.That(rebooted);
                    await Task.Delay(10 * 1000);

                    bool shutdown = await client.MachineShutdownAsync();
                    Assert.That(shutdown);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task ServerMachineMoonrakerTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperMoonrakerProcessStatsResult info = await client.GetMoonrakerProcessStatsAsync();
                    Assert.That(info is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task ServerFilesTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    List<IGcode> files = await client.GetAvailableFilesAsync();
                    Assert.That(files?.Count > 0);

                    string? fileName = files[0]?.FilePath;
                    KlipperGcodeMetaResult? meta = await client.GetGcodeMetadataAsync(fileName);
                    Assert.That(meta is not null);


                    files = await client.GetAvailableFilesAsync("config");
                    Assert.That(files?.Count > 0);

                    files = await client.GetAvailableFilesAsync("config_examples");
                    Assert.That(files?.Count > 0);

                    files = await client.GetAvailableFilesAsync("docs ");
                    Assert.That(files?.Count > 0);

                    string dirName = "gcodes/Kundenaufträge";
                    KlipperDirectoryActionResult? created = await client.CreateDirectoryAsync(dirName);
                    Assert.That(created is not null);

                    KlipperDirectoryActionResult? copyFile = await client.CopyDirectoryOrFileAsync($"gcodes/{fileName}", $"{dirName}/{fileName}");
                    Assert.That(copyFile is not null);

                    KlipperDirectoryInfoResult? dirs = await client.GetDirectoryInformationAsync(dirName);
                    Assert.That(dirs is not null);

                    KlipperDirectoryActionResult? deleteFile = await client.DeleteFileAsync($"{dirName}/{fileName}");
                    Assert.That(deleteFile is not null);

                    KlipperDirectoryActionResult? deleted = await client.DeleteDirectoryAsync(dirName);
                    Assert.That(deleted is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        #endregion

        #region Files

        [Test]
        public async Task UploadFileTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    string testFile = @"Files/Groot_0.2mm_PETG_MK3S_11h28m.gcode";

                    Assert.That(File.Exists(testFile));
                    FileInfo info = new(testFile);
                    string fullPath = info.FullName;

                    byte[]? file = await File.ReadAllBytesAsync(testFile);

                    KlipperFileActionResult? msg = await client.UploadFileAsync(localFilePath: fullPath, timeout: 100000);
                    KlipperDirectoryActionResult? deleted = await client.DeleteFileAsync("gcodes", info.Name);

                    msg = await client.UploadFileAsync(fileName: info.Name, file: file, timeout: 100000);
                    Assert.That(msg is not null);

                    KlipperGcodeMetaResult? meta = await client.GetGcodeMetadataAsync(msg?.Item?.Path);
                    Assert.That(meta is not null);

                    string? thumbnail = meta?.GcodeImages.FirstOrDefault()?.Path;
                    // Get small image (30x30)
                    byte[]? image = await client.GetGcodeThumbnailImageAsync(thumbnail);
                    // Get big image (400x300)
                    byte[]? image2 = await client.GetGcodeThumbnailImageAsync(meta, 1);

                    deleted = await client.DeleteFileAsync("gcodes", info.Name);
                    Assert.That(deleted is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task DownloadLogFileTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    byte[]? logFile = await client.DownloadLogFileAsync(KlipperLogFileTypes.Klippy);
                    await File.WriteAllBytesAsync("klippy.log", logFile);
                    Assert.That(logFile?.Length > 0);

                    logFile = await client.DownloadLogFileAsync(KlipperLogFileTypes.Moonraker);
                    await File.WriteAllBytesAsync("moonraker.log", logFile);
                    Assert.That(logFile?.Length > 0);

                    File.Delete("klippy.log");
                    File.Delete("moonraker.log");
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        #endregion

        #region Authorization
        [Test]
        public async Task AuthTest()
        {
            try
            {
                if (_skipOnlineTests) return;
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    string username = "TestUser";
                    string password = "TestPassword";

                    KlipperUserActionResult login2 = await client.LoginUserAsync(username, password);

                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperUserActionResult userCreated = await client.CreateUserAsync(username, password);
                    Assert.That(userCreated is not null);

                    List<KlipperUser> users = await client.ListAvailableUsersAsync();
                    Assert.That(users?.Count > 0);

                    KlipperUser currentUser = await client.GetCurrentUserAsync();
                    if (currentUser != null)
                    {
                        //Assert.IsNotNull(await client.LogoutCurrentUserAsync());
                    }

                    KlipperUserActionResult login = await client.LoginUserAsync(username, password);
                    Assert.That(login is not null);
                    Assert.That(login.Username == username);

                    currentUser = await client.GetCurrentUserAsync();
                    Assert.That(currentUser is not null);

                    KlipperUserActionResult newTokenResult = await client.RefreshJSONWebTokenAsync();
                    Assert.That(newTokenResult is not null);
                    Assert.That(client.UserToken == newTokenResult.Token);

                    string newPassword = "TestPasswordChanged";
                    KlipperUserActionResult refreshPassword = await client.ResetUserPasswordAsync(password, newPassword);
                    Assert.That(refreshPassword is not null);

                    KlipperUserActionResult logout = await client.LogoutCurrentUserAsync();
                    Assert.That(logout is not null);

                    login = await client.LoginUserAsync(username, newPassword);
                    Assert.That(login is not null);
                    Assert.That(login?.Username == username);

                    logout = await client.LogoutCurrentUserAsync();
                    Assert.That(logout is not null);

                    KlipperUserActionResult userDeleted = await client.DeleteUserAsync(username);
                    Assert.That(userDeleted is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        #endregion

        #region Database APIs
        [Test]
        public async Task DatabaseTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                client.Error += (sender, e) =>
                {
                    if (e is UnhandledExceptionEventArgs args)
                        Console.WriteLine(args.ExceptionObject?.ToString());
                    //Assert.Fail(e.ToString());
                };
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    //List<string> namespaces = await client.ListDatabaseNamespacesAsync();
                    Assert.That(client.AvailableNamespaces?.Count > 0);
                    if (client.OperatingSystem == MoonrakerOperatingSystems.FluiddPi)
                    {
                        client.ApiKey = _api;
                    }

                    string currentNamespace = client.OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                    Dictionary<string, object> items = await client.GetDatabaseItemAsync(currentNamespace);
                    Assert.That(items?.Count > 0);

                    foreach (KeyValuePair<string, object> pair in items)
                    {
                        Type type = pair.Value.GetType();
                        if (pair.Value is JObject jObject)
                        {
                            foreach (var property in jObject.Properties())
                            {
                                Dictionary<string, object> childItems = await client.GetDatabaseItemAsync(currentNamespace, property.Name);
                            }
                        }
                    }

                    Dictionary<string, object> moonrakerItems = await client.GetDatabaseItemAsync("moonraker");

                    Dictionary<string, object> webcams = await client.GetDatabaseItemAsync(currentNamespace,
                        client.OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "webcam" : "cameras");
                    Assert.That(webcams?.Count > 0);

                    List<IWebCamConfig>? webcamConfig = await client.GetWebCamSettingsAsync();
                    Assert.That(webcamConfig?.Count > 0);
                    if (webcamConfig.Count > 0)
                        Assert.That(!string.IsNullOrEmpty(webcamConfig.FirstOrDefault()?.WebCamUrlDynamic?.ToString()));

                    List<KlipperDatabaseTemperaturePreset> presets = await client.GetDashboardPresetsAsync();
                    Assert.That(presets?.Count > 0);

                    if (client.OperatingSystem == MoonrakerOperatingSystems.MainsailOS)
                    {
                        KlipperDatabaseMainsailValueHeightmapSettings? heightmap = await client.GetMeshHeightMapSettingsAsync();
                        Assert.That(heightmap is not null);
                    }

                    Dictionary<string, object> add = await client.AddDatabaseItemAsync(currentNamespace, "testkey", 56);
                    Assert.That(add?.Count > 0);

                    Dictionary<string, object> delete = await client.DeleteDatabaseItemAsync(currentNamespace, "testkey");
                    Assert.That(delete?.Count > 0);

                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }

        [Test]
        public async Task RemotePrintersTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                client.Error += (sender, e) =>
                {
                    if (e is UnhandledExceptionEventArgs args)
                        Console.WriteLine(args.ExceptionObject?.ToString());
                    Assert.Fail(e.ToString());
                };
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    //List<string> namespaces = await client.ListDatabaseNamespacesAsync();
                    Assert.That(client.AvailableNamespaces?.Count > 0);

                    string currentNamespace = client.OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                    List<KlipperDatabaseRemotePrinter> printers = await client.GetRemotePrintersAsync();
                    Assert.That(printers?.Count > 0);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        #endregion

        #region Job Queue
        [Test]
        public async Task ActiveJobTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperJobQueueResult jobstatus = await client.GetJobQueueStatusAsync();
                    Assert.That(jobstatus is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        [Test]
        public async Task JobQueueTest()
        {
            try
            {
                if (_skipOnlineTests) return;

                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperJobQueueResult? jobstatus = await client.GetJobQueueStatusAsync();
                    Assert.That(jobstatus is not null);

                    List<IGcode> files = await client.GetAvailableFilesAsync();
                    Assert.That(files?.Count > 0);

                    string? fileName = files[0]?.FilePath;
                    KlipperJobQueueResult? queued = await client.EnqueueJobsAsync(new string[] { fileName });

                    jobstatus = await client.GetJobQueueStatusAsync();
                    Assert.That(jobstatus is not null);

                    KlipperJobQueueResult? start = await client.StartJobQueueAsync();
                    Assert.That(start is not null);

                    KlipperJobQueueResult? pause = await client.PauseJobQueueAsync();
                    Assert.That(pause is not null);

                    KlipperJobQueueResult? removedAll = await client.RemoveAllJobAsync();
                    Assert.That(removedAll is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        #endregion

        #region Power APIs
        [Test]
        public async Task PowerApisTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    List<KlipperDevice> devices = await client.GetDeviceListAsync();
                    Assert.That(devices?.Count > 0);

                    string device = devices[0].Device;
                    Dictionary<string, string> status = await client.GetDeviceStatusAsync(device);
                    Assert.That(status?.Count > 0);
                    Assert.That(status["printer"] == "off");

                    Dictionary<string, string> stateChanged = await client.SetDeviceStateAsync(device, KlipperDeviceActions.On);
                    Assert.That(stateChanged?.Count > 0);
                    Assert.That(stateChanged["printer"] == "on");

                    stateChanged = await client.SetDeviceStateAsync(device, KlipperDeviceActions.Off);
                    Assert.That(stateChanged?.Count > 0);
                    Assert.That(stateChanged["printer"] == "off");
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        #endregion

        #region Update Manager API
        [Test]
        public async Task UpdateManagerApiTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    KlipperUpdateStatusResult status = await client.GetUpdateStatusAsync();
                    Assert.That(status is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        #endregion

        #region Octoprint
        [Test]
        public async Task OctoprintApiTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    OctoprintApiVersionResult version = await client.GetOctoPrintApiVersionInfoAsync();
                    Assert.That(version is not null);

                    OctoprintApiServerStatusResult server = await client.GetOctoPrintApiServerStatusAsync();
                    Assert.That(server is not null);

                    OctoprintApiSettingsResult settings = await client.GetOctoPrintApiSettingsAsync();
                    Assert.That(settings is not null);

                    OctoprintApiJobResult jobStatus = await client.GetOctoPrintApiJobStatusAsync();
                    Assert.That(jobStatus is not null);

                    OctoprintApiPrinterStatusResult printerStatus = await client.GetOctoPrintApiPrinterStatusAsync();
                    Assert.That(printerStatus is not null);

                    Dictionary<string, OctoprintApiPrinter> printerProfiles = await client.GetOctoPrintApiPrinterProfilesAsync();
                    Assert.That(printerProfiles?.Count > 0);

                    bool gcodeCommand = await client.SendOctoPrintApiGcodeCommandAsync("G28");
                    Assert.That(gcodeCommand);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        #endregion

        #region History APIs
        [Test]
        public async Task HistoryApiTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    List<KlipperJobItem> history = await client.GetHistoryJobListAsync();
                    Assert.That(history?.Count > 0);

                    KlipperHistoryJobTotalsResult total = await client.GetHistoryTotalJobsAsync();
                    Assert.That(total is not null);

                    string uid = history[0].JobId;
                    KlipperJobItem job = await client.GetHistoryJobAsync(uid);
                    Assert.That(job is not null);

                    List<string> deletedIds = await client.DeleteHistoryJobAsync(uid);
                    Assert.That(deletedIds?.Count > 0);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        #endregion

        #region Print3dServer implementation tests
        [Test]
        public async Task Print3dServerApiTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                await client.CheckOnlineAsync();
                if (client.IsOnline)
                {
                    await client.RefreshAllAsync();
                    Assert.That(client.InitialDataFetched);

                    List<IPrinter3d> printers = await client.GetPrintersAsync();
                    Assert.That(printers?.Count > 0);

                    List<IGcode> gcodes = await client.GetFilesAsync();
                    Assert.That(gcodes?.Count > 0);

                    IToolhead? toolheads = await client.GetExtruderStatusAsync();
                    Assert.That(toolheads is not null);

                    IPrint3dFan? fan = await client.GetFanStatusAsync();
                    Assert.That(fan is not null);

                    IHeaterComponent? heaterBed = await client.GetHeaterBedStatusAsync();
                    Assert.That(heaterBed is not null);
                }
                else
                    Assert.Fail($"Server {client.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
            finally
            {

            }
        }
        #endregion

        [Test]
        public async Task OnlineTest()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                client.Error += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                client.ServerWentOffline += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                await client.CheckOnlineAsync(3500);
                await client.CheckOnlineAsync(3500);
                await client.CheckOnlineAsync(3500);
                // Wait 10 minutes
                CancellationTokenSource cts = new(new TimeSpan(0, 10, 0));
                do
                {
                    await Task.Delay(10000);
                    await client.CheckOnlineAsync();
                    Debug.WriteLine($"Online: {client.IsOnline}");
                    //await client.RefreshAllAsync();
                } while (client.IsOnline && !cts.IsCancellationRequested);
                Assert.That(cts.IsCancellationRequested);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task WebsocketTest()
        {
            try
            {
                Dictionary<DateTime, string> websocketMessages = [];
                string host = $"{(_ssl ? "https://" : "http://")}{_host}:{_port}";
                string ws = $"{(_ssl ? "wss://" : "ws://")}{_host}:{_port}/socket";

                MoonrakerClient client = new(host)
                {
                    LoginRequired = false,
                    WebSocketTargetUri = ws,
                };

                await client.CheckOnlineAsync();
                Assert.That(client.IsOnline);

                await client.RefreshAllAsync();

                if (client.LoginRequired)
                {
                    await client.LoginUserAsync("TestUser", "TestPassword");
                }
                await client.StartListeningAsync(commandsOnConnect: wsStartCommands);

                client.WebSocketConnectionIdChanged += (o, args) =>
                {
                    Assert.That(args?.ConnectionId is not null);
                    Assert.That(args?.ConnectionId > 0);
                    Task.Run(async () =>
                    {
                        string subResult = await client.SubscribeAllPrinterObjectStatusAsync(args.ConnectionId);
                    });
                };

                client.Error += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                client.ServerWentOffline += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };

                // Subscirbe to state changes
                client.ToolheadsChanged += (o, args) =>
                {
                    foreach (var pair in args.Toolheads)
                    {
                        Debug.WriteLine($"Extruder{pair.Key}: {pair.Value?.TempRead} �C (Target: {pair.Value?.TempSet} �C)");
                    }
                };
                client.HeatersChanged += (o, args) =>
                {
                    foreach (var pair in args.Heaters)
                    {
                        Debug.WriteLine($"HeatedBed{pair.Key}: {pair.Value?.TempRead} �C (Target: {pair.Value?.TempSet} �C)");
                    }
                };
                client.KlipperDisplayStatusChanged += (o, args) =>
                {
                    if (args.NewDisplayStatus != null)
                        Debug.WriteLine($"Progress: {args.NewDisplayStatus.Progress * 100} % (Msg: {args.NewDisplayStatus.Message})");
                };

                client.KlipperToolHeadStateChanged += (o, args) =>
                {
                    //Debug.WriteLine($"Toolhead: {args.ToolheadStates.EstimatedPrintTime}");
                };
                client.KlipperPrintStateChanged += (o, args) =>
                {
                    //Debug.WriteLine($"PrintState: New => {args.NewPrintState.State}; Previous => {args.PreviousPrintState?.State}");
                };

                client.WebSocketMessageReceived += (o, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Message))
                    {
                        websocketMessages.Add(DateTime.Now, args.Message);
                        //Debug.WriteLine($"WebSocket Data: {args.Message} (Total: {websocketMessages.Count})");
                    }
                };

                client.WebSocketError += (o, args) =>
                {
                    Assert.Fail($"Websocket closed due to an error: {args}");
                };

                // Wait a few minutes
                CancellationTokenSource cts = new(new TimeSpan(0, 10, 0));
                client.WebSocketDisconnected += (o, args) =>
                {
                    if (!cts.IsCancellationRequested)
                        Assert.Fail($"Websocket unexpectly closed: {args}");
                };

                do
                {
                    await Task.Delay(10000);
                    await client.CheckOnlineAsync();
                    Debug.WriteLine($"Online: {client.IsOnline}");
                } while (client.IsOnline && !cts.IsCancellationRequested);
                await client.StopListeningAsync();

                StringBuilder sb = new();
                foreach (var pair in websocketMessages)
                {
                    sb.AppendLine($"{pair.Key}: {pair.Value}");
                }
                await File.WriteAllTextAsync("ws_messages.txt", sb.ToString());

                Assert.That(cts.IsCancellationRequested);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        [Test]
        public async Task VeryLongWebsocketTest()
        {
            try
            {
                Dictionary<DateTime, string> websocketMessages = [];
                string host = $"{(_ssl ? "https://" : "http://")}{_host}:{_port}";
                string ws = $"{(_ssl ? "wss://" : "ws://")}{_host}:{_port}/socket";

                MoonrakerClient client = new(host)
                {
                    LoginRequired = false,
                    WebSocketTargetUri = ws,
                };

                await client.CheckOnlineAsync();
                Assert.That(client.IsOnline);

                await client.RefreshAllAsync();

                //var test = await client.GetActiveJobStatusAsync();

                if (client.LoginRequired)
                {
                    await client.LoginUserAsync("TestUser", "TestPassword");
                }
                //KlipperAccessTokenResult oneshot = await client.GetOneshotTokenAsync();
                //client.OneShotToken = oneshot.Result;
                await client.StartListeningAsync(commandsOnConnect: wsStartCommands);

                client.WebSocketConnectionIdChanged += (o, args) =>
                {
                    Assert.That(args?.ConnectionId is not null);
                    Assert.That(args?.ConnectionId > 0);
                    Task.Run(async () =>
                    {
                        string subResult = await client.SubscribeAllPrinterObjectStatusAsync(args.ConnectionId);
                    });
                };

                client.Error += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                client.ServerWentOffline += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };

                // Subscirbe to state changes
                client.ToolheadsChanged += (o, args) =>
                {
                    foreach (var pair in args.Toolheads)
                    {
                        Debug.WriteLine($"Extruder{pair.Key}: {pair.Value?.TempRead} �C (Target: {pair.Value?.TempSet} �C)");
                    }
                };
                client.HeatersChanged += (o, args) =>
                {
                    foreach (var pair in args.Heaters)
                    {
                        Debug.WriteLine($"HeatedBed{pair.Key}: {pair.Value?.TempRead} �C (Target: {pair.Value?.TempSet} �C)");
                    }
                };
                client.KlipperDisplayStatusChanged += (o, args) =>
                {
                    if (args.NewDisplayStatus != null)
                        Debug.WriteLine($"Progress: {args.NewDisplayStatus?.Progress * 100} % (Msg: {args.NewDisplayStatus?.Message})");
                };

                client.KlipperToolHeadStateChanged += (o, args) =>
                {
                    //Debug.WriteLine($"Toolhead: {args.ToolheadStates.EstimatedPrintTime}");
                };
                client.KlipperPrintStateChanged += (o, args) =>
                {
                    //Debug.WriteLine($"PrintState: New => {args.NewPrintState.State}; Previous => {args.PreviousPrintState?.State}");
                };

                client.WebSocketMessageReceived += (o, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Message))
                    {
                        websocketMessages.Add(DateTime.Now, args.Message);
                        Debug.WriteLine($"WebSocket Data: {args.Message} (Total: {websocketMessages.Count})");
                    }
                };

                client.WebSocketError += (o, args) =>
                {
                    Assert.Fail($"Websocket closed due to an error: {args}");
                };

                // Wait a 90 minutes
                CancellationTokenSource cts = new(new TimeSpan(1, 30, 0));
                client.WebSocketDisconnected += (o, args) =>
                {
                    if (!cts.IsCancellationRequested)
                        Assert.Fail($"Websocket unexpectly closed: {args}");
                };

                do
                {
                    await Task.Delay(10000);
                    await client.CheckOnlineAsync();
                    Debug.WriteLine($"Online: {client.IsOnline}");
                    if (client.IsPrinting)
                    {
                        if (client.ActiveJob is not null)
                        {

                        }
                    }
                } while (client.IsOnline && !cts.IsCancellationRequested);
                await client.StopListeningAsync();

                StringBuilder sb = new();
                foreach (var pair in websocketMessages)
                {
                    sb.AppendLine($"{pair.Key}: {pair.Value}");
                }
                await File.WriteAllTextAsync("ws_messages.txt", sb.ToString());

                Assert.That(cts.IsCancellationRequested);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
    }
}