using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperIpAddress
    {
        #region Properties
        [JsonProperty("family")]
        public string Family { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("is_link_local")]
        public bool IsLinkLocal { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
