using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Websocket.Client;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Properties
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

        #endregion

        #region Methods

        /* Not available for HTTP
        public async Task<KlipperAccessTokenResult> GetWebSocketIdAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperAccessTokenResult resultObject = null;
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.Get, "websocket_id").ConfigureAwait(false);
                KlipperAccessTokenResult accessToken = GetObjectFromJson<KlipperAccessTokenResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                SessionId = accessToken?.Result;
                return accessToken;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        */
        protected void Client_WebSocketMessageReceived(object sender, WebsocketEventArgs e)
        {
            try
            {
                if (e == null || string.IsNullOrEmpty(e.Message))
                    return;
                string text = e.Message;
                if (text.ToLower().Contains("method"))
                {
                    string name = string.Empty;
                    string jsonBody = string.Empty;
                    try
                    {
                        ConcurrentDictionary<int, KlipperStatusExtruder> extruderStats = new();
                        KlipperWebSocketMessage method = JsonConvert.DeserializeObject<KlipperWebSocketMessage>(text);
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
                                        if (tempUsageObject is not null)
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
                                        if (tempMemoryObject is not null)
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
                                        if (PrintStats is not null)
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
                                        ToolHeadStatus = toolhead;
                                        break;
                                    case "heater_bed":
                                        // In the status report the temp is missing, so do not parse the heater then.
                                        if (!jsonBody.Contains("temperature") || RefreshHeatersDirectly) break;
                                        KlipperStatusHeaterBed heaterBed =
                                            JsonConvert.DeserializeObject<KlipperStatusHeaterBed>(jsonBody);
                                        HeatedBeds ??= new();
                                        if (ActiveHeatedBed is not null)
                                        {
                                            // This property is only sent once if changed, so store it
                                            if (!jsonBody.Contains("target"))
                                            {
                                                heaterBed.TempSet = ActiveHeatedBed.TempSet;
                                            }
                                        }
                                        HeatedBeds.AddOrUpdate(0, heaterBed, (key, oldValue) => oldValue = heaterBed);
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
                                                extruder.TempSet = previousExtruderState.TempSet;
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
                                        ActiveJob = job;
                                        //if (JobStatus?.Status == KlipperJobStates.Completed)
                                        if (ActiveJob?.State == Print3dJobState.Completed)
                                        {
                                            OnJobStatusFinished(new()
                                            {
                                                JobStatus = job,
                                            });
                                            OnJobFinished(new()
                                            {
                                                Job = null,
                                            });
                                        }
                                        break;
                                    case "updated_queue":
                                        if (string.IsNullOrEmpty(jsonBody))
                                        {
                                            Jobs = new();
                                            break;
                                        }
                                        List<KlipperJobQueueItem> queueUpdate =
                                            JsonConvert.DeserializeObject<List<KlipperJobQueueItem>>(jsonBody);
                                        Jobs = new(queueUpdate);
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
                                                    if (fanObject is not null)
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
                                                    if (tempObject is not null)
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
                                                    if (drvObject is not null)
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
                else if (text.ToLower().Contains("error"))
                {
                    //Session = JsonConvert.DeserializeObject<EventSession>(text);
                }
                else if (text.ToLower().Contains("result"))
                {
                    try
                    {
                        KlipperWebSocketResult result = JsonConvert.DeserializeObject<KlipperWebSocketResult>(text);
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
                                        ConcurrentDictionary<string, string> loggedResults = new(IgnoredJsonResults);
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
				/*
                OnWebSocketMessageReceived(new WebsocketEventArgs()
                {
                    CallbackId = PingCounter,
                    Message = e.Message,
                    SessionId = SessionId,
                });
				*/
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

        new void WebSocket_MessageReceived(ResponseMessage? msg)
        {
            try
            {
                if (msg.Text == null || string.IsNullOrEmpty(msg.Text))
                    return;
                base.WebSocket_MessageReceived(msg);
                string text = msg.Text;
                if (text.ToLower().Contains("method"))
                {
                    string name = string.Empty;
                    string jsonBody = string.Empty;
                    try
                    {
                        ConcurrentDictionary<int, KlipperStatusExtruder> extruderStats = new();
                        KlipperWebSocketMessage method = JsonConvert.DeserializeObject<KlipperWebSocketMessage>(text);
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
                                        if (tempUsageObject is not null)
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
                                        if (tempMemoryObject is not null)
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
                                        if (PrintStats is not null)
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
                                        ToolHeadStatus = toolhead;
                                        break;
                                    case "heater_bed":
                                        // In the status report the temp is missing, so do not parse the heater then.
                                        if (!jsonBody.Contains("temperature") || RefreshHeatersDirectly) break;
                                        KlipperStatusHeaterBed heaterBed =
                                            JsonConvert.DeserializeObject<KlipperStatusHeaterBed>(jsonBody);
                                        HeatedBeds ??= new();
                                        if (ActiveHeatedBed is not null)
                                        {
                                            // This property is only sent once if changed, so store it
                                            if (!jsonBody.Contains("target"))
                                            {
                                                heaterBed.TempSet = ActiveHeatedBed.TempSet;
                                            }
                                        }
                                        HeatedBeds.AddOrUpdate(0, heaterBed, (key, oldValue) => oldValue = heaterBed);
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
                                                extruder.TempSet = previousExtruderState.TempSet;
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
                                        ActiveJob = job;
                                        //if (JobStatus?.Status == KlipperJobStates.Completed)
                                        if (ActiveJob?.State == Print3dJobState.Completed)
                                        {
                                            OnJobStatusFinished(new()
                                            {
                                                JobStatus = job,
                                            });
                                            OnJobFinished(new()
                                            {
                                                Job = null,
                                            });
                                        }
                                        break;
                                    case "updated_queue":
                                        if (string.IsNullOrEmpty(jsonBody))
                                        {
                                            Jobs = new();
                                            break;
                                        }
                                        List<KlipperJobQueueItem> queueUpdate =
                                            JsonConvert.DeserializeObject<List<KlipperJobQueueItem>>(jsonBody);
                                        Jobs = new(queueUpdate);
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
                                                    if (fanObject is not null)
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
                                                    if (tempObject is not null)
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
                                                    if (drvObject is not null)
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
                        if (extruderStats?.Count > 0)
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
                else if (text.ToLower().Contains("error"))
                {
                    //Session = JsonConvert.DeserializeObject<EventSession>(text);
                }
                else if (text.ToLower().Contains("result"))
                {
                    try
                    {
                        KlipperWebSocketResult result = JsonConvert.DeserializeObject<KlipperWebSocketResult>(text);
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
                                        ConcurrentDictionary<string, string> loggedResults = new(IgnoredJsonResults);
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
                            OriginalString = msg.Text,
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
                OnWebSocketMessageReceived(new WebsocketEventArgs()
                {
                    CallbackId = PingCounter,
                    Message = msg.Text,
                    SessionId = SessionId,
                });
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = msg.Text,
                    Message = jecx.Message,
                });
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        #endregion
    }
}
