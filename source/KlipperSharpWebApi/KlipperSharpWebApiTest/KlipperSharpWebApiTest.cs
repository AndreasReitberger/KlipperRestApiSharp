using AndreasReitberger;
using AndreasReitberger.Models;
using AndreasReitberger.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using AndreasReitberger.Enum;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace RepetierServerSharpApiTest
{
    [TestClass]
    public class KlipperSharpWebApiTest
    {

        private readonly string _host = "192.168.10.113";
        private readonly int _port = 80;
        private readonly string _api = "";
        private readonly bool _ssl = false;

        private readonly bool _skipOnlineTests = true;

        [TestMethod]
        public void SerializeJsonTest()
        {

            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {

                KlipperClient.Instance = new KlipperClient(_host, _api, _port, _ssl)
                {
                    FreeDiskSpace = 1523165212,
                    TotalDiskSpace = 65621361616161,
                };
                KlipperClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", SecureStringHelper.ConvertToSecureString("my_awesome_pwd"), true);

                var serializedString = JsonConvert.SerializeObject(KlipperClient.Instance);
                var serializedObject = JsonConvert.DeserializeObject<KlipperClient>(serializedString);
                Assert.IsTrue(serializedObject is KlipperClient server && server != null);

            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public void SerializeTest()
        {

            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {
                XmlSerializer xmlSerializer = new(typeof(KlipperClient));
                using (var fileStream = new FileStream(serverConfig, FileMode.Create))
                {
                    KlipperClient.Instance = new KlipperClient(_host, _api, _port, _ssl)
                    {
                        UsedDiskSpace = 1523152132,
                        FreeDiskSpace = 1523165212,
                        TotalDiskSpace = 65621361616161,
                    };
                    KlipperClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", SecureStringHelper.ConvertToSecureString("my_awesome_pwd"), true);

                    xmlSerializer.Serialize(fileStream, KlipperClient.Instance);
                    Assert.IsTrue(File.Exists(Path.Combine(dir, "server.xml")));
                }

                xmlSerializer = new XmlSerializer(typeof(KlipperClient));
                using (FileStream fileStream = new(serverConfig, FileMode.Open))
                {
                    KlipperClient instance = (KlipperClient)xmlSerializer.Deserialize(fileStream);
                }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public void ExtendedSerializeTest()
        {
            try
            {
                // Check if all works
                _ = new KlipperServerConfig()
                {

                }.ToString();
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public async Task ServerInitTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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

        [TestMethod]
        public async Task RefreshTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };

                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    await _server.RefreshAvailableFilesAsync();
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

        #region Printer Tests
        [TestMethod]
        public async Task PrinterStatusTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    _server.RestJsonConvertError += (o, args) =>
                    {
                        Assert.Fail(args.Message);
                    };
                    _server.StartListening();

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

        [TestMethod]
        public async Task GcodeMacroTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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
        
        [TestMethod]
        public async Task PrinterInfoTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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

        [TestMethod]
        public async Task SendGcodeCommandTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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

        [TestMethod]
        public async Task MovePrinterTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    bool homed = await _server.HomeAxesAsync(true, true, true);
                    Assert.IsTrue(homed);

                    bool moved = await _server.MoveAxesAsync(6000, 100);
                    Assert.IsTrue(moved);
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
        [TestMethod]
        public async Task PrinterManagementTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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
        [TestMethod]
        public async Task ServerApiTest()
        {
            try
            {
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
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public async Task ServerConfigTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperServerConfig config = await _server.GetServerConfigAsync();
                    Assert.IsNotNull(config);

                    KlipperServerTempData tempData = await _server.GetServerCachedTemperatureDataAsync();
                    Assert.IsNotNull(tempData);

                    List<KlipperGcode> cachedGcodes = await _server.GetServerCachedGcodesAsync();
                    Assert.IsNotNull(cachedGcodes);
                    //Assert.IsTrue(cachedGcodes?.Count > 0);

                    bool restart = await _server.RestartServerAsync();
                    Assert.IsTrue(restart);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public async Task ServerGcodeApiTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    Dictionary<string, string> help = await _server.GetGcodeHelpAsync();
                    Assert.IsNotNull(help);

                    bool succeed = await _server.RunGcodeScriptAsync("G28");
                    Assert.IsTrue(succeed);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public async Task ServerMachineCommandTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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

        [TestMethod]
        public async Task ServerMachineMoonrakerTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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

        [TestMethod]
        public async Task ServerFilesTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    ObservableCollection<KlipperFile> files = await _server.GetAvailableFilesAsync();
                    Assert.IsNotNull(files);

                    string fileName = files[0]?.Path;
                    KlipperGcodeMetaResult meta = await _server.GetGcodeMetadataAsync(fileName);
                    Assert.IsNotNull(meta);


                    files = await _server.GetAvailableFilesAsync("config");
                    Assert.IsNotNull(files);

                    files = await _server.GetAvailableFilesAsync("config_examples");
                    Assert.IsNotNull(files);

                    files = await _server.GetAvailableFilesAsync("docs ");
                    Assert.IsNotNull(files);

                    string dirName = "gcodes/test2";
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

        [TestMethod]
        public async Task UploadFileTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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

                    string thumbnail = meta.Thumbnails.FirstOrDefault()?.RelativePath;
                    // Get small image (30x30)
                    byte[] image = _server.GetGcodeThumbnailImage(thumbnail);
                    // Get big image (400x400)
                    byte[] image2 = _server.GetGcodeThumbnailImage(meta, 1);

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

        [TestMethod]
        public async Task DownloadLogFileTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    byte[] logFile = _server.DownloadLogFile(KlipperLogFileTypes.Klippy);
                    await File.WriteAllBytesAsync("klippy.log", logFile);
                    Assert.IsNotNull(logFile);

                    logFile = _server.DownloadLogFile(KlipperLogFileTypes.Moonraker);
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
        [TestMethod]
        public async Task AuthTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

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
        [TestMethod]
        public async Task DatabaseTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    List<string> namespaces = await _server.ListDatabaseNamespacesAsync();
                    Assert.IsNotNull(namespaces);

                    Dictionary<string, object> items = await _server.GetDatabaseItemAsync("mainsail");
                    Assert.IsNotNull(items);

                    Dictionary<string, object> webcams = await _server.GetDatabaseItemAsync("mainsail", "webcam");
                    Assert.IsNotNull(webcams);

                    KlipperDatabaseMainsailValueWebcam webcamConfig = JsonConvert.DeserializeObject<KlipperDatabaseMainsailValueWebcam>(webcams.FirstOrDefault().Value.ToString());
                    Assert.IsNotNull(webcamConfig);

                    webcamConfig = await _server.GetWebCamSettingsAsync();
                    Assert.IsNotNull(webcamConfig);

                    List<KlipperDatabaseMainsailValuePreset> presets = await _server.GetDashboardPresetsAsync();
                    Assert.IsNotNull(presets);

                    var heightmap = await _server.GetMeshHeightMapAsync();
                    Assert.IsNotNull(heightmap);

                    Dictionary<string, object> add = await _server.AddDatabaseItemAsync("mainsail", "testkey", 56);
                    Assert.IsNotNull(add);

                    Dictionary<string, object> delete = await _server.DeleteDatabaseItemAsync("mainsail", "testkey");
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
        #endregion

        #region Job Queue
        [TestMethod]
        public async Task JobQueueTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperJobQueueResult jobstatus = await _server.GetJobQueueStatusAsync();
                    Assert.IsNotNull(jobstatus);

                    ObservableCollection<KlipperFile> files = await _server.GetAvailableFilesAsync();
                    Assert.IsNotNull(files);

                    string fileName = files[0]?.Path;
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
        [TestMethod]
        public async Task PowerApisTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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
        [TestMethod]
        public async Task UpdateManagerApiTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
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
        [TestMethod]
        public async Task OctoprintApiTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    OctoprintApiVersionResult version = await _server.GetVersionInfoAsync();
                    Assert.IsNotNull(version);

                    OctoprintApiServerStatusResult server = await _server.GetServerStatusAsync();
                    Assert.IsNotNull(server);

                    //var userInfo = await _server.GetUserInformationAsync();
                    //Assert.IsNotNull(userInfo);

                    OctoprintApiSettingsResult settings = await _server.GetSettingsAsync();
                    Assert.IsNotNull(settings);

                    OctoprintApiJobResult jobStatus = await _server.GetJobStatusAsync();
                    Assert.IsNotNull(jobStatus);

                    OctoprintApiPrinterStatusResult printerStatus = await _server.GetPrinterStatusAsync();
                    Assert.IsNotNull(printerStatus);

                    Dictionary<string, OctoprintApiPrinter> printerProfiles = await _server.GetPrinterProfilesAsync();
                    Assert.IsNotNull(printerProfiles);

                    bool gcodeCommand = await _server.SendGcodeCommandAsync("G28");
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
        [TestMethod]
        public async Task HistoryApiTest()
        {
            try
            {
                KlipperClient _server = new(_host, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);

                    KlipperHistoryResult history = await _server.GetHistoryJobListAsync();
                    Assert.IsNotNull(history);

                    KlipperHistoryJobTotalsResult total = await _server.GetHistoryTotalJobsAsync();
                    Assert.IsNotNull(total);

                    string uid = history.Jobs[0].JobId;
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

        /*
        [TestMethod]
        public async Task DownloadPrintReport()
        {
            try
            {
                OpenScanClient _server = new(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    await _server.SetPrinterActiveAsync(1);
                    var report = RepetierServerPro.Instance.GetHistoryReport(522);
                    Assert.IsTrue(report.Length > 0);
                    string downloadTarget = @"C:\VS\RepetierServerSharpApi\Source\RepetierServerSharpApi\TestResults\report.pdf";
                    await File.WriteAllBytesAsync(downloadTarget, report);
                    Assert.IsTrue(File.Exists(downloadTarget));
                    //Process.Start(downloadTarget);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        */
        [TestMethod]
        public async Task OnlineTest()
        {
            if (_skipOnlineTests) return;
            try
            {
                KlipperClient _server = new(_host, _api, _port, _ssl);
                _server.Error += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                _server.ServerWentOffline += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                // Wait 10 minutes
                CancellationTokenSource cts = new(new TimeSpan(0, 10, 0));
                do
                {
                    await Task.Delay(10000);
                    await _server.CheckOnlineAsync();
                    await _server.RefreshAllAsync();
                } while (_server.IsOnline && !cts.IsCancellationRequested);
                Assert.IsTrue(cts.IsCancellationRequested);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [TestMethod]
        public async Task WebsocketTest()
        {
            try
            {
                Dictionary<DateTime, string> websocketMessages = new();
                KlipperClient _server = new(_host, _api, _port, _ssl);

                await _server.CheckOnlineAsync();
                Assert.IsTrue(_server.IsOnline);

                var test = await _server.GetActiveJobStatusAsync();

                _server.StartListening();

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
                        Debug.WriteLine($"Extruder{pair.Key}: {pair.Value?.Temperature} °C (Target: {pair.Value?.Target} °C)");
                    }
                };
                _server.KlipperHeaterBedStateChanged += (o, args) =>
                {
                    Debug.WriteLine($"HeatedBed: {args.HeaterBedState.Temperature} °C (Target: {args.HeaterBedState.Target} °C)");
                };
                _server.KlipperDisplayStatusChanged += (o, args) =>
                {
                    if(args.NewDisplayStatus != null)
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

                _server.WebSocketDataReceived += (o, args) =>
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
                CancellationTokenSource cts = new(new TimeSpan(0, 15, 0));
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

                StringBuilder sb = new();
                foreach(var pair in websocketMessages)
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
