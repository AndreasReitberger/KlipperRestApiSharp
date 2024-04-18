using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusHeaters : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("available_sensors")]
        List<string> availableSensors = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("available_heaters")]
        List<string> availableHeaters = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
