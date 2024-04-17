using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHeaterBedHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("temperatures")]
        List<double> temperatures = new();

        [ObservableProperty]
        [property: JsonProperty("targets")]
        List<long> targets = new();

        [ObservableProperty]
        [property: JsonProperty("powers")]
        List<long> powers = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
