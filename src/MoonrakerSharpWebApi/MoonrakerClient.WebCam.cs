using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Moonraker.Structs;
using AndreasReitberger.API.Print3dServer.Core.Events;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {

        #region Properties

        /*
        [ObservableProperty, Obsolete("Replaced by the base collection WebCams")]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        List<KlipperDatabaseWebcamConfig> webCamConfigs = new();
        partial void OnWebCamConfigsChanged(List<KlipperDatabaseWebcamConfig> value)
        {
            OnKlipperWebCamConfigChanged(new KlipperWebCamConfigChangedEventArgs()
            {
                NewConfig = value,
            });
        }
        */
        #endregion

        #region Methods

        public override Task<ObservableCollection<IWebCamConfig>> GetWebCamConfigsAsync() => GetWebCamSettingsAsync();

        public async Task<ObservableCollection<IWebCamConfig>> GetWebCamSettingsAsync()
        {
            string resultString = string.Empty;
            IRestApiRequestRespone result = null;
            ObservableCollection<IWebCamConfig> resultObject = new();
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "webcams/list",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                KlipperWebcamConfigRespone configs = GetObjectFromJson<KlipperWebcamConfigRespone>(result.Result, NewtonsoftJsonSerializerSettings);
                if (configs?.Result?.Webcams?.Count > 0)
                    return new(configs?.Result?.Webcams ?? new());
                else
                    // If nothing is returned, try to get it from the database directly.
                    return await GetWebCamSettingsFromDatabaseAsync().ConfigureAwait(false);
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

        public async Task<ObservableCollection<IWebCamConfig>> GetWebCamSettingsFromDatabaseAsync()
        {
            string resultString = string.Empty;
            ObservableCollection<IWebCamConfig> resultObject = new();
            try
            {
                // Both operating systems handles their datababase namespaces and keys differently....
                // @fluidd
                // It seems that the webcams setting are also stored in the namespace=webcams
                //string currentNameSpace = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "webcams";
                //string currentKey = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "webcam" : "";

                // Seems to be the write way for both, MainsailOS and Fluidd
                string currentNameSpace = "webcams";
                string currentKey = "";

                Dictionary<string, object> result = await GetDatabaseItemAsync(currentNameSpace, currentKey).ConfigureAwait(false);
                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (string.IsNullOrEmpty(pair?.Key)) return resultObject;

                //resultString = pair.Value.ToString();
                resultString = pair.Value.Value.ToString();

                switch (OperatingSystem)
                {
                    case MoonrakerOperatingSystems.MainsailOS:
                    case MoonrakerOperatingSystems.FluiddPi:
                        Dictionary<Guid, KlipperDatabaseFluiddValueWebcamConfig> fluiddObject = GetObjectFromJson<Dictionary<Guid, KlipperDatabaseFluiddValueWebcamConfig>>(resultString, NewtonsoftJsonSerializerSettings);
                        if (fluiddObject?.Count > 0)
                        {
                            IEnumerable<KlipperDatabaseWebcamConfig> temp = fluiddObject.Select(item => new KlipperDatabaseWebcamConfig()
                            {
                                Id = item.Key,
                                Enabled = item.Value.Enabled,
                                Alias = item.Value.Name,
                                FlipX = item.Value.FlipX,
                                FlipY = item.Value.FlipY,
                                Service = item.Value.Service,
                                TargetFps = item.Value.Fpstarget,
                                Url = item.Value.UrlStream,
                                UrlSnapshot = item.Value.UrlSnapshot,
                                Orientation = (long)item.Value.Rotation,
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
                    TargetType = nameof(KlipperDatabaseMainsailValueWebcam),
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

        public async Task RefreshWebCamConfigAsync()
        {
            try
            {
                ObservableCollection<IWebCamConfig> result = await GetWebCamSettingsAsync().ConfigureAwait(false);
                WebCams = result ?? new();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                WebCams = new();
            }
        }

        /*
        
        [Obsolete("Replaced by base method")]
        public string GetDefaultWebCamUriOld()
        {
            try
            {
                string token = !string.IsNullOrEmpty(ApiKey) ? ApiKey : UserToken;
                return $"{FullWebAddress}/webcam/?action=stream{(!string.IsNullOrEmpty(token) ? $"?t={token}" : "")}";
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return "";
            }
        }

        [Obsolete("Replaced by base method")]
        public async Task<string> GetWebCamUriAsyncOld(int index = 0, bool refreshWebCamConfig = false)
        {
            try
            {
                if (WebCamConfigs?.Count <= 0 || refreshWebCamConfig)
                {
                    await RefreshWebCamConfigAsync().ConfigureAwait(false);
                }
                KlipperDatabaseWebcamConfig config = null;
                if (WebCamConfigs?.Count > index)
                {
                    config = WebCamConfigs[index];
                }
                else if (WebCamConfigs.Count > 0)
                {
                    config = WebCamConfigs.FirstOrDefault();
                }
                // If nothing is found, try the default setup
                else
                {
                    config = new KlipperDatabaseWebcamConfig()
                    {
                        Id = Guid.NewGuid(),
                        Alias = "Default",
                        Enabled = true,
                        FlipX = false,
                        FlipY = false,
                        TargetFps = 15,
                        Url = "/webcam?action=stream"
                    };
                }
                string token = !string.IsNullOrEmpty(ApiKey) ? ApiKey : UserToken;
                return config == null ? GetDefaultWebCamUri() : $"{FullWebAddress}{config.Url}{(!string.IsNullOrEmpty(token) ? $"&t={token}" : "")}";
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return "";
            }
        }
        */

        #endregion
    }
}
