using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Moonraker.Structs;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.REST.Events;
using AndreasReitberger.API.REST.Interfaces;
using AndreasReitberger.Shared.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Properties

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial double LiveVelocity { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial double LiveExtruderVelocity { get; set; } = 0;

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial KlipperPrinterStateMessageResult? PrinterInfo { get; set; }
        partial void OnPrinterInfoChanged(KlipperPrinterStateMessageResult? value)
        {
            OnKlipperPrinterInfoChanged(new KlipperPrinterInfoChangedEventArgs()
            {
                NewPrinterInfo = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
            UpdatePrinterInfo(value);
        }
        #endregion

        #region Methods

        #region Printer Administration

        public override async Task<List<IPrinter3d>> GetPrintersAsync()
        {
            await Task.Delay(1);
            return [];
        }

        public async Task RefreshPrinterInfoAsync()
        {
            try
            {
                KlipperPrinterStateMessageResult? result = await GetPrinterInfoAsync().ConfigureAwait(false);
                PrinterInfo = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                PrinterInfo = null;
            }
        }

        public async Task<KlipperPrinterStateMessageResult?> GetPrinterInfoAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperPrinterStateMessageResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "info",
                       body: null,
                       authHeaders: AuthHeaders,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "info")
                    .ConfigureAwait(false);
                */
                KlipperPrinterStateMessageRespone? state = JsonConvertHelper.ToObject<KlipperPrinterStateMessageRespone>(result?.Result, context: MoonrakerClientSourceGenerationContext.Default);
                return state?.Result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<bool> EmergencyStopPrinterAsync()
        {
            try
            {
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "emergency_stop",
                       body: null,
                       authHeaders: AuthHeaders,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "emergency_stop")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> RestartPrinterAsync()
        {
            try
            {
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "restart",
                       body: null,
                       authHeaders: AuthHeaders,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "restart")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> RestartFirmwareAsync()
        {
            try
            {
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "firmware_restart",
                       body: null,
                       authHeaders: AuthHeaders,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "firmware_restart")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetHeaterBedTargetAsync(int target)
        {
            try
            {
                string cmd = $"SET_HEATER_TEMPERATURE HEATER=heater_bed TARGET={target}";
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetExtruderTargetAsync(int target, int extruder = 0)
        {
            try
            {
                string cmd = $"SET_HEATER_TEMPERATURE HEATER=extruder{(extruder <= 0 ? "" : $"{extruder}")} TARGET={target}";
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetSpeedFactorAsync(int target)
        {
            try
            {
                string cmd = $"M220 S{target}";
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetExtrusionFactorAsync(int target)
        {
            try
            {
                string cmd = $"M221 S{target}";
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetFanSpeedTargetAsync(int target, bool isPercentage, int fanId = 0)
        {
            try
            {
                int setSpeed = target;
                if (!isPercentage)
                {
                    // Avoid invalid ranges
                    switch (target)
                    {
                        case > 255:
                            setSpeed = 255;
                            break;
                        case < 0:
                            setSpeed = 0;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    setSpeed = Convert.ToInt32(target * 255f / 100f);
                }

                string cmd = $"M106 S{setSpeed}";
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Printer Status
        public async Task<List<string>> GetPrinterObjectListAsync(string startsWith = "", bool removeStartTag = false)
        {
            IRestApiRequestRespone? result = null;
            List<string> resultObject = [];
            try
            {
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "objects/list",
                       body: null,
                       authHeaders: AuthHeaders,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "objects/list")
                    .ConfigureAwait(false);
                */
                KlipperActionListRespone? state = JsonConvertHelper.ToObject<KlipperActionListRespone>(result?.Result, context: MoonrakerClientSourceGenerationContext.Default);
                if (!string.IsNullOrEmpty(startsWith))
                {
                    resultObject = [.. state?.Result?.Objects.Where(obj => obj.StartsWith(startsWith)) ?? []];
                    if (resultObject is not null && removeStartTag)
                    {
                        resultObject = resultObject.Select(item => item.Replace(startsWith, string.Empty).Trim()).ToList();
                    }
                    return resultObject ?? [];
                }
                return state?.Result?.Objects ?? resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, object>> QueryPrinterObjectStatusAsync(Dictionary<string, string> objects)
        {
            IRestApiRequestRespone? result = null;
            Dictionary<string, object> resultObject = [];
            try
            {
                List<Tuple<string, string>> urlSegments = [];
                foreach (KeyValuePair<string, string> obj in objects)
                {
                    // Do not query macros here, there is an extra method for this.
                    if (obj.Key.StartsWith("gcode_macro")) continue;
                    urlSegments.Add(new(obj.Key, obj.Value));
                }
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "objects/query",
                       body: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);

                KlipperPrinterStatusRespone? queryResult = JsonConvertHelper.ToObject<KlipperPrinterStatusRespone>(result?.Result, context: MoonrakerClientSourceGenerationContext.Default);
                if (queryResult?.Result?.Status is JsonObject jsonObject)
                {
                    foreach (KeyValuePair<string, JsonNode?> property in jsonObject)
                    {
                        Stack<JsonNode> availableProperties = new(jsonObject.Select(kvp => kvp.Value).Where(v => v != null)!);
                        do
                        {
                            JsonNode token = availableProperties.Pop();
                            if (token is JsonObject propTest)
                            {
                                // Get the childs for this tags
                                foreach (KeyValuePair<string, JsonNode?> prop in propTest)
                                {
                                    if (prop.Key.StartsWith("configfile") ||
                                        prop.Key.StartsWith("settings"))
                                    {
                                        // Kinder auf den Stack legen
                                        if (prop.Value is JsonObject childObj)
                                        {
                                            foreach (var child in childObj)
                                            {
                                                if (child.Value != null)
                                                    availableProperties.Push(child.Value);
                                            }
                                        }
                                        continue;
                                    }
                                }
                                /*
                                if (propTest.Name.StartsWith("configfile") || propTest.Name.StartsWith("settings"))
                                {
                                    // Add all child properties back to the stack
                                    List<JToken> children = token.Children().ToList();
                                    foreach (JToken child in children)
                                    {
                                        avilableProperties.Push(child);
                                    }
                                    continue;
                                }
                                */
                            }
                            /*
                            else if (token is JToken childToken)
                            {
                                if (childToken?.First is not JProperty jp)
                                    continue;
                                // Get the childs for this tags
                                if (jp.Name.StartsWith("configfile") || jp.Name.StartsWith("settings"))
                                {
                                    // Add all child properties back to the stack
                                    List<JToken> children = [.. token.Children()];
                                    foreach (JToken child in children)
                                    {
                                        avilableProperties.Push(child);
                                    }
                                    continue;
                                }

                            }
                            if (token is not JProperty parent)
                            {
                                // Add all child properties back to the stack
                                List<JToken> chilTokens = [.. token.Children()];
                                foreach (JToken child in chilTokens)
                                {
                                    avilableProperties.Push(child);
                                }
                                continue;
                            }
                            */

                            // DEBUG THIS FIRST WITH A LIVE SESSION!!!!
                            throw new NotImplementedException("This part of the code needs to be debugged and implemented properly. The current implementation is incomplete and may not handle all cases correctly.");
                            /*
                            string name = parent.Name;
                            string path = parent.Path;
                            string jsonBody = parent.Value.ToString();
                            switch (name)
                            {
                                case "probe":
                                    KlipperStatusProbe? probe =
                                        JsonConvertHelper.ToObject<KlipperStatusProbe>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (probe is not null)
                                        resultObject.Add(name, probe);
                                    break;
                                case "configfile":
                                    KlipperStatusConfigfile? configFile =
                                        JsonConvertHelper.ToObject<KlipperStatusConfigfile>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (configFile is not null)
                                        resultObject.Add(name, configFile);
                                    break;
                                case "query_endstops":
                                    KlipperStatusQueryEndstops? endstops =
                                        JsonConvertHelper.ToObject<KlipperStatusQueryEndstops>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (endstops is not null)
                                        resultObject.Add(name, endstops);
                                    break;
                                case "virtual_sdcard":
                                    KlipperStatusVirtualSdcard? virtualSdcardState =
                                        JsonConvertHelper.ToObject<KlipperStatusVirtualSdcard>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (virtualSdcardState is not null)
                                        resultObject.Add(name, virtualSdcardState);
                                    break;
                                case "display_status":
                                    KlipperStatusDisplay? displayState =
                                        JsonConvertHelper.ToObject<KlipperStatusDisplay>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (displayState is not null)
                                        resultObject.Add(name, displayState);
                                    break;
                                case "moonraker_stats":
                                    MoonrakerStatInfo? notifyProcState =
                                        JsonConvertHelper.ToObject<MoonrakerStatInfo>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (notifyProcState is not null)
                                        resultObject.Add(name, notifyProcState);
                                    break;
                                case "mcu":
                                    KlipperStatusMcu? mcuState =
                                        JsonConvertHelper.ToObject<KlipperStatusMcu>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (mcuState is not null)
                                        resultObject.Add(name, mcuState);
                                    break;
                                case "system_stats":
                                    KlipperStatusSystemStats? systemState =
                                        JsonConvertHelper.ToObject<KlipperStatusSystemStats>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (systemState is not null)
                                        resultObject.Add(name, systemState);
                                    break;
                                case "cpu_temp":
                                    double cpuTemp =
                                        JsonConvertHelper.ToObject<double>(jsonBody.Replace(",", "."));
                                    resultObject.Add(name, cpuTemp);
                                    break;
                                case "websocket_connections":
                                    int wsConnections =
                                        JsonConvertHelper.ToObject<int>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    resultObject.Add(name, wsConnections);
                                    break;
                                case "network":
                                    Dictionary<string, KlipperNetworkInterface>? network =
                                        JsonConvertHelper.ToObject<Dictionary<string, KlipperNetworkInterface>>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (network is not null)
                                        resultObject.Add(name, network);
                                    break;
                                case "gcode_move":
                                    KlipperStatusGcodeMove? gcodeMoveState =
                                        JsonConvertHelper.ToObject<KlipperStatusGcodeMove>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (gcodeMoveState is not null)
                                        resultObject.Add(name, gcodeMoveState);
                                    break;
                                case "print_stats":
                                    KlipperStatusPrintStats? printStats =
                                        JsonConvertHelper.ToObject<KlipperStatusPrintStats>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (printStats is not null)
                                    {
                                        printStats.ValidPrintState = jsonBody.Contains("state");
                                        resultObject.Add(name, printStats);
                                    }
                                    break;
                                case "fan":
                                    KlipperStatusFan? fanState =
                                        JsonConvertHelper.ToObject<KlipperStatusFan>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (fanState is not null)
                                        resultObject.Add(name, fanState);
                                    break;
                                case "toolhead":
                                    KlipperStatusToolhead? toolhead =
                                        JsonConvertHelper.ToObject<KlipperStatusToolhead>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (toolhead is not null)
                                        resultObject.Add(name, toolhead);
                                    break;
                                case "heater_bed":
                                    // In the status report the temp is missing, so do not parse the heater then.
                                    //if (!jsonBody.Contains("temperature")) break;
                                    if (path.EndsWith("settings.heater_bed"))
                                    {
                                        KlipperConfigHeaterBed? settingsHeaterBed =
                                            JsonConvertHelper.ToObject<KlipperConfigHeaterBed>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                        if (settingsHeaterBed is not null)
                                            resultObject.Add(name, settingsHeaterBed);
                                    }
                                    else
                                    {
                                        KlipperStatusHeaterBed? heaterBed =
                                            JsonConvertHelper.ToObject<KlipperStatusHeaterBed>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                        if (heaterBed is not null)
                                            resultObject.Add(name, heaterBed);
                                    }
                                    break;
                                case "extruder":
                                case "extruder1":
                                case "extruder2":
                                case "extruder3":
                                    // In the status report the temp is missing, so do not parse the heater then.
                                    //if (!jsonBody.Contains("temperature")) break;
                                    if (path.EndsWith("settings.extruder"))
                                    {
                                        KlipperConfigExtruder? settingsExtruder =
                                            JsonConvertHelper.ToObject<KlipperConfigExtruder>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                        if (settingsExtruder is not null)
                                            resultObject.Add(name, settingsExtruder);
                                    }
                                    else
                                    {
                                        KlipperStatusExtruder? extruder =
                                            JsonConvertHelper.ToObject<KlipperStatusExtruder>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                        if (extruder is not null)
                                            resultObject.Add(name, extruder);
                                    }
                                    break;
                                case "motion_report":
                                    KlipperStatusMotionReport? motionReport =
                                        JsonConvertHelper.ToObject<KlipperStatusMotionReport>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (motionReport is not null)
                                        resultObject.Add(name, motionReport);
                                    break;
                                case "idle_timeout":
                                    KlipperStatusIdleTimeout? idleTimeout =
                                        JsonConvertHelper.ToObject<KlipperStatusIdleTimeout>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (idleTimeout is not null)
                                    {
                                        idleTimeout.ValidState = jsonBody.Contains("state");
                                        resultObject.Add(name, idleTimeout);
                                    }
                                    break;
                                case "filament_switch_sensor fsensor":
                                    KlipperStatusFilamentSensor? fSensor =
                                        JsonConvertHelper.ToObject<KlipperStatusFilamentSensor>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (fSensor is not null)
                                        resultObject.Add(name, fSensor);
                                    break;
                                case "pause_resume":
                                    KlipperStatusPauseResume? pauseResume =
                                        JsonConvertHelper.ToObject<KlipperStatusPauseResume>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (pauseResume is not null)
                                        resultObject.Add(name, pauseResume);
                                    break;
                                case "action":
                                    string action = jsonBody;
                                    resultObject.Add(name, action);
                                    break;
                                case "bed_mesh":
                                    KlipperStatusMesh? mesh =
                                        JsonConvertHelper.ToObject<KlipperStatusMesh>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (mesh is not null)
                                        resultObject.Add(name, mesh);
                                    break;
                                case "job":
                                    KlipperStatusJob? job =
                                        JsonConvertHelper.ToObject<KlipperStatusJob>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                    if (job is not null)
                                        resultObject.Add(name, job);
                                    break;
                                default:
#if DEBUG
                                    Console.WriteLine($"No Json object found for '{name}' => '{jsonBody}");
#endif
                                    if (name.StartsWith("gcode_macro"))
                                    {
                                        KlipperGcodeMacro? gcMacro =
                                            JsonConvertHelper.ToObject<KlipperGcodeMacro>(jsonBody, context: MoonrakerClientSourceGenerationContext.Default);
                                        if (gcMacro is not null)
                                        {
                                            if (string.IsNullOrEmpty(gcMacro.Name))
                                            {
                                                gcMacro.Name = name.Replace("gcode_macro", string.Empty).Trim();
                                            }
                                            resultObject.Add(name, gcMacro);
                                        }
                                    }
                                    else
                                    {
                                        // If no parser found, pass the json object instead
                                        resultObject.Add(name, parent.Value);
                                    }
#if ConcurrentDictionary
                                    ConcurrentDictionary<string, string> loggedResults = new(IgnoredJsonResults);
#else
                                    Dictionary<string, string> loggedResults = new(IgnoredJsonResults);
#endif
                                    if (!loggedResults.ContainsKey(name))
                                    {
                                        // Log unused json results for further releases
#if ConcurrentDictionary
                                        loggedResults.TryAdd(name, jsonBody);
#else
                                        loggedResults.Add(name, jsonBody);
#endif
                                        IgnoredJsonResults = loggedResults;
                                    }
                                    break;
                            }
                            */
                        }
                        while (availableProperties.Count > 0);
                    }
                }
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, KlipperGcodeMacro>> GetGcodeMacrosAsync()
        {
            IRestApiRequestRespone? result = null;
            Dictionary<string, KlipperGcodeMacro> resultObject = [];
            try
            {
                Dictionary<string, string> objects = new()
                {
                    { "configfile", "settings" }
                };

                Dictionary<string, object> settings = await QueryPrinterObjectStatusAsync(objects).ConfigureAwait(false);
#if NETSTANDARD || NET6_0_OR_GREATER
                IEnumerable<KeyValuePair<string, KlipperGcodeMacro>> macros =
                    settings.Where(keypair => keypair.Key.StartsWith("gcode_macro"))
                    .Select(pair => new KeyValuePair<string, KlipperGcodeMacro>(pair.Key, pair.Value as KlipperGcodeMacro));
                return new(macros);
#else
                List<KeyValuePair<string, KlipperGcodeMacro>> macros =
                    settings.Where(keypair => keypair.Key.StartsWith("gcode_macro"))
                    .Select(pair => new KeyValuePair<string, KlipperGcodeMacro>(pair.Key, pair.Value as KlipperGcodeMacro))
                .ToList()
                ;

                resultObject = new();
                for (int i = 0; i < macros?.Count; i++)
                {
                    resultObject.Add(macros[i].Key, macros[i].Value);
                }
                return resultObject;
#endif
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
        public async Task<Dictionary<string, KlipperStatusFilamentSensor>> GetFilamentSensorsAsync(Dictionary<string, string>? macros = null)
        {
            IRestApiRequestRespone? result = null;
            Dictionary<string, KlipperStatusFilamentSensor> resultObject = [];
            try
            {
                List<Tuple<string, string>> urlSegments = [];
                if (macros is not null)
                {
                    foreach (KeyValuePair<string, string> obj in macros)
                    {
                        urlSegments.Add(new(obj.Key, obj.Value));
                    }
                }
                else
                {
                    urlSegments.Add(new("filament_switch_sensor", string.Empty));
                }
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "objects/query",
                       body: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                KlipperFilamentSensorsRespone? queryResult = JsonConvertHelper.ToObject<KlipperFilamentSensorsRespone>(result?.Result, context: MoonrakerClientSourceGenerationContext.Default);
                return queryResult?.Result?.Status ?? resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperStatusPrintStats?> GetPrintStatusAsync()
        {
            KlipperStatusPrintStats? resultObject = null;
            try
            {
                // Doc: https://moonraker.readthedocs.io/en/latest/printer_objects/#print_stats
                string key = "print_stats";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, "" }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                if (result.ContainsKey(key) && result?[key] is KlipperStatusPrintStats stateObj)
                {
                    stateObj.ValidPrintState = true;
                    //IsPrinting = stateObj.State == KlipperPrintStates.Printing;
                    resultObject = stateObj;
                }
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshPrintStatusAsync()
        {
            try
            {
                KlipperStatusPrintStats? result = await GetPrintStatusAsync().ConfigureAwait(false);
                PrintStats = result;
                if (PrintStats is not null)
                {
                    await RefreshGcodeMetadataAsync(PrintStats.Filename).ConfigureAwait(false);
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                PrintStats = null;
                GcodeMeta = null;
            }
        }

        public async Task<KlipperStatusExtruder?> GetExtruderStatusAsync(int index = 0)
        {
            KlipperStatusExtruder? resultObject = null;
            try
            {
                string key = $"extruder{(index > 0 ? index : "")}";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, "" }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                if (result.ContainsKey(key) && result?[key] is KlipperStatusExtruder stateObj)
                    resultObject = stateObj;
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshExtruderStatusAsync(int index = 0)
        {
            try
            {
                KlipperStatusExtruder? result = await GetExtruderStatusAsync(index).ConfigureAwait(false);
                if (result is not null)
                {
#if ConcurrentDictionary
                    ConcurrentDictionary<int, IToolhead> states = new(Toolheads);
                    states.AddOrUpdate(index, result, (key, oldValue) => oldValue = result);
#else
                    Dictionary<int, KlipperStatusExtruder> states = new()
                    {
                        { index, result }
                    };
#endif
                    //Extruders = states;
                    Toolheads = states;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Toolheads = new();
            }
        }

        public async Task<KlipperStatusFan?> GetFanStatusAsync()
        {
            KlipperStatusFan? resultObject = null;
            try
            {
                string key = "fan";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, "" }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                if (result.ContainsKey(key) && result?[key] is KlipperStatusFan stateObj)
                    resultObject = stateObj;
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshFanStatusAsync()
        {
            try
            {
                KlipperStatusFan? result = await GetFanStatusAsync().ConfigureAwait(false);
                ConcurrentDictionary<string, IPrint3dFan> fans = new(Fans);
                if (result is not null)
                    fans.AddOrUpdate("", result, (key, oldValue) => oldValue = result);
                Fans = fans;
                //Fan = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                //Fan = null;
                Fans = new();
            }
        }

        public async Task<KlipperStatusIdleTimeout?> GetIdleStatusAsync()
        {
            KlipperStatusIdleTimeout? resultObject = null;
            try
            {
                string key = "idle_timeout";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, string.Empty }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusIdleTimeout stateObj)
                {
                    stateObj.ValidState = true;
                    resultObject = stateObj;
                }
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshIdleStatusAsync()
        {
            try
            {
                KlipperStatusIdleTimeout? result = await GetIdleStatusAsync().ConfigureAwait(false);
                IdleState = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                IdleState = null;
            }
        }

        public async Task<KlipperStatusDisplay?> GetDisplayStatusAsync()
        {
            KlipperStatusDisplay? resultObject = null;
            try
            {
                string key = "display_status";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, string.Empty }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusDisplay stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshDisplayStatusAsync()
        {
            try
            {
                KlipperStatusDisplay? result = await GetDisplayStatusAsync().ConfigureAwait(false);
                DisplayStatus = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                DisplayStatus = null;
            }
        }

        public async Task<KlipperStatusToolhead?> GetToolHeadStatusAsync()
        {
            KlipperStatusToolhead? resultObject = null;
            try
            {
                string key = "toolhead";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, string.Empty }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusToolhead stateObj)
                    resultObject = stateObj;
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshToolHeadStatusAsync()
        {
            try
            {
                KlipperStatusToolhead? result = await GetToolHeadStatusAsync().ConfigureAwait(false);
                ToolHeadStatus = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                ToolHeadStatus = new();
            }
        }

        public async Task<KlipperStatusGcodeMove?> GetGcodeMoveStatusAsync()
        {
            KlipperStatusGcodeMove? resultObject = null;
            try
            {
                string key = "gcode_move";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, string.Empty }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusGcodeMove stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshGcodeMoveStatusAsync()
        {
            try
            {
                KlipperStatusGcodeMove? result = await GetGcodeMoveStatusAsync().ConfigureAwait(false);
                GcodeMove = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                GcodeMove = null;
            }
        }

        public async Task<KlipperStatusMotionReport?> GetMotionReportAsync()
        {
            KlipperStatusMotionReport? resultObject = null;
            try
            {
                string key = "motion_report";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, string.Empty }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusMotionReport stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshMotionReportAsync()
        {
            try
            {
                KlipperStatusMotionReport? result = await GetMotionReportAsync().ConfigureAwait(false);
                MotionReport = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                MotionReport = null;
            }
        }

        public async Task<KlipperStatusVirtualSdcard?> GetVirtualSdCardStatusAsync()
        {
            KlipperStatusVirtualSdcard? resultObject = null;
            try
            {
                string key = "virtual_sdcard";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, string.Empty }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusVirtualSdcard stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshVirtualSdCardStatusAsync()
        {
            try
            {
                KlipperStatusVirtualSdcard? result = await GetVirtualSdCardStatusAsync().ConfigureAwait(false);
                VirtualSdCard = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                VirtualSdCard = null;
            }
        }

        public async Task<KlipperStatusHeaterBed?> GetHeaterBedStatusAsync()
        {
            KlipperStatusHeaterBed? resultObject = null;
            try
            {
                string key = "heater_bed";
                Dictionary<string, string> queryObjects = new()
                {
                    { key, string.Empty }
                };

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusHeaterBed stateObj)
                {
                    resultObject = stateObj;
                }
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshHeaterBedStatusAsync()
        {
            try
            {
                KlipperStatusHeaterBed? result = await GetHeaterBedStatusAsync().ConfigureAwait(false);
                ConcurrentDictionary<int, IHeaterComponent> heaters = new(HeatedBeds);
                if (result is not null)
                    heaters.AddOrUpdate(0, result, (key, oldValue) => oldValue = result);
                HeatedBeds = heaters;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                HeatedBeds = new();
            }
        }

        //public Task<string> SubscribePrinterObjectStatusAsync(long? connectionId, List<string> objects) => SubscribePrinterObjectStatusAsync((long)connectionId, objects);
        public async Task<string> SubscribePrinterObjectStatusAsync(long? connectionId, List<string> objects)
        {
            IRestApiRequestRespone? result = null;
            string? resultObject = "";
            try
            {
                List<Tuple<string, string>> urlSegments = 
                [
                    new("connection_id", $"{connectionId}")
                ];

                for (int i = 0; i < objects.Count; i++)
                {
                    string key = objects[i];
                    string value = string.Empty;
                    urlSegments.Add(new(key, value));
                }

                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "objects/subscribe",
                       body: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                return result?.Result ?? resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<string> SubscribeAllPrinterObjectStatusAsync(long? connectionId)
        {
            List<string> objects = await GetPrinterObjectListAsync().ConfigureAwait(false);
            return await SubscribePrinterObjectStatusAsync(connectionId, objects).ConfigureAwait(false);
        }
        public async Task<KlipperEndstopQueryResult?> QueryEndstopsAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperEndstopQueryResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "query_endstops/status",
                       body: null,
                       authHeaders: AuthHeaders,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "query_endstops/status").ConfigureAwait(false);
                KlipperEndstopQueryRespone? queryResult = JsonConvertHelper.ToObject<KlipperEndstopQueryRespone>(result?.Result, context: MoonrakerClientSourceGenerationContext.Default);
                return queryResult?.Result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #endregion
    }
}
