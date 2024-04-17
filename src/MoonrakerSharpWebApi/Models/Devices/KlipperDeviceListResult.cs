using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDeviceListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("devices")]
        List<KlipperDevice> devices = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
