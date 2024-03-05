using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Moonraker.Structs;
using AndreasReitberger.API.Print3dServer.Core.Events;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Properties

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        List<string> availableNamespaces = new();

        #endregion

        #region Methods
        public async Task<List<string>> ListDatabaseNamespacesAsync()
        {
            IRestApiRequestRespone result = null;
            List<string> resultObject = new();
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "database/list",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"database/list")
                    .ConfigureAwait(false);
                */
                KlipperDatabaseNamespaceListRespone queryResult = GetObjectFromJson<KlipperDatabaseNamespaceListRespone>(result.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result?.Namespaces ?? new();
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDatabaseNamespaceListRespone),
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
        public async Task RefreshDatabaseNamespacesAsync()
        {
            try
            {
                List<string> result = await ListDatabaseNamespacesAsync().ConfigureAwait(false);
                AvailableNamespaces = result ?? new();
                if (AvailableNamespaces?.Count > 0)
                {
                    // Try to detect the current operating system
                    OperatingSystem = AvailableNamespaces.Contains("mainsail") ?
                        MoonrakerOperatingSystems.MainsailOS :
                        MoonrakerOperatingSystems.FluiddPi
                        ;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                AvailableNamespaces = new();
            }
        }

        public async Task<Dictionary<string, object>> GetDatabaseItemAsync(string namespaceName, string key = "", bool throwOnMissingNamespace = false)
        {
            IRestApiRequestRespone result = null;
            Dictionary<string, object> resultObject = new();
            try
            {
                if (AvailableNamespaces?.Count == 0 || AvailableNamespaces == null)
                {
                    AvailableNamespaces = await ListDatabaseNamespacesAsync().ConfigureAwait(false);
                }
                if (!AvailableNamespaces.Contains(namespaceName))
                {
                    if (throwOnMissingNamespace)
                    {
                        throw new ArgumentOutOfRangeException(namespaceName, "The requested namespace name was not found in the database!");
                    }
                    // If namespace is missing, just return an empty resultObject now
                    else return resultObject;
                }

                Dictionary<string, string> urlSegments = new()
                {
                    { "namespace", namespaceName }
                };
                if (!string.IsNullOrEmpty(key)) urlSegments.Add("key", key);

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "database/item",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"database/item", default, null, urlSegments)
                            .ConfigureAwait(false);
                */
                KlipperDatabaseItemRespone queryResult = GetObjectFromJson<KlipperDatabaseItemRespone>(result?.Result);
                if (queryResult != null)
                {
                    resultObject = new()
                    {
                        { $"{namespaceName}{(!string.IsNullOrEmpty(key) ? $"|{key}" : "")}", queryResult?.Result?.Value }
                    };
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDatabaseItemRespone),
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

        public async Task<KlipperDatabaseSettingsGeneral> GetGeneralSettingsAsync()
        {
            string resultString = string.Empty;
            KlipperDatabaseSettingsGeneral resultObject = null;
            try
            {
                string currentNamespace = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                string currentKey = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "general" : "uiSettings";

                Dictionary<string, object> result = await GetDatabaseItemAsync(currentNamespace, currentKey).ConfigureAwait(false);

                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (string.IsNullOrEmpty(pair?.Key)) return resultObject;

                //resultString = pair.Value.ToString();
                resultString = pair.Value.Value.ToString();

                switch (OperatingSystem)
                {
                    case MoonrakerOperatingSystems.MainsailOS:
                        KlipperDatabaseMainsailValueGeneral mainsailObject = GetObjectFromJson<KlipperDatabaseMainsailValueGeneral>(resultString);
                        if (mainsailObject != null)
                        {
                            resultObject = new()
                            {
                                DisplayCancelPrint = mainsailObject.DisplayCancelPrint,
                                Printername = mainsailObject.Printername,
                            };
                        }
                        break;
                    case MoonrakerOperatingSystems.FluiddPi:
                        KlipperDatabaseFluiddValueUiSettings fluiddObject = GetObjectFromJson<KlipperDatabaseFluiddValueUiSettings>(resultString);
                        if (fluiddObject?.General != null)
                        {
                            resultObject = new()
                            {
                                Locale = fluiddObject.General.Locale,
                                Printername = fluiddObject.General.InstanceName,
                            };
                        }
                        break;
                    default:
                        break;
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValueGeneral),
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

        public async Task RefreshGeneralSettingsAsync()
        {
            try
            {
                KlipperDatabaseSettingsGeneral result = await GetGeneralSettingsAsync().ConfigureAwait(false);
                HostName = result?.Printername;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                HostName = string.Empty;
            }
        }

        public async Task<List<KlipperDatabaseRemotePrinter>> GetRemotePrintersAsync()
        {
            string resultString = string.Empty;
            List<KlipperDatabaseRemotePrinter> resultObject = new();
            try
            {
                /*
                if (OperatingSystem != MoonrakerOperatingSystems.MainsailOS)
                {
                    throw new NotSupportedException($"The method '{nameof(GetRemotePrintersAsync)}() is only support on '{MoonrakerOperatingSystems.MainsailOS}!");
                }*/
                string currentNamespace = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                string currentKey = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "remote_printers" : "uiSettings";

                Dictionary<string, object> result = await GetDatabaseItemAsync(currentNamespace, currentKey).ConfigureAwait(false);

                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (string.IsNullOrEmpty(pair?.Key)) return resultObject;

                //resultString = pair.Value.ToString();
                resultString = pair.Value.Value.ToString();

                switch (OperatingSystem)
                {
                    case MoonrakerOperatingSystems.MainsailOS:
                        List<KlipperDatabaseMainsailValueRemotePrinter> mainsailObject = GetObjectFromJson<List<KlipperDatabaseMainsailValueRemotePrinter>>(resultString);
                        if (mainsailObject != null)
                        {
                            resultObject = new(mainsailObject.Select(item => new KlipperDatabaseRemotePrinter()
                            {
                                Hostname = item.Hostname,
                                Port = item.Port,
                                Settings = item.Settings,
                                WebPort = item.WebPort,
                            }));
                        }
                        break;
                    case MoonrakerOperatingSystems.FluiddPi:
#if DEBUG
                    //throw new NotSupportedException($"The method '{nameof(GetRemotePrintersAsync)}() is only support on '{MoonrakerOperatingSystems.MainsailOS}!");
#endif
                    /*
                    KlipperDatabaseFluiddValueUiSettings fluiddObject = GetObjectFromJson<KlipperDatabaseFluiddValueUiSettings>(resultString);
                    if (fluiddObject?.General != null)
                    {
                        resultObject = new()
                        {
                            Locale = fluiddObject.General.Locale,
                            Printername = fluiddObject.General.InstanceName,
                        };
                    }
                    break;
                    */
                    default:
                        break;
                }

                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValueRemotePrinter),
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

        public async Task RefreshRemotePrintersAsync()
        {
            try
            {
                List<KlipperDatabaseRemotePrinter> result = await GetRemotePrintersAsync().ConfigureAwait(false);
                Printers = new(result ?? new());
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Printers = new();
            }
        }

        public async Task<List<KlipperDatabaseTemperaturePreset>> GetDashboardPresetsAsync()
        {
            string resultString = string.Empty;
            List<KlipperDatabaseTemperaturePreset> resultObject = new();
            try
            {
                string currentNamespace = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                string currentKey = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "presets" : "uiSettings";

                Dictionary<string, object> result = await GetDatabaseItemAsync(currentNamespace, currentKey).ConfigureAwait(false);
                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (string.IsNullOrEmpty(pair?.Key)) return resultObject;

                resultString = pair.Value.Value.ToString();

                switch (OperatingSystem)
                {
                    case MoonrakerOperatingSystems.MainsailOS:
                        // New since latest update
                        KlipperDatabaseMainsailValuePresets mainsailObject = GetObjectFromJson<KlipperDatabaseMainsailValuePresets>(resultString, NewtonsoftJsonSerializerSettings);
                        //List<KlipperDatabaseMainsailValuePreset> mainsailObject = GetObjectFromJson<List<KlipperDatabaseMainsailValuePreset>>(resultString);
                        if (mainsailObject != null)
                        {
                            IEnumerable<KlipperDatabaseTemperaturePreset> temp = mainsailObject.Presets.Select((item, index) => new KlipperDatabaseTemperaturePreset()
                            {
                                //Id = Guid.NewGuid(),
                                Id = item.Key,
                                Name = item.Value.Name,
                                Gcode = item.Value.Gcode,
                                Values = new(item.Value.Values.Select(valuePair => new KeyValuePair<string, KlipperDatabaseTemperaturePresetHeater>(
                                    valuePair.Key, new KlipperDatabaseTemperaturePresetHeater()
                                    {
                                        Name = valuePair.Key,
                                        Active = valuePair.Value.Bool,
                                        Type = valuePair.Value.Type,
                                        Value = valuePair.Value.Value,
                                    }))),
                            });
                            resultObject = new(temp);
                        }
                        break;
                    case MoonrakerOperatingSystems.FluiddPi:
                        //resultString = pair.Value.ToString();
                        KlipperDatabaseFluiddValueUiSettings fluiddObject = GetObjectFromJson<KlipperDatabaseFluiddValueUiSettings>(resultString);
                        if (fluiddObject?.Dashboard?.TempPresets != null)
                        {
                            IEnumerable<KlipperDatabaseTemperaturePreset> temp = fluiddObject.Dashboard.TempPresets.Select(item => new KlipperDatabaseTemperaturePreset()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Gcode = item.Gcode,
                                Values = new(item.Values.Select(valuePair => new KeyValuePair<string, KlipperDatabaseTemperaturePresetHeater>(
                                    valuePair.Key, new KlipperDatabaseTemperaturePresetHeater()
                                    {
                                        Name = valuePair.Key,
                                        Active = valuePair.Value.Active,
                                        Type = valuePair.Value.Type,
                                        Value = valuePair.Value.Value,
                                    }))),
                            });
                            resultObject = new(temp);
                        }
                        break;
                    default:
                        break;
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValuePreset),
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
        public async Task RefreshDashboardPresetsAsync()
        {
            try
            {
                // Could be null if no presets have been defined yet
                List<KlipperDatabaseTemperaturePreset> result = await GetDashboardPresetsAsync().ConfigureAwait(false);
                Presets = result ?? new();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Presets = new();
            }
        }

        public async Task<KlipperDatabaseMainsailValueHeightmapSettings> GetMeshHeightMapSettingsAsync()
        {
            string resultString = string.Empty;
            KlipperDatabaseMainsailValueHeightmapSettings resultObject = null;
            try
            {
                if (OperatingSystem != MoonrakerOperatingSystems.MainsailOS)
                {
                    throw new NotSupportedException($"The method '{nameof(GetMeshHeightMapSettingsAsync)}() is only support on '{MoonrakerOperatingSystems.MainsailOS}!");
                }

                Dictionary<string, object> result = await GetDatabaseItemAsync("mainsail", "heightmap").ConfigureAwait(false);
                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (string.IsNullOrEmpty(pair?.Key)) return resultObject;

                //resultString = pair.Value.ToString();
                resultString = pair.Value.Value.ToString();

                resultObject = GetObjectFromJson<KlipperDatabaseMainsailValueHeightmapSettings>(resultString);
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValueHeightmapSettings),
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

        public async Task<Dictionary<string, object>> AddDatabaseItemAsync(string namespaceName, string key, object value)
        {
            IRestApiRequestRespone result = null;
            Dictionary<string, object> resultObject = new();
            try
            {
                object cmd = new
                {
                    @namespace = namespaceName,
                    key = key,
                    value = value,
                };
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "database/item",
                       jsonObject: cmd,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, "database/item", cmd).ConfigureAwait(false);
                KlipperDatabaseItemRespone queryResult = GetObjectFromJson<KlipperDatabaseItemRespone>(result.Result, NewtonsoftJsonSerializerSettings);
                if (queryResult != null)
                {
                    resultObject = new()
                    {
                        { $"{namespaceName}{(!string.IsNullOrEmpty(key) ? $"|{key}" : "")}", queryResult?.Result?.Value }
                    };
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDatabaseItemRespone),
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

        public async Task<Dictionary<string, object>> DeleteDatabaseItemAsync(string namespaceName, string key)
        {
            IRestApiRequestRespone result = null;
            Dictionary<string, object> resultObject = new();
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "namespace", namespaceName }
                };
                if (!string.IsNullOrEmpty(key)) urlSegments.Add("key", key);

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: "database/item",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"database/item", default, null, urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperDatabaseItemRespone queryResult = GetObjectFromJson<KlipperDatabaseItemRespone>(result.Result, NewtonsoftJsonSerializerSettings);
                if (queryResult != null)
                {
                    resultObject = new()
                    {
                        { $"{namespaceName}{(!string.IsNullOrEmpty(key) ? $"|{key}" : "")}", queryResult?.Result?.Value }
                    };
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDatabaseItemRespone),
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
    }
}
