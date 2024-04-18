using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryInfoResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("dirs")]
        public List<KlipperDirectory> dirs = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("files")]
        public List<KlipperFile> files = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("disk_usage")]
        public KlipperDiskUsage? diskUsage;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("root_info")]
        public KlipperRootInfo? rootInfo;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
