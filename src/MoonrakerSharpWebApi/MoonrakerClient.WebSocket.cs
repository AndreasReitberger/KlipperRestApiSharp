using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Moonraker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security;
using System.Xml.Serialization;
using System.Threading;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;
using System.Text.RegularExpressions;
using System.Security.Authentication;
using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Core.Enums;
using System.Threading.Tasks;
using WebSocket4Net;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;
using System.Collections.Concurrent;
using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region WebSocket

        #region Properties

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
#if WebSocket4Net
        WebSocket webSocket;
#else
        WebSocket webSocket;
#endif

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        long? webSocketConnectionId;
        partial void OnWebSocketConnectionIdChanged(long? value)
        {
            OnWebSocketConnectionIdChanged(new KlipperWebSocketConnectionChangedEventArgs()
            {
                ConnectionId = value,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        Timer pingTimer;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        int pingCounter = 0;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        int refreshCounter = 0;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        bool isListeningToWebsocket = false;
        partial void OnIsListeningToWebsocketChanged(bool value)
        {
            OnListeningChanged(new KlipperEventListeningChangedEventArgs()
            {
                SessonId = SessionId,
                IsListening = IsListening,
                IsListeningToWebSocket = value,
            });
        }
        #endregion

        #region Methods

#if WebSocket4Net
#endif
        [Obsolete("Use ConnectWebSocketAsync instead")]
        public void ConnectWebSocket()
        {
            try
            {
                //if (!IsReady) return;
                if (!string.IsNullOrEmpty(FullWebAddress) && (
                    Regex.IsMatch(FullWebAddress, RegexHelper.IPv4AddressRegex) ||
                    Regex.IsMatch(FullWebAddress, RegexHelper.IPv6AddressRegex) ||
                    Regex.IsMatch(FullWebAddress, RegexHelper.Fqdn)))
                {
                    return;
                }
                //if (!IsReady || IsListeningToWebsocket) return;

                DisconnectWebSocket();
                // https://github.com/Arksine/moonraker/blob/master/docs/web_api.md#appendix
                // ws://host:port/websocket?token={32 character base32 string}
                //string target = $"ws://192.168.10.113:80/websocket?token={API}";
                //string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(API) ? $"?token={(LoginRequired ? UserToken : API)}" : "")}";
                //var token = GetOneshotTokenAsync();

                // If logged in, even if passing an api key, the websocket returns 401?!
                // The OneShotToken seems to work in both cases
                string target = LoginRequired ?
                        $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket?token={OneShotToken}" :
                        //$"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(API) ? $"?token={API}" : "")}";
                        $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(OneShotToken) ? $"?token={OneShotToken}" : $"?token={ApiKey}")}";
                WebSocket = new WebSocket(target)
                {
                    EnableAutoSendPing = false,

                };
                if (LoginRequired)
                {
                    //WebSocket.Security.Credential = new NetworkCredential(Username, Password);
                }

                if (IsSecure)
                {
                    // https://github.com/sta/websocket-sharp/issues/219#issuecomment-453535816
                    SslProtocols sslProtocolHack = (SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
                    //Avoid TlsHandshakeFailure
                    if (WebSocket.Security.EnabledSslProtocols != sslProtocolHack)
                    {
                        WebSocket.Security.EnabledSslProtocols = sslProtocolHack;
                    }
                }

                WebSocket.MessageReceived += WebSocket_MessageReceived;
                //WebSocket.DataReceived += WebSocket_DataReceived;
                WebSocket.Opened += WebSocket_Opened;
                WebSocket.Closed += WebSocket_Closed;
                WebSocket.Error += WebSocket_Error;

#if NETSTANDARD || NET6_0_OR_GREATER
                WebSocket.OpenAsync();
#else
                WebSocket.Open();
#endif

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        [Obsolete("Use DisconnectWebSocketAsync instead")]
        public void DisconnectWebSocket()
        {
            try
            {
                if (WebSocket != null)
                {
                    if (WebSocket.State == WebSocketState.Open)
#if NETSTANDARD || NET6_0_OR_GREATER
                        WebSocket.CloseAsync();
#else
                        WebSocket.Close();
#endif
                    StopPingTimer();

                    WebSocket.MessageReceived -= WebSocket_MessageReceived;
                    //WebSocket.DataReceived -= WebSocket_DataReceived;
                    WebSocket.Opened -= WebSocket_Opened;
                    WebSocket.Closed -= WebSocket_Closed;
                    WebSocket.Error -= WebSocket_Error;

                    WebSocket = null;
                }
                //WebSocket = null;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        public async Task ConnectWebSocketAsync()
        {
            try
            {
                //if (!IsReady) return;
                if (!string.IsNullOrEmpty(FullWebAddress) && (
                    Regex.IsMatch(FullWebAddress, RegexHelper.IPv4AddressRegex) ||
                    Regex.IsMatch(FullWebAddress, RegexHelper.IPv6AddressRegex) ||
                    Regex.IsMatch(FullWebAddress, RegexHelper.Fqdn)))
                {
                    return;
                }
                //if (!IsReady || IsListeningToWebsocket) return;

                await DisconnectWebSocketAsync();
                // https://github.com/Arksine/moonraker/blob/master/docs/web_api.md#appendix
                // ws://host:port/websocket?token={32 character base32 string}
                //string target = $"ws://192.168.10.113:80/websocket?token={API}";
                //string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(API) ? $"?token={(LoginRequired ? UserToken : API)}" : "")}";

                KlipperAccessTokenResult oneshotToken = await GetOneshotTokenAsync();
                OneShotToken = oneshotToken?.Result;

                string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket?token={OneShotToken}";
                WebSocket = new WebSocket(target)
                {
                    EnableAutoSendPing = false,

                };

                if (IsSecure)
                {
                    // https://github.com/sta/websocket-sharp/issues/219#issuecomment-453535816
                    SslProtocols sslProtocolHack = (SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
                    //Avoid TlsHandshakeFailure
                    if (WebSocket.Security.EnabledSslProtocols != sslProtocolHack)
                    {
                        WebSocket.Security.EnabledSslProtocols = sslProtocolHack;
                    }
                }

                WebSocket.MessageReceived += WebSocket_MessageReceived;
                WebSocket.Opened += WebSocket_Opened;
                WebSocket.Closed += WebSocket_Closed;
                WebSocket.Error += WebSocket_Error;

                await WebSocket.OpenAsync();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public async Task DisconnectWebSocketAsync()
        {
            try
            {
                if (WebSocket != null)
                {
                    if (WebSocket.State == WebSocketState.Open)
                        await WebSocket.CloseAsync();
                    StopPingTimer();

                    if (WebSocket != null)
                    {
                        WebSocket.MessageReceived -= WebSocket_MessageReceived;
                        WebSocket.Opened -= WebSocket_Opened;
                        WebSocket.Closed -= WebSocket_Closed;
                        WebSocket.Error -= WebSocket_Error;
                        WebSocket = null;
                    }
                }
                //WebSocket = null;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void WebSocket_Error(object sender, ErrorEventArgs e)
        {
            try
            {
                IsListeningToWebsocket = false;
                WebSocketConnectionId = -1;
                OnWebSocketError(e);
                OnError(e);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void WebSocket_Closed(object sender, EventArgs e)
        {
            try
            {
                IsListeningToWebsocket = false;
                WebSocketConnectionId = -1;
                StopPingTimer();
                OnWebSocketDisconnected(new KlipperEventArgs()
                {
                    Message = $"WebSocket connection to {WebSocket} closed. Connection state while closing was '{(IsOnline ? "online" : "offline")}'",
                });
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void WebSocket_Opened(object sender, EventArgs e)
        {
            try
            {
                // Get ready state from klipper
                string infoCommand = $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.info\",\"params\":{{}},\"id\":1}}";
                WebSocket?.Send(infoCommand);

                // Get the websocket Id of the current connection
                string connectionId = $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.websocket.id\",\"params\":{{}},\"id\":2}}";
                WebSocket?.Send(connectionId);

                // No ping needed to keep connection alive
                //PingTimer = new Timer((action) => PingServer(), null, 0, 2500);

                IsListeningToWebsocket = true;
                OnWebSocketConnected(new KlipperEventArgs()
                {
                    Message = $"WebSocket connection to {WebSocket} established. Connection state while opening was '{(IsOnline ? "online" : "offline")}'",
                });
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void WebSocket_DataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                OnWebSocketDataReceived(new KlipperWebSocketDataEventArgs()
                {
                    CallbackId = PingCounter,
                    Data = e.Data,
                    SessonId = SessionId,
                });
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                if (e.Message == null || string.IsNullOrEmpty(e.Message))
                {
                    return;
                }
                if (e.Message.ToLower().Contains("method"))
                {
                    string name = string.Empty;
                    string jsonBody = string.Empty;
                    try
                    {
#if ConcurrentDictionary
                        ConcurrentDictionary<int, KlipperStatusExtruder> extruderStats = new();
#else
                        Dictionary<int, KlipperStatusExtruder> extruderStats = new();
#endif
                        KlipperWebSocketMessage method = JsonConvert.DeserializeObject<KlipperWebSocketMessage>(e.Message);
                        for (int i = 0; i < method?.Params?.Count; i++)
                        {
                            if (method.Params[i] is not JObject jsonObject)
                            {
                                continue;
                            }
                            // Parse each property individually
                            foreach (JProperty property in jsonObject.Children<JProperty>())
                            {
                                name = property.Name;
                                jsonBody = property.Value.ToString();
                                switch (name)
                                {
                                    case "klippy_state":
                                        KlipperState = jsonBody;
                                        break;
                                    case "probe":
                                        KlipperStatusProbe probe =
                                            JsonConvert.DeserializeObject<KlipperStatusProbe>(jsonBody);
                                        break;
                                    case "virtual_sdcard":
                                        if (!jsonBody.Contains("progress"))
                                        {
                                            //break;
                                        }
                                        KlipperStatusVirtualSdcard virtualSdcardState =
                                            JsonConvert.DeserializeObject<KlipperStatusVirtualSdcard>(jsonBody);
                                        VirtualSdCard = virtualSdcardState;
                                        break;
                                    case "display_status":
                                        if (!jsonBody.Contains("progress"))
                                        {
                                            break;
                                        }
                                        KlipperStatusDisplay displayState =
                                            JsonConvert.DeserializeObject<KlipperStatusDisplay>(jsonBody);
                                        DisplayStatus = displayState;
                                        break;
                                    case "moonraker_stats":
                                        MoonrakerStatInfo notifyProcState =
                                            JsonConvert.DeserializeObject<MoonrakerStatInfo>(jsonBody);
                                        break;
                                    case "mcu":
                                        KlipperStatusMcu mcuState =
                                            JsonConvert.DeserializeObject<KlipperStatusMcu>(jsonBody);
                                        break;
                                    case "system_stats":
                                        KlipperStatusSystemStats systemState =
                                            JsonConvert.DeserializeObject<KlipperStatusSystemStats>(jsonBody);
                                        break;
                                    case "registered_directories":
                                        RegisteredDirectories =
                                            JsonConvert.DeserializeObject<List<string>>(jsonBody);
                                        break;
                                    case "cpu_temp":
                                        CpuTemp =
                                            JsonConvert.DeserializeObject<double>(jsonBody.Replace(",", "."));
                                        break;
                                    case "system_cpu_usage":
                                        Dictionary<string, double?> tempUsageObject = JsonConvert.DeserializeObject<Dictionary<string, double?>>(jsonBody);
                                        if (tempUsageObject != null)
                                        {
                                            foreach (KeyValuePair<string, double?> cpuUsageItem in tempUsageObject)
                                            {
                                                string cpuUsageIdentifier = cpuUsageItem.Key;
                                                if (CpuUsage.ContainsKey(cpuUsageIdentifier))
                                                {
                                                    CpuUsage[cpuUsageIdentifier] = cpuUsageItem.Value;
                                                }
                                                else
                                                {
                                                    CpuUsage.TryAdd(cpuUsageIdentifier, cpuUsageItem.Value);
                                                }
                                            }
                                        }
                                        break;
                                    case "system_memory":
                                        Dictionary<string, long?> tempMemoryObject = JsonConvert.DeserializeObject<Dictionary<string, long?>>(jsonBody);
                                        if (tempMemoryObject != null)
                                        {
                                            foreach (KeyValuePair<string, long?> memoryUsage in tempMemoryObject)
                                            {
                                                string memoryIdentifier = memoryUsage.Key;
                                                if (SystemMemory.ContainsKey(memoryIdentifier))
                                                {
                                                    SystemMemory[memoryIdentifier] = memoryUsage.Value;
                                                }
                                                else
                                                {
                                                    SystemMemory.TryAdd(memoryIdentifier, memoryUsage.Value);
                                                }
                                            }
                                        }
                                        break;
                                    case "moonraker_version":
                                        MoonrakerVersion = jsonBody;
                                        break;
                                    case "websocket_connections":
                                        int wsConnections = JsonConvert.DeserializeObject<int>(jsonBody);
                                        break;
                                    case "network":
                                        Dictionary<string, KlipperNetworkInterface> network =
                                            JsonConvert.DeserializeObject<Dictionary<string, KlipperNetworkInterface>>(jsonBody);
                                        break;
                                    case "gcode_move":
                                        KlipperStatusGcodeMove gcodeMoveState = JsonConvert.DeserializeObject<KlipperStatusGcodeMove>(jsonBody);
                                        GcodeMove = gcodeMoveState;
                                        break;
                                    case "print_stats":
                                        KlipperStatusPrintStats printStats =
                                            JsonConvert.DeserializeObject<KlipperStatusPrintStats>(jsonBody);
                                        printStats.ValidPrintState = jsonBody.Contains("state");
                                        if (PrintStats != null)
                                        {
                                            // This property is only sent once if changed, so store it
                                            if (!jsonBody.Contains("filename"))
                                            {
                                                printStats.Filename = PrintStats.Filename;
                                            }
                                        }
                                        PrintStats = printStats;
                                        break;
                                    case "fan":
                                        KlipperStatusFan fanState =
                                            JsonConvert.DeserializeObject<KlipperStatusFan>(jsonBody);
                                        Fan = fanState;
                                        break;
                                    case "toolhead":
                                        KlipperStatusToolhead toolhead =
                                            JsonConvert.DeserializeObject<KlipperStatusToolhead>(jsonBody);
                                        ToolHead = toolhead;
                                        break;
                                    case "heater_bed":
                                        // In the status report the temp is missing, so do not parse the heater then.
                                        if (!jsonBody.Contains("temperature") || RefreshHeatersDirectly) break;
                                        KlipperStatusHeaterBed heaterBed =
                                            JsonConvert.DeserializeObject<KlipperStatusHeaterBed>(jsonBody);
                                        if (HeaterBed != null)
                                        {
                                            // This property is only sent once if changed, so store it
                                            if (!jsonBody.Contains("target"))
                                            {
                                                heaterBed.Target = HeaterBed.Target;
                                            }
                                            else
                                            {

                                            }
                                        }
                                        HeaterBed = heaterBed;
                                        break;
                                    case "extruder":
                                    case "extruder1":
                                    case "extruder2":
                                    case "extruder3":
                                        // In the status report the temp is missing, so do not parse the heater then.
                                        if (!jsonBody.Contains("temperature") || RefreshHeatersDirectly) break;
                                        int index = 0;
                                        string extruderIndex = name.Replace("extruder", string.Empty);
                                        if (extruderIndex.Length > 0)
                                        {
                                            int.TryParse(extruderIndex, out index);
                                        }
                                        KlipperStatusExtruder extruder =
                                            JsonConvert.DeserializeObject<KlipperStatusExtruder>(jsonBody);
                                        if (Extruders.ContainsKey(index))
                                        {
                                            // This property is only sent once if changed, so store it
                                            KlipperStatusExtruder previousExtruderState = Extruders[index];
                                            if (!jsonBody.Contains("target"))
                                            {
                                                extruder.Target = previousExtruderState.Target;
                                            }
                                        }
                                        extruderStats.TryAdd(index, extruder);
                                        break;
                                    case "motion_report":
                                        KlipperStatusMotionReport motionReport =
                                            JsonConvert.DeserializeObject<KlipperStatusMotionReport>(jsonBody);
                                        MotionReport = motionReport;
                                        break;
                                    case "idle_timeout":
                                        KlipperStatusIdleTimeout idleTimeout =
                                            JsonConvert.DeserializeObject<KlipperStatusIdleTimeout>(jsonBody);
                                        idleTimeout.ValidState = jsonBody.Contains("state");
                                        IdleState = idleTimeout;
                                        break;
                                    case "filament_switch_sensor fsensor":
                                        KlipperStatusFilamentSensor fSensor =
                                            JsonConvert.DeserializeObject<KlipperStatusFilamentSensor>(jsonBody);
                                        FilamentSensor = fSensor;
                                        break;
                                    case "pause_resume":
                                        KlipperStatusPauseResume pauseResume =
                                            JsonConvert.DeserializeObject<KlipperStatusPauseResume>(jsonBody);
                                        IsPaused = pauseResume.IsPaused;
                                        break;
                                    case "action":
                                        string action = jsonBody;
                                        break;
                                    case "bed_mesh":
                                        KlipperStatusMesh mesh =
                                            JsonConvert.DeserializeObject<KlipperStatusMesh>(jsonBody);
                                        break;
                                    case "job":
                                        if (!jsonBody.Contains("filename")) break;
                                        KlipperStatusJob job =
                                            JsonConvert.DeserializeObject<KlipperStatusJob>(jsonBody);
                                        //ActiveJobName = job?.Filename;
                                        JobStatus = job;
                                        if (JobStatus?.Status == KlipperJobStates.Completed)
                                        {
                                            OnJobFinished(new()
                                            {
                                                Job = job,
                                            });
                                        }
                                        break;
                                    case "updated_queue":
                                        if (string.IsNullOrEmpty(jsonBody))
                                        {
                                            JobList = new();
                                            break;
                                        }
                                        List<KlipperJobQueueItem> queueUpdate =
                                            JsonConvert.DeserializeObject<List<KlipperJobQueueItem>>(jsonBody);
                                        JobList = new(queueUpdate);
                                        break;
                                    case "queue_state":
                                        string state = jsonBody;
                                        JobListState = state;
                                        break;
                                    case "remote_printers":
                                        string remotePrinter = jsonBody;
                                        //JobListState = state;
                                        break;

                                    // Not relevant so far
                                    //case "temperature_host raspberry_pi":
                                    case "item":
#if DEBUG
                                        Console.WriteLine($"Ignored Json object: '{name}' => '{jsonBody}");
#endif
                                        break;
                                    default:
                                        bool nameFound = false;
                                        try
                                        {
                                            if (name.StartsWith("heater_fan"))
                                            {
                                                string[] fan = name.Split(' ');
                                                if (fan.Length >= 2)
                                                {
#if NETSTANDARD || NET6_0_OR_GREATER
                                                    string fanName = fan[^1];
#else
                                                    string fanName = fan[fan.Length - 1];
#endif
                                                    KlipperStatusFan fanObject = JsonConvert.DeserializeObject<KlipperStatusFan>(jsonBody);
                                                    if (fanObject != null)
                                                    {
                                                        if (Fans.ContainsKey(fanName))
                                                        {
                                                            Fans[fanName] = fanObject;
                                                        }
                                                        else
                                                        {
                                                            Fans.TryAdd(fanName, fanObject);
                                                        }
                                                    }
                                                    nameFound = true;
                                                }
                                            }
                                            else if (name.StartsWith("temperature_sensor") || name.StartsWith("temperature_host"))
                                            {
                                                string[] sensor = name.Split(' ');
                                                if (sensor.Length >= 2)
                                                {
#if NETSTANDARD || NET6_0_OR_GREATER
                                                    string sensorName = sensor[^1];
#else
                                                    string sensorName = sensor[sensor.Length - 1];
#endif
                                                    KlipperStatusTemperatureSensor tempObject = JsonConvert.DeserializeObject<KlipperStatusTemperatureSensor>(jsonBody);
                                                    if (tempObject != null)
                                                    {
                                                        if (TemperatureSensors.ContainsKey(sensorName))
                                                        {
                                                            TemperatureSensors[sensorName] = tempObject;
                                                        }
                                                        else
                                                        {
                                                            TemperatureSensors.TryAdd(sensorName, tempObject);
                                                        }
                                                        nameFound = true;
                                                    }
                                                }
                                            }
                                            else if (name.StartsWith("tmc2130"))
                                            {
                                                string[] driver = name.Split(' ');
                                                if (driver.Length >= 2)
                                                {
#if NETSTANDARD || NET6_0_OR_GREATER
                                                    string driverName = driver[^1];
#else
                                                    string driverName = driver[driver.Length - 1];
#endif
                                                    KlipperStatusDriverRespone drvObject = JsonConvert.DeserializeObject<KlipperStatusDriverRespone>(jsonBody);
                                                    if (drvObject != null)
                                                    {
                                                        if (Drivers.ContainsKey(driverName))
                                                        {
                                                            Drivers[driverName] = drvObject.DrvStatus;
                                                        }
                                                        else
                                                        {
                                                            Drivers.TryAdd(driverName, drvObject.DrvStatus);
                                                        }
                                                        nameFound = true;
                                                    }
                                                }
                                            }
                                        }
                                        catch (JsonException jecx)
                                        {
                                            OnError(new JsonConvertEventArgs()
                                            {
                                                Exception = jecx,
                                                OriginalString = jsonBody,
                                                Message = jecx.Message,
                                            });
                                        }
                                        catch (Exception exc)
                                        {
                                            OnError(new UnhandledExceptionEventArgs(exc, false));
                                        }
                                        if (nameFound)
                                        {
                                            break;
                                        }
#if DEBUG
                                        Console.WriteLine($"No Json object found for '{name}' => '{jsonBody}");
#endif
#if ConcurrentDictionary
                                        ConcurrentDictionary<string, string> loggedResults = new(IgnoredJsonResults);
#else
                                        Dictionary<string, string> loggedResults = new(IgnoredJsonResults);
#endif
                                        if (!loggedResults.ContainsKey(name))
                                        {
                                            // Log unused json results for further releases
                                            loggedResults.TryAdd(name, jsonBody);
                                            IgnoredJsonResults = loggedResults;
                                        }
                                        break;
                                }
                            }
                        }

                        // Update extruder states if changed
                        if (extruderStats.Count > 0)
                        {
                            Extruders = extruderStats;
                        }
                    }
                    catch (JsonException jecx)
                    {
                        OnError(new JsonConvertEventArgs()
                        {
                            Exception = jecx,
                            OriginalString = jsonBody,
                            TargetType = name,
                            Message = jecx.Message,
                        });
                    }
                    catch (Exception exc)
                    {
                        OnError(new UnhandledExceptionEventArgs(exc, false));
                    }
                }
                else if (e.Message.ToLower().Contains("error"))
                {
                    //Session = JsonConvert.DeserializeObject<EventSession>(e.Message);
                }
                else if (e.Message.ToLower().Contains("result"))
                {
                    try
                    {
                        KlipperWebSocketResult result = JsonConvert.DeserializeObject<KlipperWebSocketResult>(e.Message);
                        //var type = result?.Result?.GetType();
                        if (result?.Result is JObject jsonObject)
                        {
                            foreach (JProperty property in jsonObject.Children<JProperty>())
                            {
                                string name = property.Name;
                                string jsonBody = property.Value.ToString();
                                switch (name)
                                {
                                    case "websocket_id":
                                        long wsId =
                                            JsonConvert.DeserializeObject<long>(jsonBody);
                                        WebSocketConnectionId = wsId;
                                        break;
                                    case "klippy_connected":
                                        bool klippyConnected =
                                            JsonConvert.DeserializeObject<bool>(jsonBody.ToLower());
                                        break;
                                    case "registered_directories":
                                        RegisteredDirectories =
                                            JsonConvert.DeserializeObject<List<string>>(jsonBody);
                                        break;
                                    case "cpu_temp":
                                        CpuTemp =
                                            JsonConvert.DeserializeObject<double>(jsonBody.Replace(",", "."));
                                        break;
                                    case "moonraker_version":
                                        MoonrakerVersion = jsonBody;
                                        break;
                                    case "klippy_state":
                                        KlipperState = jsonBody;
                                        break;
                                    // Not relevant so far
                                    case "components":
                                    case "failed_components":
                                    //case "registered_directories":
                                    case "warnings":
                                    case "websocket_count":
                                        //case "moonraker_version":
#if DEBUG
                                        Console.WriteLine($"Ignored Json object: '{name}' => '{jsonBody}");
#endif
                                        break;
                                    default:
#if DEBUG
                                        Console.WriteLine($"No Json object found for '{name}' => '{jsonBody}");
#endif
#if ConcurrentDictionary
                                        ConcurrentDictionary<string, string> loggedResults = new(IgnoredJsonResults);
#else
                                        Dictionary<string, string> loggedResults = new(IgnoredJsonResults);
#endif
                                        if (!loggedResults.ContainsKey(name))
                                        {
                                            // Log unused json results for further releases
                                            loggedResults.TryAdd(name, jsonBody);
                                            IgnoredJsonResults = loggedResults;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    catch (JsonException jecx)
                    {
                        OnError(new JsonConvertEventArgs()
                        {
                            Exception = jecx,
                            OriginalString = e.Message,
                            Message = jecx.Message,
                        });
                    }
                    catch (Exception exc)
                    {
                        OnError(new UnhandledExceptionEventArgs(exc, false));
                    }
                }
                else
                {

                }
                OnWebSocketMessageReceived(new KlipperEventArgs()
                {
                    CallbackId = PingCounter,
                    Message = e.Message,
                    SessonId = SessionId,
                });
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = e.Message,
                    Message = jecx.Message,
                });
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public void SendWebSocketCommand(string command)
        {
            try
            {
                //string infoCommand = $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.info\",\"params\":{{}},\"id\":1}}";
                if (WebSocket?.State == WebSocketState.Open)
                {
                    WebSocket.Send(command);
                }
            }
            catch (Exception exc)
            {
                OnWebSocketError(new ErrorEventArgs(exc));
            }
        }

        #endregion

        #endregion
    }
}
