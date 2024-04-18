using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectory : ObservableObject, IGcodeGroup
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("dirname")]
        string directoryName = string.Empty;
        partial void OnDirectoryNameChanged(string value)
        {
            if (string.IsNullOrEmpty(Name))
                Name = value;
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("path")]
        string path = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("root")]
        string root = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("modified")]
        double modified;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("size")]
        long size;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("permissions")]
        string permissions = string.Empty;

        #region JsonIgnore
        [ObservableProperty, JsonIgnore]
        Guid id;

        [ObservableProperty, JsonIgnore]
        string name = string.Empty;
        #endregion

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
