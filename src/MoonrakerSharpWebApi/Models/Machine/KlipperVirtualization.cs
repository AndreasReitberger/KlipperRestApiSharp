using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVirtualization
    {
        #region Properties
        [JsonProperty("virt_type")]
        public string VirtType { get; set; }

        [JsonProperty("virt_identifier")]
        public string VirtIdentifier { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
