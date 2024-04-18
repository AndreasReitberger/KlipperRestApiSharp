using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperExtruderHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperatures")]
        List<double> temperatures = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("targets")]
        List<long> targets = new();

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("powers")]
        List<long> powers = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
