using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseWebcamConfig : ObservableObject, IWebCamConfig
    {
        #region Properties
        [ObservableProperty]

        public partial Guid Id { get; set; } = Guid.Empty;

        [ObservableProperty]

        public partial bool Enabled { get; set; }

        [ObservableProperty]

        [JsonProperty("name")]
        public partial string Alias { get; set; } = string.Empty;

        [ObservableProperty]

        public partial string Icon { get; set; } = string.Empty;

        [ObservableProperty]

        public partial string Service { get; set; } = string.Empty;

        [ObservableProperty]

        public partial long TargetFps { get; set; }

        [ObservableProperty]

        public partial string Url { get; set; } = string.Empty;

        partial void OnUrlChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri? result))
                WebCamUrlDynamic = result;
        }

        [ObservableProperty]

        public partial Uri? WebCamUrlDynamic { get; set; }

        [ObservableProperty]

        public partial string UrlSnapshot { get; set; } = string.Empty;

        partial void OnUrlSnapshotChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri? result))
                WebCamUrlStatic = result;
        }

        [ObservableProperty]

        public partial Uri? WebCamUrlStatic { get; set; }

        [ObservableProperty]

        public partial bool FlipX { get; set; }

        [ObservableProperty]

        public partial bool FlipY { get; set; }

        [ObservableProperty]

        [JsonProperty("rotation")]
        public partial long Orientation { get; set; }

        [ObservableProperty]

        public partial long Position { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
