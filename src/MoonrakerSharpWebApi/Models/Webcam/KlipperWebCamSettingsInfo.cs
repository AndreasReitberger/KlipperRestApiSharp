using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebCamSettingsInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        Guid id = Guid.Empty;

        [ObservableProperty]
        bool isDefault = false;

        [ObservableProperty]
        bool autostart = false;

        [ObservableProperty]
        string name = string.Empty;

        [ObservableProperty]
        Guid serverId = Guid.Empty;

        [ObservableProperty]
        int camIndex = -1;

        [ObservableProperty]
        int rotationAngle = 0;

        [ObservableProperty]
        int networkBufferTime = 150;

        [ObservableProperty]
        int fileCachingTime = 1000;

        [ObservableProperty]
        bool overwriteWebCamUri = false;

        [ObservableProperty]
        string webcamUri = string.Empty;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
