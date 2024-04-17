using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectory : ObservableObject, IGcodeGroup
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("dirname")]
        string directoryName = string.Empty;
        partial void OnDirectoryNameChanged(string value)
        {
            if (string.IsNullOrEmpty(Name))
                Name = value;
        }

        [ObservableProperty]
        [property: JsonProperty("path")]
        string path = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("root")]
        string root = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("modified")]
        double modified;

        [ObservableProperty]
        [property: JsonProperty("size")]
        long size;

        [ObservableProperty]
        [property: JsonProperty("permissions")]
        string permissions = string.Empty;

        #region JsonIgnore
        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        string name = string.Empty;
        #endregion

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
