using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Moonraker.Structs;
using AndreasReitberger.API.Print3dServer.Core.Events;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Properties

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        double liveVelocity = 0;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        double liveExtruderVelocity = 0;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperPrinterStateMessageResult? printerInfo;
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

        /*
        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        ObservableCollection<KlipperDatabaseRemotePrinter> printers = new();
        partial void OnPrintersChanged(ObservableCollection<KlipperDatabaseRemotePrinter> value)
        {
            OnKlipperRemotePrinterChanged(new KlipperRemotePrintersChangedEventArgs()
            {
                NewPrinters = value,
                SessionId = SessionId,
                CallbackId = -1,
                Token = !string.IsNullOrEmpty(UserToken) ? UserToken : ApiKey,
            });
        }
        */
        #endregion

        #region Methods

        #region Printer Administration

        public override async Task<ObservableCollection<IPrinter3d>> GetPrintersAsync()
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
                //object cmd = new { name = ScriptName };

                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "info",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "info")
                    .ConfigureAwait(false);
                */
                KlipperPrinterStateMessageRespone? state = GetObjectFromJson<KlipperPrinterStateMessageRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return state?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperPrinterStateMessageRespone),
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

        public async Task<bool> EmergencyStopPrinterAsync()
        {
            try
            {
                //object cmd = new { name = ScriptName };

                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "emergency_stop",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
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
                //object cmd = new { name = ScriptName };
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "restart",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
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
                //object cmd = new { name = ScriptName };
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "firmware_restart",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
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
                //object cmd = new { name = ScriptName };
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "objects/list",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "objects/list")
                    .ConfigureAwait(false);
                */
                KlipperActionListRespone? state = GetObjectFromJson<KlipperActionListRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                if (!string.IsNullOrEmpty(startsWith))
                {
                    resultObject = [.. state?.Result?.Objects.Where(obj => obj.StartsWith(startsWith))];
                    if (resultObject is not null && removeStartTag)
                    {
                        resultObject = resultObject.Select(item => item.Replace(startsWith, string.Empty).Trim()).ToList();
                    }
                    return resultObject ?? [];
                }
                return state?.Result?.Objects ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperActionListRespone),
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

        public async Task<Dictionary<string, object>> QueryPrinterObjectStatusAsync(Dictionary<string, string> objects)
        {
            IRestApiRequestRespone? result = null;
            Dictionary<string, object> resultObject = [];
            try
            {
                Dictionary<string, string> urlSegments = [];
                foreach (KeyValuePair<string, string> obj in objects)
                {
                    // Do not query macros here, there is an extra method for this.
                    if (obj.Key.StartsWith("gcode_macro")) continue;
                    urlSegments.Add(obj.Key, obj.Value);
                }
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "objects/query",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "objects/query", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperPrinterStatusRespone? queryResult = GetObjectFromJson<KlipperPrinterStatusRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                if (queryResult?.Result?.Status is JObject jsonObject)
                {
                    foreach (JProperty property in jsonObject.Children<JProperty>())
                    {
                        Stack<JToken> avilableProperties = new(jsonObject.Children<JToken>());
                        do
                        {
                            JToken token = avilableProperties.Pop();
                            if (token is JProperty propTest)
                            {
                                // Get the childs for this tags
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
                            }
                            else if (token is JToken childToken)
                            {
                                if (childToken?.First is not JProperty jp)
                                    continue;
                                /**/
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

                            string name = parent.Name;
                            string path = parent.Path;
                            string jsonBody = parent.Value.ToString();
                            switch (name)
                            {
                                case "probe":
                                    KlipperStatusProbe? probe =
                                        GetObjectFromJson<KlipperStatusProbe>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (probe is not null)
                                        resultObject.Add(name, probe);
                                    break;
                                case "configfile":
                                    KlipperStatusConfigfile? configFile =
                                        GetObjectFromJson<KlipperStatusConfigfile>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (configFile is not null)
                                        resultObject.Add(name, configFile);
                                    break;
                                case "query_endstops":
                                    KlipperStatusQueryEndstops? endstops =
                                        GetObjectFromJson<KlipperStatusQueryEndstops>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (endstops is not null)
                                        resultObject.Add(name, endstops);
                                    break;
                                case "virtual_sdcard":
                                    KlipperStatusVirtualSdcard? virtualSdcardState =
                                        GetObjectFromJson<KlipperStatusVirtualSdcard>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (virtualSdcardState is not null)
                                        resultObject.Add(name, virtualSdcardState);
                                    break;
                                case "display_status":
                                    KlipperStatusDisplay? displayState =
                                        GetObjectFromJson<KlipperStatusDisplay>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (displayState is not null)
                                        resultObject.Add(name, displayState);
                                    break;
                                case "moonraker_stats":
                                    MoonrakerStatInfo? notifyProcState =
                                        GetObjectFromJson<MoonrakerStatInfo>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (notifyProcState is not null)
                                        resultObject.Add(name, notifyProcState);
                                    break;
                                case "mcu":
                                    KlipperStatusMcu? mcuState =
                                        GetObjectFromJson<KlipperStatusMcu>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (mcuState is not null)
                                        resultObject.Add(name, mcuState);
                                    break;
                                case "system_stats":
                                    KlipperStatusSystemStats? systemState =
                                        GetObjectFromJson<KlipperStatusSystemStats>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (systemState is not null)
                                        resultObject.Add(name, systemState);
                                    break;
                                case "cpu_temp":
                                    double cpuTemp =
                                        GetObjectFromJson<double>(jsonBody.Replace(",", "."));
                                    resultObject.Add(name, cpuTemp);
                                    break;
                                case "websocket_connections":
                                    int wsConnections =
                                        GetObjectFromJson<int>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    resultObject.Add(name, wsConnections);
                                    break;
                                case "network":
                                    Dictionary<string, KlipperNetworkInterface>? network =
                                        GetObjectFromJson<Dictionary<string, KlipperNetworkInterface>>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (network is not null)
                                        resultObject.Add(name, network);
                                    break;
                                case "gcode_move":
                                    KlipperStatusGcodeMove? gcodeMoveState =
                                        GetObjectFromJson<KlipperStatusGcodeMove>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (gcodeMoveState is not null)
                                        resultObject.Add(name, gcodeMoveState);
                                    break;
                                case "print_stats":
                                    KlipperStatusPrintStats? printStats =
                                        GetObjectFromJson<KlipperStatusPrintStats>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (printStats is not null)
                                    {
                                        printStats.ValidPrintState = jsonBody.Contains("state");
                                        resultObject.Add(name, printStats);
                                    }
                                    break;
                                case "fan":
                                    KlipperStatusFan? fanState =
                                        GetObjectFromJson<KlipperStatusFan>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (fanState is not null)
                                        resultObject.Add(name, fanState);
                                    break;
                                case "toolhead":
                                    KlipperStatusToolhead? toolhead =
                                        GetObjectFromJson<KlipperStatusToolhead>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (toolhead is not null)
                                        resultObject.Add(name, toolhead);
                                    break;
                                case "heater_bed":
                                    // In the status report the temp is missing, so do not parse the heater then.
                                    //if (!jsonBody.Contains("temperature")) break;
                                    if (path.EndsWith("settings.heater_bed"))
                                    {
                                        KlipperConfigHeaterBed? settingsHeaterBed =
                                            GetObjectFromJson<KlipperConfigHeaterBed>(jsonBody, NewtonsoftJsonSerializerSettings);
                                        if (settingsHeaterBed is not null)
                                            resultObject.Add(name, settingsHeaterBed);
                                    }
                                    else
                                    {
                                        KlipperStatusHeaterBed? heaterBed =
                                            GetObjectFromJson<KlipperStatusHeaterBed>(jsonBody, NewtonsoftJsonSerializerSettings);
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
                                            GetObjectFromJson<KlipperConfigExtruder>(jsonBody, NewtonsoftJsonSerializerSettings);
                                        if (settingsExtruder is not null)
                                            resultObject.Add(name, settingsExtruder);
                                    }
                                    else
                                    {
                                        KlipperStatusExtruder? extruder =
                                            GetObjectFromJson<KlipperStatusExtruder>(jsonBody, NewtonsoftJsonSerializerSettings);
                                        if (extruder is not null)
                                            resultObject.Add(name, extruder);
                                    }
                                    break;
                                case "motion_report":
                                    KlipperStatusMotionReport? motionReport =
                                        GetObjectFromJson<KlipperStatusMotionReport>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (motionReport is not null)
                                        resultObject.Add(name, motionReport);
                                    break;
                                case "idle_timeout":
                                    KlipperStatusIdleTimeout? idleTimeout =
                                        GetObjectFromJson<KlipperStatusIdleTimeout>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (idleTimeout is not null)
                                    {
                                        idleTimeout.ValidState = jsonBody.Contains("state");
                                        resultObject.Add(name, idleTimeout);
                                    }
                                    break;
                                case "filament_switch_sensor fsensor":
                                    KlipperStatusFilamentSensor? fSensor =
                                        GetObjectFromJson<KlipperStatusFilamentSensor>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (fSensor is not null)
                                        resultObject.Add(name, fSensor);
                                    break;
                                case "pause_resume":
                                    KlipperStatusPauseResume? pauseResume =
                                        GetObjectFromJson<KlipperStatusPauseResume>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (pauseResume is not null)
                                        resultObject.Add(name, pauseResume);
                                    break;
                                case "action":
                                    string action = jsonBody;
                                    resultObject.Add(name, action);
                                    break;
                                case "bed_mesh":
                                    KlipperStatusMesh? mesh =
                                        GetObjectFromJson<KlipperStatusMesh>(jsonBody, NewtonsoftJsonSerializerSettings);
                                    if (mesh is not null)
                                        resultObject.Add(name, mesh);
                                    break;
                                case "job":
                                    KlipperStatusJob? job =
                                        GetObjectFromJson<KlipperStatusJob>(jsonBody, NewtonsoftJsonSerializerSettings);
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
                                            GetObjectFromJson<KlipperGcodeMacro>(jsonBody, NewtonsoftJsonSerializerSettings);
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

                        }
                        while (avilableProperties.Count > 0);
                    }
                }
                return resultObject;
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

        public async Task<Dictionary<string, KlipperGcodeMacro>> GetGcodeMacrosAsync()
        {
            IRestApiRequestRespone? result = null;
            Dictionary<string, KlipperGcodeMacro> resultObject = new();
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
        public async Task<Dictionary<string, KlipperStatusFilamentSensor>> GetFilamentSensorsAsync(Dictionary<string, string> macros = null)
        {
            IRestApiRequestRespone? result = null;
            Dictionary<string, KlipperStatusFilamentSensor> resultObject = [];
            try
            {
                Dictionary<string, string> urlSegments = [];
                if (macros is not null)
                {
                    foreach (KeyValuePair<string, string> obj in macros)
                    {
                        urlSegments.Add(obj.Key, obj.Value);
                    }
                }
                else
                {
                    urlSegments.Add("filament_switch_sensor", string.Empty);
                }
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "objects/query",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "objects/query", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperFilamentSensorsRespone? queryResult = GetObjectFromJson<KlipperFilamentSensorsRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result?.Status ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperFilamentSensorsRespone),
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

        public async Task<KlipperStatusDisplay> GetDisplayStatusAsync()
        {
            KlipperStatusDisplay resultObject = null;
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
                KlipperStatusDisplay result = await GetDisplayStatusAsync().ConfigureAwait(false);
                DisplayStatus = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                DisplayStatus = null;
            }
        }

        public async Task<KlipperStatusToolhead> GetToolHeadStatusAsync()
        {
            KlipperStatusToolhead resultObject = null;
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
                KlipperStatusToolhead result = await GetToolHeadStatusAsync().ConfigureAwait(false);
                ToolHeadStatus = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                ToolHeadStatus = new();
            }
        }

        public async Task<KlipperStatusGcodeMove> GetGcodeMoveStatusAsync()
        {
            KlipperStatusGcodeMove resultObject = null;
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
                KlipperStatusGcodeMove result = await GetGcodeMoveStatusAsync().ConfigureAwait(false);
                GcodeMove = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                GcodeMove = null;
            }
        }

        public async Task<KlipperStatusMotionReport> GetMotionReportAsync()
        {
            KlipperStatusMotionReport resultObject = null;
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
                KlipperStatusMotionReport result = await GetMotionReportAsync().ConfigureAwait(false);
                MotionReport = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                MotionReport = null;
            }
        }

        public async Task<KlipperStatusVirtualSdcard> GetVirtualSdCardStatusAsync()
        {
            KlipperStatusVirtualSdcard resultObject = null;
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
                KlipperStatusVirtualSdcard result = await GetVirtualSdCardStatusAsync().ConfigureAwait(false);
                VirtualSdCard = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                VirtualSdCard = null;
            }
        }

        public async Task<KlipperStatusHeaterBed> GetHeaterBedStatusAsync()
        {
            KlipperStatusHeaterBed resultObject = null;
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
                KlipperStatusHeaterBed result = await GetHeaterBedStatusAsync().ConfigureAwait(false);
                ConcurrentDictionary<int, IHeaterComponent> heaters = new(HeatedBeds);
                heaters.AddOrUpdate(0, result, (key, oldValue) => oldValue = result);
                HeatedBeds = heaters;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                HeatedBeds = new();
            }
        }

        public Task<string> SubscribePrinterObjectStatusAsync(long? connectionId, List<string> objects) => SubscribePrinterObjectStatusAsync((long)connectionId, objects);
        public async Task<string> SubscribePrinterObjectStatusAsync(long connectionId, List<string> objects)
        {
            IRestApiRequestRespone? result = null;
            string resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "connection_id", $"{connectionId}" }
                };

                for (int i = 0; i < objects.Count; i++)
                {
                    string key = objects[i];
                    string value = string.Empty;
                    urlSegments.Add(key, value);
                }

                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "objects/subscribe",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "objects/subscribe", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                return result?.Result;
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
            return await SubscribePrinterObjectStatusAsync((long)connectionId, objects).ConfigureAwait(false);
        }
        public async Task<KlipperEndstopQueryResult> QueryEndstopsAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperEndstopQueryResult resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "query_endstops/status",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "query_endstops/status").ConfigureAwait(false);
                KlipperEndstopQueryRespone queryResult = GetObjectFromJson<KlipperEndstopQueryRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperEndstopQueryRespone),
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
        #endregion

        #endregion
    }
}
