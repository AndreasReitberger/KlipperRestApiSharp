using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDevice
    {
        #region Properties
        [JsonProperty("device")]
        public string Device { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("locked_while_printing")]
        public bool LockedWhilePrinting { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
