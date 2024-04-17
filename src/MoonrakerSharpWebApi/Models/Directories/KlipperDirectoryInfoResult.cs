using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryInfoResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("dirs")]
        public List<KlipperDirectory> dirs = [];

        [ObservableProperty]
        [property: JsonProperty("files")]
        public List<KlipperFile> files = [];

        [ObservableProperty]
        [property: JsonProperty("disk_usage")]
        public KlipperDiskUsage? diskUsage;

        [ObservableProperty]
        [property: JsonProperty("root_info")]
        public KlipperRootInfo? rootInfo;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
