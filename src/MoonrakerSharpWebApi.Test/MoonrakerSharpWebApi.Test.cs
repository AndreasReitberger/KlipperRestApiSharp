using AndreasReitberger.API.Moonraker;
using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace MoonrakerSharpWebApi.Test
{
    public class Tests
    {
        private readonly string _host = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").Ip ?? "";
        private readonly int _port = 80;
        private readonly string _api = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").ApiKey ?? "";
        private readonly bool _ssl = false;

        private readonly bool _skipOnlineTests = true;

        [SetUp]
        public void Setup()
        {
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
                MoonrakerClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", SecureStringHelper.ConvertToSecureString("my_awesome_pwd"), true);

                var serializedString = System.Text.Json.JsonSerializer.Serialize(MoonrakerClient.Instance);
                var serializedObject = System.Text.Json.JsonSerializer.Deserialize<MoonrakerClient>(serializedString);
                Assert.IsTrue(serializedObject is MoonrakerClient server && server != null);

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
                MoonrakerClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", SecureStringHelper.ConvertToSecureString("my_awesome_pwd"), true);

                var serializedString = JsonConvert.SerializeObject(MoonrakerClient.Instance, Formatting.Indented);
                var serializedObject = JsonConvert.DeserializeObject<MoonrakerClient>(serializedString);
                Assert.IsTrue(serializedObject is MoonrakerClient server && server != null);

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
                    MoonrakerClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", SecureStringHelper.ConvertToSecureString("my_awesome_pwd"), true);

                    xmlSerializer.Serialize(fileStream, MoonrakerClient.Instance);
                    Assert.IsTrue(File.Exists(Path.Combine(dir, "server.xml")));
                }

                xmlSerializer = new XmlSerializer(typeof(MoonrakerClient));
                using (FileStream fileStream = new(serverConfig, FileMode.Open))
                {
                    MoonrakerClient instance = (MoonrakerClient)xmlSerializer.Deserialize(fileStream);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    //var token = await _server.GetOneshotTokenAsync();
                    KlipperAccessTokenResult token2 = await _server.GetApiKeyAsync();
                    Assert.IsTrue(!string.IsNullOrEmpty(token2.Result));

                    //var emergencyStop = await _server.EmergencyStopPrinterAsync();
                    //var objectList = await _server.GetPrinterObjectListAsync();
                    Dictionary<string, object> objectList = await _server.QueryPrinterObjectStatusAsync(new() { { "gcode_move", "" } });
                    objectList = await _server.QueryPrinterObjectStatusAsync(new() { { "toolhead", "position,status" } });
                    //objectList = await _server.QueryPrinterObjectStatusAsync("toolhead", new string[] { "position", "status" });
                    //RepetierGcodeScript result = await _server.GetPrinterInfoAsync();
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
                        Assert.Fail(args.Message);
                    };

                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    //await _server.RefreshIdleStatusAsync();
                    Assert.IsNotNull(_server.IdleState);

                    //await _server.RefreshDisplayStatusAsync();
                    Assert.IsNotNull(_server.DisplayStatus);

                    //await _server.RefreshToolHeadStatusAsync();
                    Assert.IsNotNull(_server.ToolHead);

                    //await _server.RefreshGcodeMoveStatusAsync();
                    Assert.IsNotNull(_server.GcodeMove);

                    //await _server.RefreshVirtualSdCardStatusAsync();
                    Assert.IsNotNull(_server.VirtualSdCard);

                    //await _server.RefreshHeaterBedStatusAsync();
                    Assert.IsNotNull(_server.HeaterBed);

                    //await _server.RefreshExtruderStatusAsync();
                    Assert.IsNotNull(_server.Extruders);

                    //await _server.RefreshPrintStatusAsync();
                    Assert.IsNotNull(_server.PrintStats);

                    //await _server.RefreshAvailableFilesAsync();
                    Assert.IsNotNull(_server.Files);
                    Assert.IsTrue(_server.Files?.Count > 0);

                    //bool eStop = await _server.EmergencyStopPrinterAsync();
                    //Assert.IsTrue(eStop);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    var webcamConfigs = await _server.GetWebCamSettingsFromDatabaseAsync();
                    Assert.IsTrue(webcamConfigs.Count > 0);

                    webcamConfigs = await _server.GetWebCamSettingsAsync();
                    Assert.IsTrue(webcamConfigs.Count > 0);

                    string webcamUri = await _server.GetWebCamUriAsync(0, false);
                    Assert.IsNotNull(_server.WebCams);
                    Assert.IsTrue(_server.WebCams?.Count > 0);
                    Assert.IsTrue(!string.IsNullOrEmpty(webcamUri));

                    //bool eStop = await _server.EmergencyStopPrinterAsync();
                    //Assert.IsTrue(eStop);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };
                    await _server.StartListeningAsync();

                    var fSensors = await _server.GetFilamentSensorsAsync();
                    Assert.IsNotNull(fSensors);

                    Dictionary<string, string> macros = new();
                    var availableMacros = await _server.GetPrinterObjectListAsync("gcode_macro");
                    for (int i = 0; i < availableMacros.Count; i++)
                    {
                        macros.Add(availableMacros[i], string.Empty);
                    }

                    var gcMacros = await _server.GetGcodeMacrosAsync();
                    Assert.IsNotNull(gcMacros);

                    List<string> objects = await _server.GetPrinterObjectListAsync();
                    Assert.IsNotNull(objects);

                    KlipperEndstopQueryResult endstops = await _server.QueryEndstopsAsync();
                    Assert.IsNotNull(endstops);

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
                    Assert.IsNotNull(availableMarcros);

                    Dictionary<string, object> objectStates = await _server.QueryPrinterObjectStatusAsync(targets);
                    Assert.IsNotNull(objectStates);

                    long? connectionId = _server.WebSocketConnectionId; // ""; // Get from WebSocket?
                    var result = await _server.SubscribePrinterObjectStatusAsync(connectionId, objects);
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
                    Assert.IsTrue(_server.InitialDataFetched);

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
                    Assert.IsNotNull(gcMacros);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };

                    ObservableCollection<IGcode> models = await _server.GetAvailableFilesAsync("gcodes", true);
                    //var childItems = models?.Where(model => model.Path.Contains("/")).ToList();
                    Assert.IsTrue(models?.Any());
                    foreach (KlipperFile gcodeFile in models.Cast<KlipperFile>()?.Where(g => g?.Meta?.GcodeImages?.Count > 0))
                    {
                        byte[] thumbnail = await _server.GetGcodeThumbnailImageAsync(gcodeFile?.Meta);
                        Assert.IsNotNull(thumbnail);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperPrinterStateMessageResult info = await _server.GetPrinterInfoAsync();
                    Assert.IsNotNull(info);

                    //bool eStop = await _server.EmergencyStopPrinterAsync();
                    //Assert.IsTrue(eStop);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    int fanSpeed = 128;
                    string gcode = $"M106 S{fanSpeed}";
                    var result = await _server.RunGcodeScriptAsync(gcode);
                    Assert.IsNotNull(result);

                    fanSpeed = 0;
                    gcode = $"M106 S{fanSpeed}";
                    result = await _server.RunGcodeScriptAsync(gcode);
                    Assert.IsNotNull(result);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    var homed = await _server.HomeAxesAsync(true, true, true);
                    Assert.IsTrue(homed, "Not homed!");

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
                    Assert.IsTrue(moved, "Did not move");
                    Assert.AreEqual(newX, end.Position[0], message: "X didn't move as expected");
                    Assert.AreEqual(newY, end.Position[1], message: "Y didn't move as expected");
                    Assert.AreEqual(newZ, end.Position[2], message: "Z didn't move as expected");

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
                    Assert.AreEqual(newX, end.Position[0], message: "X didn't move as expected");
                    Assert.AreEqual(newY, end.Position[1], message: "Y didn't move as expected");
                    Assert.AreEqual(newZ, end.Position[2], message: "Z didn't move as expected");
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    var fan = await _server.SetFanSpeedTargetAsync(128, false);
                    fan = await _server.SetFanSpeedTargetAsync(0, false);
                    fan = await _server.SetFanSpeedTargetAsync(100, true);
                    fan = await _server.SetFanSpeedTargetAsync(0, false);

                    var extruder = await _server.SetExtruderTargetAsync(50);
                    extruder = await _server.SetExtruderTargetAsync(50, 1);
                    await Task.Delay(5000);
                    extruder = await _server.SetExtruderTargetAsync(0);
                    extruder = await _server.SetExtruderTargetAsync(0, 1);

                    var heaterBed = await _server.SetHeaterBedTargetAsync(50);
                    await Task.Delay(5000);
                    heaterBed = await _server.SetHeaterBedTargetAsync(0);

                    string fileName = "test.gcode";
                    bool printStarted = await _server.PrintFileAsync(fileName);
                    Assert.IsTrue(printStarted);

                    if (printStarted)
                    {
                        // Wait a minute
                        await Task.Delay(60 * 1000);
                        bool paused = await _server.PausePrintAsync();
                        await Task.Delay(5000);
                        Assert.IsTrue(paused);

                        if (paused)
                        {
                            bool resumed = await _server.ResumePrintAsync();
                            await Task.Delay(5000);
                            Assert.IsTrue(resumed);

                            if (resumed)
                            {
                                bool cancelled = await _server.CancelPrintAsync();
                                await Task.Delay(5000);
                                Assert.IsTrue(cancelled);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    //var token = await _server.GetOneshotTokenAsync();
                    KlipperAccessTokenResult token2 = await _server.GetApiKeyAsync();
                    Assert.IsTrue(!string.IsNullOrEmpty(token2.Result));
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperServerConfig config = await _server.GetServerConfigAsync();
                    Assert.IsNotNull(config);

                    Dictionary<string, KlipperTemperatureSensorHistory> tempData = await _server.GetServerCachedTemperatureDataAsync();
                    //Assert.IsNotNull(tempData);
                    Assert.IsTrue(tempData?.Count > 0);

                    List<KlipperGcode> cachedGcodes = await _server.GetServerCachedGcodesAsync();
                    Assert.IsNotNull(cachedGcodes);
                    //Assert.IsTrue(cachedGcodes?.Count > 0);

                    if (!_skipOnlineTests)
                    {
                        bool restart = await _server.RestartServerAsync();
                        Assert.IsTrue(restart);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    Dictionary<string, string> help = await _server.GetGcodeHelpAsync();
                    Assert.IsNotNull(help);
                    if (!_skipOnlineTests)
                    {
                        bool succeed = await _server.RunGcodeScriptAsync("G28");
                        Assert.IsTrue(succeed);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperMachineInfo info = await _server.GetMachineSystemInfoAsync();
                    Assert.IsNotNull(info);

                    bool restarted = await _server.RestartSystemServiceAsync(KlipperServices.webcamd);
                    Assert.IsTrue(restarted);
                    await Task.Delay(10 * 1000);

                    bool stopped = await _server.StopSystemServiceAsync(KlipperServices.webcamd);
                    Assert.IsTrue(stopped);
                    await Task.Delay(10 * 1000);

                    bool started = await _server.StartSystemServiceAsync(KlipperServices.webcamd);
                    Assert.IsTrue(started);
                    await Task.Delay(10 * 1000);

                    bool rebooted = await _server.MachineRebootAsync();
                    Assert.IsTrue(rebooted);
                    await Task.Delay(10 * 1000);

                    bool shutdown = await _server.MachineShutdownAsync();
                    Assert.IsTrue(shutdown);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperMoonrakerProcessStatsResult info = await _server.GetMoonrakerProcessStatsAsync();
                    Assert.IsNotNull(info);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    ObservableCollection<IGcode> files = await _server.GetAvailableFilesAsync();
                    Assert.IsNotNull(files);

                    string fileName = files[0]?.FilePath;
                    KlipperGcodeMetaResult meta = await _server.GetGcodeMetadataAsync(fileName);
                    Assert.IsNotNull(meta);


                    files = await _server.GetAvailableFilesAsync("config");
                    Assert.IsNotNull(files);

                    files = await _server.GetAvailableFilesAsync("config_examples");
                    Assert.IsNotNull(files);

                    files = await _server.GetAvailableFilesAsync("docs ");
                    Assert.IsNotNull(files);

                    string dirName = "gcodes/Kundenaufträge";
                    KlipperDirectoryActionResult created = await _server.CreateDirectoryAsync(dirName);
                    Assert.IsNotNull(created);

                    KlipperDirectoryActionResult copyFile = await _server.CopyDirectoryOrFileAsync($"gcodes/{fileName}", $"{dirName}/{fileName}");
                    Assert.IsNotNull(copyFile);

                    KlipperDirectoryInfoResult dirs = await _server.GetDirectoryInformationAsync(dirName);
                    Assert.IsNotNull(dirs);

                    var deleteFile = await _server.DeleteFileAsync($"{dirName}/{fileName}");
                    Assert.IsNotNull(dirs);

                    KlipperDirectoryActionResult deleted = await _server.DeleteDirectoryAsync(dirName);
                    Assert.IsNotNull(deleted);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    string testFile = @"Files/Groot_0.2mm_PETG_MK3S_11h28m.gcode";
                    //string testFile = @"Files/Test.gcode";
                    Assert.IsTrue(File.Exists(testFile));
                    FileInfo info = new(testFile);
                    string fullPath = info.FullName;

                    byte[] file = null;
                    file = await File.ReadAllBytesAsync(testFile);

                    KlipperFileActionResult msg = await _server.UploadFileAsync(fullPath);
                    Assert.IsNotNull(msg);

                    var meta = await _server.GetGcodeMetadataAsync(msg.Item.Path);
                    Assert.IsNotNull(meta);

                    string thumbnail = meta.GcodeImages.FirstOrDefault()?.Path;
                    // Get small image (30x30)
                    byte[] image = await _server.GetGcodeThumbnailImageAsync(thumbnail);
                    // Get big image (400x300)
                    byte[] image2 = await _server.GetGcodeThumbnailImageAsync(meta, 1);

                    KlipperDirectoryActionResult deleted = await _server.DeleteFileAsync("gcodes", info.Name);
                    Assert.IsNotNull(deleted);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    byte[] logFile = await _server.DownloadLogFileAsync(KlipperLogFileTypes.Klippy);
                    await File.WriteAllBytesAsync("klippy.log", logFile);
                    Assert.IsNotNull(logFile);

                    logFile = await _server.DownloadLogFileAsync(KlipperLogFileTypes.Moonraker);
                    await File.WriteAllBytesAsync("moonraker.log", logFile);
                    Assert.IsNotNull(logFile);

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
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperUserActionResult userCreated = await _server.CreateUserAsync(username, password);
                    Assert.IsNotNull(userCreated);

                    List<KlipperUser> users = await _server.ListAvailableUsersAsync();
                    Assert.IsTrue(users?.Count > 0);

                    KlipperUser currentUser = await _server.GetCurrentUserAsync();
                    if (currentUser != null)
                    {
                        //Assert.IsNotNull(await _server.LogoutCurrentUserAsync());
                    }

                    KlipperUserActionResult login = await _server.LoginUserAsync(username, password);
                    Assert.IsNotNull(login);
                    Assert.IsTrue(login.Username == username);

                    currentUser = await _server.GetCurrentUserAsync();
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    //List<string> namespaces = await _server.ListDatabaseNamespacesAsync();
                    Assert.IsTrue(_server.AvailableNamespaces?.Count > 0);
                    if (_server.OperatingSystem == MoonrakerOperatingSystems.FluiddPi)
                    {
                        _server.ApiKey = _api;
                    }

                    string currentNamespace = _server.OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                    Dictionary<string, object> items = await _server.GetDatabaseItemAsync(currentNamespace);
                    Assert.IsNotNull(items);

                    foreach (KeyValuePair<string, object> pair in items)
                    {
                        var type = pair.Value.GetType();
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
                    Assert.IsNotNull(webcams);

                    /*
                    Dictionary<string, object> remotePrinters = await _server.GetDatabaseItemAsync(currentNamespace, "remote_printers");
                    Assert.IsNotNull(remotePrinters);

                    List<KlipperDatabaseMainsailValueRemotePrinter> remotePrinters2 = await _server.GetRemotePrintersAsync();
                    Assert.IsNotNull(remotePrinters2);
                    */
                    ObservableCollection<IWebCamConfig> webcamConfig = await _server.GetWebCamSettingsAsync();
                    Assert.IsNotNull(webcamConfig);
                    if (webcamConfig.Count > 0)
                        Assert.IsTrue(!string.IsNullOrEmpty(webcamConfig.FirstOrDefault()?.WebCamUrlDynamic?.ToString()));

                    List<KlipperDatabaseTemperaturePreset> presets = await _server.GetDashboardPresetsAsync();
                    Assert.IsNotNull(presets);

                    if (_server.OperatingSystem == MoonrakerOperatingSystems.MainsailOS)
                    {
                        KlipperDatabaseMainsailValueHeightmapSettings heightmap = await _server.GetMeshHeightMapSettingsAsync();
                        Assert.IsNotNull(heightmap);
                    }

                    Dictionary<string, object> add = await _server.AddDatabaseItemAsync(currentNamespace, "testkey", 56);
                    Assert.IsNotNull(add);

                    Dictionary<string, object> delete = await _server.DeleteDatabaseItemAsync(currentNamespace, "testkey");
                    Assert.IsNotNull(delete);

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
                    Assert.IsTrue(_server.InitialDataFetched);

                    //List<string> namespaces = await _server.ListDatabaseNamespacesAsync();
                    Assert.IsTrue(_server.AvailableNamespaces?.Count > 0);

                    string currentNamespace = _server.OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                    List<KlipperDatabaseRemotePrinter> printers = await _server.GetRemotePrintersAsync();
                    Assert.IsNotNull(printers);


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
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperJobQueueResult jobstatus = await _server.GetJobQueueStatusAsync();
                    Assert.IsNotNull(jobstatus);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperJobQueueResult jobstatus = await _server.GetJobQueueStatusAsync();
                    Assert.IsNotNull(jobstatus);

                    ObservableCollection<IGcode> files = await _server.GetAvailableFilesAsync();
                    Assert.IsNotNull(files);

                    string fileName = files[0]?.FilePath;
                    KlipperJobQueueResult queued = await _server.EnqueueJobsAsync(new string[] { fileName });

                    jobstatus = await _server.GetJobQueueStatusAsync();
                    Assert.IsNotNull(jobstatus);

                    KlipperJobQueueResult start = await _server.StartJobQueueAsync();
                    Assert.IsNotNull(start);

                    KlipperJobQueueResult pause = await _server.PauseJobQueueAsync();
                    Assert.IsNotNull(pause);

                    KlipperJobQueueResult removedAll = await _server.RemoveAllJobAsync();
                    Assert.IsNotNull(removedAll);

                    // Once a printer is available, use real ids
                    //string[] ids = new string[] { "00000000000000001", "00000000000323" };
                    //KlipperJobQueueResult removedIDs = await _server.RemoveJobAsync(ids);
                    //Assert.IsNotNull(removedIDs);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    var devices = await _server.GetDeviceListAsync();
                    Assert.IsNotNull(devices);

                    string device = devices[0].Device;
                    var status = await _server.GetDeviceStatusAsync(device);
                    Assert.IsNotNull(status);
                    Assert.IsTrue(status["printer"] == "off");

                    Dictionary<string, string> stateChanged = await _server.SetDeviceStateAsync(device, KlipperDeviceActions.On);
                    Assert.IsNotNull(stateChanged);
                    Assert.IsTrue(stateChanged["printer"] == "on");

                    stateChanged = await _server.SetDeviceStateAsync(device, KlipperDeviceActions.Off);
                    Assert.IsNotNull(stateChanged);
                    Assert.IsTrue(stateChanged["printer"] == "off");
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    var status = await _server.GetUpdateStatusAsync();
                    Assert.IsNotNull(status);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    OctoprintApiVersionResult version = await _server.GetOctoPrintApiVersionInfoAsync();
                    Assert.IsNotNull(version);

                    OctoprintApiServerStatusResult server = await _server.GetOctoPrintApiServerStatusAsync();
                    Assert.IsNotNull(server);

                    //var userInfo = await _server.GetUserInformationAsync();
                    //Assert.IsNotNull(userInfo);

                    OctoprintApiSettingsResult settings = await _server.GetOctoPrintApiSettingsAsync();
                    Assert.IsNotNull(settings);

                    OctoprintApiJobResult jobStatus = await _server.GetOctoPrintApiJobStatusAsync();
                    Assert.IsNotNull(jobStatus);

                    OctoprintApiPrinterStatusResult printerStatus = await _server.GetOctoPrintApiPrinterStatusAsync();
                    Assert.IsNotNull(printerStatus);

                    Dictionary<string, OctoprintApiPrinter> printerProfiles = await _server.GetOctoPrintApiPrinterProfilesAsync();
                    Assert.IsNotNull(printerProfiles);

                    bool gcodeCommand = await _server.SendOctoPrintApiGcodeCommandAsync("G28");
                    Assert.IsTrue(gcodeCommand);
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
                    Assert.IsTrue(_server.InitialDataFetched);

                    List<KlipperJobItem> history = await _server.GetHistoryJobListAsync();
                    Assert.IsNotNull(history);

                    KlipperHistoryJobTotalsResult total = await _server.GetHistoryTotalJobsAsync();
                    Assert.IsNotNull(total);

                    string uid = history[0].JobId;
                    KlipperJobItem job = await _server.GetHistoryJobAsync(uid);
                    Assert.IsNotNull(job);

                    var deletedIds = await _server.DeleteHistoryJobAsync(uid);
                    Assert.IsNotNull(job);
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
                Assert.IsTrue(cts.IsCancellationRequested);
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
                Assert.IsTrue(_server.IsOnline);

                await _server.RefreshAllAsync();

                if (_server.LoginRequired)
                {
                    await _server.LoginUserAsync("TestUser", "TestPassword");
                }
                await _server.StartListeningAsync();

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

                // Subscirbe to state changes
                _server.KlipperExtruderStatesChanged += (o, args) =>
                {
                    foreach (var pair in args.ExtruderStates)
                    {
                        Debug.WriteLine($"Extruder{pair.Key}: {pair.Value?.Temperature} �C (Target: {pair.Value?.Target} �C)");
                    }
                };
                _server.KlipperHeaterBedStateChanged += (o, args) =>
                {
                    Debug.WriteLine($"HeatedBed: {args.NewHeaterBedState.Temperature} �C (Target: {args.NewHeaterBedState.Target} �C)");
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

                Assert.IsTrue(cts.IsCancellationRequested);
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
                Assert.IsTrue(_server.IsOnline);

                await _server.RefreshAllAsync();

                //var test = await _server.GetActiveJobStatusAsync();

                if (_server.LoginRequired)
                {
                    await _server.LoginUserAsync("TestUser", "TestPassword");
                }
                //KlipperAccessTokenResult oneshot = await _server.GetOneshotTokenAsync();
                //_server.OneShotToken = oneshot.Result;
                await _server.StartListeningAsync();

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

                // Subscirbe to state changes
                _server.KlipperExtruderStatesChanged += (o, args) =>
                {
                    foreach (var pair in args.ExtruderStates)
                    {
                        Debug.WriteLine($"Extruder{pair.Key}: {pair.Value?.Temperature} �C (Target: {pair.Value?.Target} �C)");
                    }
                };
                _server.KlipperHeaterBedStateChanged += (o, args) =>
                {
                    Debug.WriteLine($"HeatedBed: {args.NewHeaterBedState?.Temperature} �C (Target: {args.NewHeaterBedState?.Target} �C)");
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
                        //Debug.WriteLine($"WebSocket Data: {args.Message} (Total: {websocketMessages.Count})");
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
                } while (_server.IsOnline && !cts.IsCancellationRequested);
                await _server.StopListeningAsync();

                StringBuilder sb = new();
                foreach (var pair in websocketMessages)
                {
                    sb.AppendLine($"{pair.Key}: {pair.Value}");
                }
                await File.WriteAllTextAsync("ws_messages.txt", sb.ToString());

                Assert.IsTrue(cts.IsCancellationRequested);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
    }
}