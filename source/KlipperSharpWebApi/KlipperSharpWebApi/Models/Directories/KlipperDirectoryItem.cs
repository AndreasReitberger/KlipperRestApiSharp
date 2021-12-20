using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperDirectoryItem
    {
        #region Properties
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
