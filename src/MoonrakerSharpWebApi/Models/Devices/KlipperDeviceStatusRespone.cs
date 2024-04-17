using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDeviceStatusRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("result")]
        Dictionary<string, string> deviceStates = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
