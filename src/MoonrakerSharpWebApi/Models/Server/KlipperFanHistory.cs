using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{

    public partial class KlipperFanHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperatures")]
        List<double> temperatures = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("targets")]
        List<long> targets = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("speeds")]
        List<long> speeds = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
