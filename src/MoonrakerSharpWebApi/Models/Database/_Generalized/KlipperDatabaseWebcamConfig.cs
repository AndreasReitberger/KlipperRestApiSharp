using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseWebcamConfig : ObservableObject, IWebCamConfig
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        public Guid id = Guid.Empty;

        [ObservableProperty, JsonIgnore]
        public bool enabled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        public string alias = string.Empty;

        [ObservableProperty, JsonIgnore]
        public string icon = string.Empty;

        [ObservableProperty, JsonIgnore]
        public string service = string.Empty;

        [ObservableProperty, JsonIgnore]
        public long targetFps;

        [ObservableProperty, JsonIgnore]
        public string url = string.Empty;
        partial void OnUrlChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri? result))
                WebCamUrlDynamic = result;
        }

        [ObservableProperty, JsonIgnore]
        public Uri? webCamUrlDynamic;
        
        [ObservableProperty, JsonIgnore]
        public string urlSnapshot = string.Empty;
        partial void OnUrlSnapshotChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri? result))
                WebCamUrlStatic = result;
        }

        [ObservableProperty, JsonIgnore]
        public Uri? webCamUrlStatic;

        [ObservableProperty, JsonIgnore]
        public bool flipX;

        [ObservableProperty, JsonIgnore]
        public bool flipY;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rotation")]
        public long orientation;

        [ObservableProperty, JsonIgnore]
        public long position;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
