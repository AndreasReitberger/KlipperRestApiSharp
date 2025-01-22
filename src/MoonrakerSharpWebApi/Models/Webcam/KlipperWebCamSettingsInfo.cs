using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebCamSettingsInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial Guid Id { get; set; } = Guid.Empty;

        [ObservableProperty]
        public partial bool IsDefault { get; set; } = false;

        [ObservableProperty]
        public partial bool Autostart { get; set; } = false;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Guid ServerId { get; set; } = Guid.Empty;

        [ObservableProperty]
        public partial int CamIndex { get; set; } = -1;

        [ObservableProperty]
        public partial int RotationAngle { get; set; } = 0;

        [ObservableProperty]
        public partial int NetworkBufferTime { get; set; } = 150;

        [ObservableProperty]
        public partial int FileCachingTime { get; set; } = 1000;

        [ObservableProperty]
        public partial bool OverwriteWebCamUri { get; set; } = false;

        [ObservableProperty]
        public partial string WebcamUri { get; set; } = string.Empty;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
