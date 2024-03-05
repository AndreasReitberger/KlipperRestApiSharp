using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseWebcamConfig : ObservableObject, IWebCamConfig
    {
        #region Properties
        [ObservableProperty]
        public Guid id = Guid.Empty;

        [ObservableProperty]
        public bool enabled;

        [ObservableProperty]
        [JsonProperty("name")]
        public string alias = string.Empty;

        [ObservableProperty]
        public string icon = string.Empty;

        [ObservableProperty]
        public string service = string.Empty;

        [ObservableProperty]
        public long targetFps;

        [ObservableProperty]
        public string url = string.Empty;
        partial void OnUrlChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri result))
                WebCamUrlDynamic = result;
        }

        [ObservableProperty]
        public Uri webCamUrlDynamic;
        
        [ObservableProperty]
        public string urlSnapshot = string.Empty;
        partial void OnUrlSnapshotChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri result))
                WebCamUrlStatic = result;
        }

        [ObservableProperty]
        public Uri webCamUrlStatic;

        [ObservableProperty]
        public bool flipX;

        [ObservableProperty]
        public bool flipY;

        [ObservableProperty]
        [JsonProperty("rotation")]
        public long orientation;

        [ObservableProperty]
        public long position;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
