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
    public class Tests
    {
        private readonly string _host = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").Ip ?? "";
        private readonly int _port = 80;
        private readonly string _api = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").ApiKey ?? "";
        private readonly string _user = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").Username ?? "";
        private readonly string _pwd = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").Password ?? "";
        private readonly bool _ssl = false;

        private readonly bool _skipOnlineTests = true;

        private string[] wsStartCommands = new string[]
        { 
            // Needed to get the ConnectionId
            $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.info\",\"params\":{{}},\"id\":1}}",
            $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.websocket.id\",\"params\":{{}},\"id\":2}}",
        };

        MoonrakerClient? client = null;

        [SetUp]
        public void Setup()
        {
            client = new MoonrakerClient.MoonrakerConnectionBuilder()
                .WithName("Test")
                .WithServerAddress(_host, _port, _ssl)
                .WithApiKey(_api)
                .Build();
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
        }

        [Test]
        public async Task BuildWitOneShotTokenAsync()
        {
            try
            {
                var client = new MoonrakerClient.MoonrakerConnectionBuilder()
                    .WithName("Test")
                    .WithServerAddress(_host, _port, _ssl)
                    .Build();
                KlipperAccessTokenResult? token = await client.GetOneshotTokenAsync();

                client.OneShotToken = token?.Result ?? string.Empty;
                Assert.That(!string.IsNullOrEmpty(client.OneShotToken));

                KlipperMachineInfo? info = await client.GetMachineSystemInfoAsync();
                Assert.That(info is not null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task BuildWitUserTokenAsync()
        {
            try
            {
                Assert.That(!string.IsNullOrEmpty(_user) && !string.IsNullOrEmpty(_pwd), "Provide a user and password in your secrets.json file first!");
                var client = new MoonrakerClient.MoonrakerConnectionBuilder()
                    .WithName("Test")
                    .WithServerAddress(_host, _port, _ssl)
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
                MoonrakerClient.Instance = new MoonrakerClient(_host, _api, _port, _ssl)
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
                MoonrakerClient.Instance = new MoonrakerClient(_host, _api, _port, _ssl)
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
                List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && !t.Name.StartsWith("<") && t.Namespace?.StartsWith("AndreasReitberger.API.Moonraker") is true)
                       .ToList()
                       ;
                //Regex r = new(@"(?<=\"")[A-Z]*[A-Z][a-zA-Z]*(?=\"")");
                Regex r = new(@"^[A-Z][A-Za-z0-9]*$");
                Regex extract = new(@"(?<=\"").+?(?=\"")");
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
                    List<PropertyInfo> p = t
                        .GetProperties()
                        .Where(prop => prop.GetCustomAttribute<JsonPropertyAttribute>(true) is not null)
                        .ToList()
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
                    List<string> properties = splitted
                        .Select(row => extract.Match(row ?? "")?.Value ?? string.Empty)
                        .ToList()
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
                    MoonrakerClient.Instance = new MoonrakerClient(_host, _api, _port, _ssl)
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperAccessTokenResult? token2 = await _server.GetApiKeyAsync();
                    Assert.That(!string.IsNullOrEmpty(token2?.Result));

                    Dictionary<string, object> objectList = await _server.QueryPrinterObjectStatusAsync(new() { { "gcode_move", "" } });
                    Assert.That(objectList?.Count > 0);

                    objectList = await _server.QueryPrinterObjectStatusAsync(new() { { "toolhead", "position,status" } });
                    Assert.That(objectList?.Count > 0);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args?.Message);
                    };

                    await _server.RefreshAllAsync();
                    await Task.Delay(250);

                    Assert.That(_server.InitialDataFetched);

                    Assert.That(_server.IdleState is not null);
                    Assert.That(_server.DisplayStatus is not null);
                    Assert.That(_server.GcodeMove is not null);
                    Assert.That(_server.GcodeMeta is not null);
                    Assert.That(_server.VirtualSdCard is not null);

                    Assert.That(_server.HeatedBeds?.Count > 0);
                    Assert.That(_server.HeatedBeds?.FirstOrDefault().Value?.TempRead > 0);
                    Assert.That(_server.ActiveHeatedBed?.TempRead > 0);

                    Assert.That(_server.Toolheads?.Count > 0);
                    Assert.That(_server.Toolheads?.FirstOrDefault().Value?.TempRead > 0);
                    Assert.That(_server.ActiveToolhead?.TempRead > 0);

                    Assert.That(_server.PrintStats is not null);

                    Assert.That(_server.Files is not null);
                    Assert.That(_server.Files?.Count > 0);

                    Assert.That(_server.Fans is not null);
                    Assert.That(_server.Fans?.Count > 0);
                    Assert.That(_server.ActiveFan?.Speed > 0);
                    if (_server.IsPrinting)
                    {
                        //Assert.That(_server.ActiveJob is not null);
                    }
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };

                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    List<IWebCamConfig>? webcamConfigs = await _server.GetWebCamSettingsFromDatabaseAsync();
                    Assert.That(webcamConfigs?.Count > 0);

                    webcamConfigs = await _server.GetWebCamSettingsAsync();
                    Assert.That(webcamConfigs?.Count > 0);

                    string webcamUri = await _server.GetWebCamUriAsync(0, false);
                    Assert.That(_server.WebCams?.Count > 0);
                    Assert.That(_server.WebCams?.Count > 0);
                    Assert.That(!string.IsNullOrEmpty(webcamUri));
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };
                    await _server.StartListeningAsync(commandsOnConnect: wsStartCommands);

                    Dictionary<string, KlipperStatusFilamentSensor> fSensors = await _server.GetFilamentSensorsAsync();
                    Assert.That(fSensors?.Count > 0);

                    Dictionary<string, string> macros = new();
                    List<string> availableMacros = await _server.GetPrinterObjectListAsync("gcode_macro");
                    for (int i = 0; i < availableMacros.Count; i++)
                    {
                        macros.Add(availableMacros[i], string.Empty);
                    }

                    Dictionary<string, KlipperGcodeMacro> gcMacros = await _server.GetGcodeMacrosAsync();
                    Assert.That(gcMacros?.Count > 0);

                    List<string> objects = await _server.GetPrinterObjectListAsync();
                    Assert.That(objects?.Count > 0);

                    KlipperEndstopQueryResult endstops = await _server.QueryEndstopsAsync();
                    Assert.That(endstops is not null);

                    macros.Clear();
                    Dictionary<string, string> targets = new();
                    for (int i = 0; i < objects.Count; i++)
                    {
                        targets.Add(objects[i], string.Empty);
                        if (objects[i].StartsWith("gcode_macro"))
                            macros.Add(objects[i], string.Empty);
                    }
                    //targets.Add("gcode_move", "");
                    //targets.Add("toolhead", "");
                    //targets.Add("extruder", "target,temperature");

                    Dictionary<string, KlipperGcodeMacro> availableMarcros = await _server.GetGcodeMacrosAsync();
                    Assert.That(availableMarcros?.Count > 0);

                    Dictionary<string, object> objectStates = await _server.QueryPrinterObjectStatusAsync(targets);
                    Assert.That(objectStates?.Count > 0);

                    long? connectionId = _server.WebSocketConnectionId; // ""; // Get from WebSocket?
                    string result = await _server.SubscribePrinterObjectStatusAsync(connectionId, objects);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };

                    Dictionary<string, string> macros = new();
                    List<string> availableMacros = await _server.GetPrinterObjectListAsync("gcode_macro", true);
                    for (int i = 0; i < availableMacros.Count; i++)
                    {
                        macros.Add(availableMacros[i], string.Empty);
                    }

                    macros.Clear();
                    List<string> objects = await _server.GetPrinterObjectListAsync("gcode_macro");
                    for (int i = 0; i < objects.Count; i++)
                    {
                        macros.Add(objects[i], string.Empty);
                    }

                    Dictionary<string, KlipperGcodeMacro> gcMacros = await _server.GetGcodeMacrosAsync();
                    Assert.That(gcMacros?.Count > 0);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                _server.Error += (sender, e) =>
                {
                    Assert.Fail(e.ToString());
                };
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };

                    List<IGcode> models = await _server.GetAvailableFilesAsync("gcodes", true);
                    //var childItems = models?.Where(model => model.Path.Contains("/")).ToList();
                    Assert.That(models?.Count > 0);
                    foreach (KlipperFile? gcodeFile in models?.Cast<KlipperFile>()?.Where(g => g?.Meta?.GcodeImages?.Count > 0))
                    {
                        byte[] thumbnail = await _server.GetGcodeThumbnailImageAsync(gcodeFile?.Meta);
                        Assert.That(thumbnail is not null);
                    }
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperPrinterStateMessageResult info = await _server.GetPrinterInfoAsync();
                    Assert.That(info is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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

                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    int fanSpeed = 128;
                    string gcode = $"M106 S{fanSpeed}";
                    bool result = await _server.RunGcodeScriptAsync(gcode);
                    Assert.That(result);

                    fanSpeed = 0;
                    gcode = $"M106 S{fanSpeed}";
                    result = await _server.RunGcodeScriptAsync(gcode);
                    Assert.That(result);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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

                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    var homed = await _server.HomeAxesAsync(true, true, true);
                    Assert.That(homed, "Not homed!");

                    // Move all 3 axes to ensure they all move correctly
                    var newX = 100;
                    var newY = 100;
                    var newZ = 100;
                    var moved = await _server.MoveAxesAsync(
                        speed: 6000,
                        x: newX,
                        y: newY,
                        z: newZ,
                        relative: false);
                    var end = await _server.GetToolHeadStatusAsync();
                    Assert.That(moved, "Did not move");
                    Assert.That(newX == end.Position[0], message: "X didn't move as expected");
                    Assert.That(newY == end.Position[1], message: "Y didn't move as expected");
                    Assert.That(newZ == end.Position[2], message: "Z didn't move as expected");

                    // Move all 3 axes independently to make sure they move as expected
                    newX = 50;
                    newY = 50;
                    newZ = 50;
                    await _server.MoveAxesAsync(
                        speed: 6000,
                        x: newX);
                    await _server.MoveAxesAsync(
                        speed: 6000,
                        y: newY);
                    await _server.MoveAxesAsync(
                        speed: 6000,
                        z: newZ);
                    end = await _server.GetToolHeadStatusAsync();
                    Assert.That(newX == end.Position[0], message: "X didn't move as expected");
                    Assert.That(newY == end.Position[1], message: "Y didn't move as expected");
                    Assert.That(newZ == end.Position[2], message: "Z didn't move as expected");
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    bool fan = await _server.SetFanSpeedTargetAsync(128, false);
                    fan = await _server.SetFanSpeedTargetAsync(0, false);
                    fan = await _server.SetFanSpeedTargetAsync(100, true);
                    fan = await _server.SetFanSpeedTargetAsync(0, false);

                    bool extruder = await _server.SetExtruderTargetAsync(50);
                    extruder = await _server.SetExtruderTargetAsync(50, 1);
                    await Task.Delay(5000);
                    extruder = await _server.SetExtruderTargetAsync(0);
                    extruder = await _server.SetExtruderTargetAsync(0, 1);

                    bool heaterBed = await _server.SetHeaterBedTargetAsync(50);
                    await Task.Delay(5000);
                    heaterBed = await _server.SetHeaterBedTargetAsync(0);

                    string fileName = "test.gcode";
                    bool printStarted = await _server.PrintFileAsync(fileName);
                    Assert.That(printStarted);

                    if (printStarted)
                    {
                        // Wait a minute
                        await Task.Delay(60 * 1000);
                        bool paused = await _server.PausePrintAsync();
                        await Task.Delay(5000);
                        Assert.That(paused);

                        if (paused)
                        {
                            bool resumed = await _server.ResumePrintAsync();
                            await Task.Delay(5000);
                            Assert.That(resumed);

                            if (resumed)
                            {
                                bool cancelled = await _server.CancelPrintAsync();
                                await Task.Delay(5000);
                                Assert.That(cancelled);
                            }
                        }
                    }
                }
                else
                {
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    //var token = await _server.GetOneshotTokenAsync();
                    KlipperAccessTokenResult token2 = await _server.GetApiKeyAsync();
                    Assert.That(!string.IsNullOrEmpty(token2.Result));
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperServerConfig config = await _server.GetServerConfigAsync();
                    Assert.That(config is not null);

                    Dictionary<string, KlipperTemperatureSensorHistory> tempData = await _server.GetServerCachedTemperatureDataAsync();
                    Assert.That(tempData?.Count > 0);

                    List<KlipperGcode> cachedGcodes = await _server.GetServerCachedGcodesAsync();
                    Assert.That(cachedGcodes?.Count > 0);

                    if (!_skipOnlineTests)
                    {
                        bool restart = await _server.RestartServerAsync();
                        Assert.That(restart);
                    }
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    Dictionary<string, string> help = await _server.GetGcodeHelpAsync();
                    Assert.That(help?.Count > 0);
                    if (!_skipOnlineTests)
                    {
                        bool succeed = await _server.RunGcodeScriptAsync("G28");
                        Assert.That(succeed);
                    }
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperMachineInfo info = await _server.GetMachineSystemInfoAsync();
                    Assert.That(info is not null);

                    bool restarted = await _server.RestartSystemServiceAsync(KlipperServices.webcamd);
                    Assert.That(restarted);
                    await Task.Delay(10 * 1000);

                    bool stopped = await _server.StopSystemServiceAsync(KlipperServices.webcamd);
                    Assert.That(stopped);
                    await Task.Delay(10 * 1000);

                    bool started = await _server.StartSystemServiceAsync(KlipperServices.webcamd);
                    Assert.That(started);
                    await Task.Delay(10 * 1000);

                    bool rebooted = await _server.MachineRebootAsync();
                    Assert.That(rebooted);
                    await Task.Delay(10 * 1000);

                    bool shutdown = await _server.MachineShutdownAsync();
                    Assert.That(shutdown);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperMoonrakerProcessStatsResult info = await _server.GetMoonrakerProcessStatsAsync();
                    Assert.That(info is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    List<IGcode> files = await _server.GetAvailableFilesAsync();
                    Assert.That(files?.Count > 0);

                    string? fileName = files[0]?.FilePath;
                    KlipperGcodeMetaResult meta = await _server.GetGcodeMetadataAsync(fileName);
                    Assert.That(meta is not null);


                    files = await _server.GetAvailableFilesAsync("config");
                    Assert.That(files?.Count > 0);

                    files = await _server.GetAvailableFilesAsync("config_examples");
                    Assert.That(files?.Count > 0);

                    files = await _server.GetAvailableFilesAsync("docs ");
                    Assert.That(files?.Count > 0);

                    string dirName = "gcodes/Kundenaufträge";
                    KlipperDirectoryActionResult created = await _server.CreateDirectoryAsync(dirName);
                    Assert.That(created is not null);

                    KlipperDirectoryActionResult copyFile = await _server.CopyDirectoryOrFileAsync($"gcodes/{fileName}", $"{dirName}/{fileName}");
                    Assert.That(copyFile is not null);

                    KlipperDirectoryInfoResult dirs = await _server.GetDirectoryInformationAsync(dirName);
                    Assert.That(dirs is not null);

                    KlipperDirectoryActionResult deleteFile = await _server.DeleteFileAsync($"{dirName}/{fileName}");
                    Assert.That(deleteFile is not null);

                    KlipperDirectoryActionResult deleted = await _server.DeleteDirectoryAsync(dirName);
                    Assert.That(deleted is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    string testFile = @"Files/Groot_0.2mm_PETG_MK3S_11h28m.gcode";

                    Assert.That(File.Exists(testFile));
                    FileInfo info = new(testFile);
                    string fullPath = info.FullName;

                    byte[]? file = await File.ReadAllBytesAsync(testFile);

                    KlipperFileActionResult? msg = await _server.UploadFileAsync(localFilePath: fullPath, timeout: 100000);
                    KlipperDirectoryActionResult? deleted = await _server.DeleteFileAsync("gcodes", info.Name);

                    msg = await _server.UploadFileAsync(fileName: info.Name, file: file, timeout: 100000);
                    Assert.That(msg is not null);

                    KlipperGcodeMetaResult? meta = await _server.GetGcodeMetadataAsync(msg?.Item?.Path);
                    Assert.That(meta is not null);

                    string? thumbnail = meta?.GcodeImages.FirstOrDefault()?.Path;
                    // Get small image (30x30)
                    byte[] image = await _server.GetGcodeThumbnailImageAsync(thumbnail);
                    // Get big image (400x300)
                    byte[] image2 = await _server.GetGcodeThumbnailImageAsync(meta, 1);

                    deleted = await _server.DeleteFileAsync("gcodes", info.Name);
                    Assert.That(deleted is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    byte[] logFile = await _server.DownloadLogFileAsync(KlipperLogFileTypes.Klippy);
                    await File.WriteAllBytesAsync("klippy.log", logFile);
                    Assert.That(logFile?.Length > 0);

                    logFile = await _server.DownloadLogFileAsync(KlipperLogFileTypes.Moonraker);
                    await File.WriteAllBytesAsync("moonraker.log", logFile);
                    Assert.That(logFile?.Length > 0);

                    File.Delete("klippy.log");
                    File.Delete("moonraker.log");
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    string username = "TestUser";
                    string password = "TestPassword";

                    KlipperUserActionResult login2 = await _server.LoginUserAsync(username, password);

                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperUserActionResult userCreated = await _server.CreateUserAsync(username, password);
                    Assert.That(userCreated is not null);

                    List<KlipperUser> users = await _server.ListAvailableUsersAsync();
                    Assert.That(users?.Count > 0);

                    KlipperUser currentUser = await _server.GetCurrentUserAsync();
                    if (currentUser != null)
                    {
                        //Assert.IsNotNull(await _server.LogoutCurrentUserAsync());
                    }

                    KlipperUserActionResult login = await _server.LoginUserAsync(username, password);
                    Assert.That(login is not null);
                    Assert.That(login.Username == username);

                    currentUser = await _server.GetCurrentUserAsync();
                    Assert.That(currentUser is not null);

                    KlipperUserActionResult newTokenResult = await _server.RefreshJSONWebTokenAsync();
                    Assert.That(newTokenResult is not null);
                    Assert.That(_server.UserToken == newTokenResult.Token);

                    string newPassword = "TestPasswordChanged";
                    KlipperUserActionResult refreshPassword = await _server.ResetUserPasswordAsync(password, newPassword);
                    Assert.That(refreshPassword is not null);

                    KlipperUserActionResult logout = await _server.LogoutCurrentUserAsync();
                    Assert.That(logout is not null);

                    login = await _server.LoginUserAsync(username, newPassword);
                    Assert.That(login is not null);
                    Assert.That(login?.Username == username);

                    logout = await _server.LogoutCurrentUserAsync();
                    Assert.That(logout is not null);

                    KlipperUserActionResult userDeleted = await _server.DeleteUserAsync(username);
                    Assert.That(userDeleted is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                _server.Error += (sender, e) =>
                {
                    if (e is UnhandledExceptionEventArgs args)
                        Console.WriteLine(args.ExceptionObject?.ToString());
                    //Assert.Fail(e.ToString());
                };
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    //List<string> namespaces = await _server.ListDatabaseNamespacesAsync();
                    Assert.That(_server.AvailableNamespaces?.Count > 0);
                    if (_server.OperatingSystem == MoonrakerOperatingSystems.FluiddPi)
                    {
                        _server.ApiKey = _api;
                    }

                    string currentNamespace = _server.OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                    Dictionary<string, object> items = await _server.GetDatabaseItemAsync(currentNamespace);
                    Assert.That(items?.Count > 0);

                    foreach (KeyValuePair<string, object> pair in items)
                    {
                        Type type = pair.Value.GetType();
                        if (pair.Value is JObject jObject)
                        {
                            foreach (var property in jObject.Properties())
                            {
                                Dictionary<string, object> childItems = await _server.GetDatabaseItemAsync(currentNamespace, property.Name);
                            }
                        }
                    }

                    Dictionary<string, object> moonrakerItems = await _server.GetDatabaseItemAsync("moonraker");

                    Dictionary<string, object> webcams = await _server.GetDatabaseItemAsync(currentNamespace,
                        _server.OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "webcam" : "cameras");
                    Assert.That(webcams?.Count > 0);

                    List<IWebCamConfig>? webcamConfig = await _server.GetWebCamSettingsAsync();
                    Assert.That(webcamConfig?.Count > 0);
                    if (webcamConfig.Count > 0)
                        Assert.That(!string.IsNullOrEmpty(webcamConfig.FirstOrDefault()?.WebCamUrlDynamic?.ToString()));

                    List<KlipperDatabaseTemperaturePreset> presets = await _server.GetDashboardPresetsAsync();
                    Assert.That(presets?.Count > 0);

                    if (_server.OperatingSystem == MoonrakerOperatingSystems.MainsailOS)
                    {
                        KlipperDatabaseMainsailValueHeightmapSettings? heightmap = await _server.GetMeshHeightMapSettingsAsync();
                        Assert.That(heightmap is not null);
                    }

                    Dictionary<string, object> add = await _server.AddDatabaseItemAsync(currentNamespace, "testkey", 56);
                    Assert.That(add?.Count > 0);

                    Dictionary<string, object> delete = await _server.DeleteDatabaseItemAsync(currentNamespace, "testkey");
                    Assert.That(delete?.Count > 0);

                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                _server.Error += (sender, e) =>
                {
                    if (e is UnhandledExceptionEventArgs args)
                        Console.WriteLine(args.ExceptionObject?.ToString());
                    Assert.Fail(e.ToString());
                };
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    //List<string> namespaces = await _server.ListDatabaseNamespacesAsync();
                    Assert.That(_server.AvailableNamespaces?.Count > 0);

                    string currentNamespace = _server.OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                    List<KlipperDatabaseRemotePrinter> printers = await _server.GetRemotePrintersAsync();
                    Assert.That(printers?.Count > 0);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperJobQueueResult jobstatus = await _server.GetJobQueueStatusAsync();
                    Assert.That(jobstatus is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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

                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperJobQueueResult? jobstatus = await _server.GetJobQueueStatusAsync();
                    Assert.That(jobstatus is not null);

                    List<IGcode> files = await _server.GetAvailableFilesAsync();
                    Assert.That(files?.Count > 0);

                    string? fileName = files[0]?.FilePath;
                    KlipperJobQueueResult? queued = await _server.EnqueueJobsAsync(new string[] { fileName });

                    jobstatus = await _server.GetJobQueueStatusAsync();
                    Assert.That(jobstatus is not null);

                    KlipperJobQueueResult? start = await _server.StartJobQueueAsync();
                    Assert.That(start is not null);

                    KlipperJobQueueResult? pause = await _server.PauseJobQueueAsync();
                    Assert.That(pause is not null);

                    KlipperJobQueueResult? removedAll = await _server.RemoveAllJobAsync();
                    Assert.That(removedAll is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    List<KlipperDevice> devices = await _server.GetDeviceListAsync();
                    Assert.That(devices?.Count > 0);

                    string device = devices[0].Device;
                    Dictionary<string, string> status = await _server.GetDeviceStatusAsync(device);
                    Assert.That(status?.Count > 0);
                    Assert.That(status["printer"] == "off");

                    Dictionary<string, string> stateChanged = await _server.SetDeviceStateAsync(device, KlipperDeviceActions.On);
                    Assert.That(stateChanged?.Count > 0);
                    Assert.That(stateChanged["printer"] == "on");

                    stateChanged = await _server.SetDeviceStateAsync(device, KlipperDeviceActions.Off);
                    Assert.That(stateChanged?.Count > 0);
                    Assert.That(stateChanged["printer"] == "off");
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    KlipperUpdateStatusResult status = await _server.GetUpdateStatusAsync();
                    Assert.That(status is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    OctoprintApiVersionResult version = await _server.GetOctoPrintApiVersionInfoAsync();
                    Assert.That(version is not null);

                    OctoprintApiServerStatusResult server = await _server.GetOctoPrintApiServerStatusAsync();
                    Assert.That(server is not null);

                    OctoprintApiSettingsResult settings = await _server.GetOctoPrintApiSettingsAsync();
                    Assert.That(settings is not null);

                    OctoprintApiJobResult jobStatus = await _server.GetOctoPrintApiJobStatusAsync();
                    Assert.That(jobStatus is not null);

                    OctoprintApiPrinterStatusResult printerStatus = await _server.GetOctoPrintApiPrinterStatusAsync();
                    Assert.That(printerStatus is not null);

                    Dictionary<string, OctoprintApiPrinter> printerProfiles = await _server.GetOctoPrintApiPrinterProfilesAsync();
                    Assert.That(printerProfiles?.Count > 0);

                    bool gcodeCommand = await _server.SendOctoPrintApiGcodeCommandAsync("G28");
                    Assert.That(gcodeCommand);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    List<KlipperJobItem> history = await _server.GetHistoryJobListAsync();
                    Assert.That(history?.Count > 0);

                    KlipperHistoryJobTotalsResult total = await _server.GetHistoryTotalJobsAsync();
                    Assert.That(total is not null);

                    string uid = history[0].JobId;
                    KlipperJobItem job = await _server.GetHistoryJobAsync(uid);
                    Assert.That(job is not null);

                    List<string> deletedIds = await _server.DeleteHistoryJobAsync(uid);
                    Assert.That(deletedIds?.Count > 0);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.That(_server.InitialDataFetched);

                    List<IPrinter3d> printers = await _server.GetPrintersAsync();
                    Assert.That(printers?.Count > 0);

                    List<IGcode> gcodes = await _server.GetFilesAsync();
                    Assert.That(gcodes?.Count > 0);

                    IToolhead? toolheads = await _server.GetExtruderStatusAsync();
                    Assert.That(toolheads is not null);

                    IPrint3dFan? fan = await _server.GetFanStatusAsync();
                    Assert.That(fan is not null);

                    IHeaterComponent? heaterBed = await _server.GetHeaterBedStatusAsync();
                    Assert.That(heaterBed is not null);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
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
                MoonrakerClient _server = new(_host, _api, _port, _ssl);
                _server.Error += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                _server.ServerWentOffline += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                await _server.CheckOnlineAsync(3500);
                await _server.CheckOnlineAsync(3500);
                await _server.CheckOnlineAsync(3500);
                // Wait 10 minutes
                CancellationTokenSource cts = new(new TimeSpan(0, 10, 0));
                do
                {
                    await Task.Delay(10000);
                    await _server.CheckOnlineAsync();
                    Debug.WriteLine($"Online: {_server.IsOnline}");
                    //await _server.RefreshAllAsync();
                } while (_server.IsOnline && !cts.IsCancellationRequested);
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
                Dictionary<DateTime, string> websocketMessages = new();
                MoonrakerClient _server = new(_host, _api, _port, _ssl)
                {
                    LoginRequired = false,
                };

                await _server.CheckOnlineAsync();
                Assert.That(_server.IsOnline);

                await _server.RefreshAllAsync();

                if (_server.LoginRequired)
                {
                    await _server.LoginUserAsync("TestUser", "TestPassword");
                }
                await _server.StartListeningAsync(commandsOnConnect: wsStartCommands);

                _server.WebSocketConnectionIdChanged += (o, args) =>
                {
                    Assert.That(args?.ConnectionId is not null);
                    Assert.That(args?.ConnectionId > 0);
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

                // Subscirbe to state changes
                _server.ToolheadsChanged += (o, args) =>
                {
                    foreach (var pair in args.Toolheads)
                    {
                        Debug.WriteLine($"Extruder{pair.Key}: {pair.Value?.TempRead} �C (Target: {pair.Value?.TempSet} �C)");
                    }
                };
                _server.HeatersChanged += (o, args) =>
                {
                    foreach (var pair in args.Heaters)
                    {
                        Debug.WriteLine($"HeatedBed{pair.Key}: {pair.Value?.TempRead} �C (Target: {pair.Value?.TempSet} �C)");
                    }
                };
                _server.KlipperDisplayStatusChanged += (o, args) =>
                {
                    if (args.NewDisplayStatus != null)
                        Debug.WriteLine($"Progress: {args.NewDisplayStatus.Progress * 100} % (Msg: {args.NewDisplayStatus.Message})");
                };

                _server.KlipperToolHeadStateChanged += (o, args) =>
                {
                    //Debug.WriteLine($"Toolhead: {args.ToolheadStates.EstimatedPrintTime}");
                };
                _server.KlipperPrintStateChanged += (o, args) =>
                {
                    //Debug.WriteLine($"PrintState: New => {args.NewPrintState.State}; Previous => {args.PreviousPrintState?.State}");
                };

                _server.WebSocketMessageReceived += (o, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Message))
                    {
                        websocketMessages.Add(DateTime.Now, args.Message);
                        //Debug.WriteLine($"WebSocket Data: {args.Message} (Total: {websocketMessages.Count})");
                    }
                };

                _server.WebSocketError += (o, args) =>
                {
                    Assert.Fail($"Websocket closed due to an error: {args}");
                };

                // Wait a few minutes
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
                    Debug.WriteLine($"Online: {_server.IsOnline}");
                } while (_server.IsOnline && !cts.IsCancellationRequested);
                await _server.StopListeningAsync();

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
                Dictionary<DateTime, string> websocketMessages = new();
                MoonrakerClient _server = new(_host, _api, _port, _ssl)
                {
                    LoginRequired = false,
                };

                await _server.CheckOnlineAsync();
                Assert.That(_server.IsOnline);

                await _server.RefreshAllAsync();

                //var test = await _server.GetActiveJobStatusAsync();

                if (_server.LoginRequired)
                {
                    await _server.LoginUserAsync("TestUser", "TestPassword");
                }
                //KlipperAccessTokenResult oneshot = await _server.GetOneshotTokenAsync();
                //_server.OneShotToken = oneshot.Result;
                await _server.StartListeningAsync(commandsOnConnect: wsStartCommands);

                _server.WebSocketConnectionIdChanged += (o, args) =>
                {
                    Assert.That(args?.ConnectionId is not null);
                    Assert.That(args?.ConnectionId > 0);
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

                // Subscirbe to state changes
                _server.ToolheadsChanged += (o, args) =>
                {
                    foreach (var pair in args.Toolheads)
                    {
                        Debug.WriteLine($"Extruder{pair.Key}: {pair.Value?.TempRead} �C (Target: {pair.Value?.TempSet} �C)");
                    }
                };
                _server.HeatersChanged += (o, args) =>
                {
                    foreach (var pair in args.Heaters)
                    {
                        Debug.WriteLine($"HeatedBed{pair.Key}: {pair.Value?.TempRead} �C (Target: {pair.Value?.TempSet} �C)");
                    }
                };
                _server.KlipperDisplayStatusChanged += (o, args) =>
                {
                    if (args.NewDisplayStatus != null)
                        Debug.WriteLine($"Progress: {args.NewDisplayStatus?.Progress * 100} % (Msg: {args.NewDisplayStatus?.Message})");
                };

                _server.KlipperToolHeadStateChanged += (o, args) =>
                {
                    //Debug.WriteLine($"Toolhead: {args.ToolheadStates.EstimatedPrintTime}");
                };
                _server.KlipperPrintStateChanged += (o, args) =>
                {
                    //Debug.WriteLine($"PrintState: New => {args.NewPrintState.State}; Previous => {args.PreviousPrintState?.State}");
                };

                _server.WebSocketMessageReceived += (o, args) =>
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

                // Wait a 90 minutes
                CancellationTokenSource cts = new(new TimeSpan(1, 30, 0));
                _server.WebSocketDisconnected += (o, args) =>
                {
                    if (!cts.IsCancellationRequested)
                        Assert.Fail($"Websocket unexpectly closed: {args}");
                };

                do
                {
                    await Task.Delay(10000);
                    await _server.CheckOnlineAsync();
                    Debug.WriteLine($"Online: {_server.IsOnline}");
                    if (_server.IsPrinting)
                    {
                        if (_server.ActiveJob is not null)
                        {

                        }
                    }
                } while (_server.IsOnline && !cts.IsCancellationRequested);
                await _server.StopListeningAsync();

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