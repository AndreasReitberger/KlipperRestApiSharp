using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperConfigVirtualSdcard
    {
        #region Properties
        [JsonProperty("path")]
        public string Path { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
