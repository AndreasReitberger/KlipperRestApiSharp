using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileItem
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
