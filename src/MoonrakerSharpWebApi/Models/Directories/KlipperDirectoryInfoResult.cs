using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryInfoResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("dirs")]
        public partial List<KlipperDirectory> Dirs { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("files")]
        public partial List<KlipperFile> Files { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("disk_usage")]
        public partial KlipperDiskUsage? DiskUsage { get; set; }

        [ObservableProperty]
        
        [JsonProperty("root_info")]
        public partial KlipperRootInfo? RootInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
