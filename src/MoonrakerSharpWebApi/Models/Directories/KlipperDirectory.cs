using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectory : ObservableObject, IGcodeGroup
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("dirname")]
        public partial string DirectoryName { get; set; } = string.Empty;

        partial void OnDirectoryNameChanged(string value)
        {
            if (string.IsNullOrEmpty(Name))
                Name = value;
        }

        [ObservableProperty]
        
        [JsonProperty("path")]
        public partial string Path { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("root")]
        public partial string Root { get; set; } = string.Empty;

        [ObservableProperty]
        
        [NotifyPropertyChangedFor(nameof(ModifiedGeneralized))]
        [JsonProperty("modified")]
        public partial double? Modified { get; set; }

        partial void OnModifiedChanged(double? value)
        {
            if (value is not null)
                ModifiedGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty]
        public partial DateTime? ModifiedGeneralized { get; set; }

        [ObservableProperty]
        
        [JsonProperty("size")]
        public partial long Size { get; set; }

        [ObservableProperty]
        
        [JsonProperty("permissions")]
        public partial string Permissions { get; set; } = string.Empty;

        #region JsonIgnore
        [ObservableProperty]
        
        public partial Guid Id { get; set; }

        [ObservableProperty]
        
        public partial string Name { get; set; } = string.Empty;

        #endregion

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
