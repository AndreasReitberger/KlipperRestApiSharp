using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectory : ObservableObject, IGcodeGroup
    {
        #region Properties
        [ObservableProperty]
        [JsonProperty("dirname")]
        string directoryName;
        partial void OnDirectoryNameChanged(string value)
        {
            if (string.IsNullOrEmpty(Name))
                Name = value;
        }

        [ObservableProperty]
        [JsonProperty("path")]
        string path;

        [ObservableProperty]
        [JsonProperty("root")]
        string root;

        [ObservableProperty]
        [JsonProperty("modified")]
        double modified;

        [ObservableProperty]
        [JsonProperty("size")]
        long size;

        [ObservableProperty]
        [JsonProperty("permissions")]
        string permissions;

        #region JsonIgnore
        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        string name;
        #endregion

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
