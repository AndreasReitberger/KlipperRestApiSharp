using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryInfoResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("dirs")]
        public partial List<KlipperDirectory> Dirs { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("files")]
        public partial List<KlipperFile> Files { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("disk_usage")]
        public partial KlipperDiskUsage? DiskUsage { get; set; }

        [ObservableProperty]

        [JsonPropertyName("root_info")]
        public partial KlipperRootInfo? RootInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDirectoryInfoResult);
        #endregion
    }
}
