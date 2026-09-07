using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectory : ObservableObject, IGcodeGroup
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("dirname")]
        public partial string DirectoryName { get; set; } = string.Empty;

        partial void OnDirectoryNameChanged(string value)
        {
            if (string.IsNullOrEmpty(Name))
                Name = value;
        }

        [ObservableProperty]

        [JsonPropertyName("path")]
        public partial string Path { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("root")]
        public partial string Root { get; set; } = string.Empty;

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(ModifiedGeneralized))]
        [JsonPropertyName("modified")]
        public partial double? Modified { get; set; }

        partial void OnModifiedChanged(double? value)
        {
            if (value is not null)
                ModifiedGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty]
        public partial DateTime? ModifiedGeneralized { get; set; }

        [ObservableProperty]

        [JsonPropertyName("size")]
        public partial long Size { get; set; }

        [ObservableProperty]

        [JsonPropertyName("permissions")]
        public partial string Permissions { get; set; } = string.Empty;

        #region JsonIgnore
        [ObservableProperty]

        public partial Guid Id { get; set; }

        [ObservableProperty]

        public partial string Name { get; set; } = string.Empty;

        #endregion

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDirectory);
        #endregion
    }
}
