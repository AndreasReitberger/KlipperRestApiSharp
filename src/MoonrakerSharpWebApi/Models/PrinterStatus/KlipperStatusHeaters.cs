using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusHeaters : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("available_sensors")]
        List<string> availableSensors = new();

        [ObservableProperty]
        [property: JsonProperty("available_heaters")]
        List<string> availableHeaters = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
