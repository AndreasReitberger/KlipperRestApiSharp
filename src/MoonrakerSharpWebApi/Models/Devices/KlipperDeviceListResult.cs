using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDeviceListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("devices")]
        public partial List<KlipperDevice> Devices { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
