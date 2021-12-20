using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperDirectoryInfoResult
    {
        #region Properties
        [JsonProperty("dirs")]
        public List<KlipperDirectoryItem> Dirs { get; set; }

        [JsonProperty("files")]
        public List<KlipperFile> Files { get; set; }

        [JsonProperty("disk_usage")]
        public KlipperDiskUsage DiskUsage { get; set; }

        [JsonProperty("root_info")]
        public KlipperRootInfo RootInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
