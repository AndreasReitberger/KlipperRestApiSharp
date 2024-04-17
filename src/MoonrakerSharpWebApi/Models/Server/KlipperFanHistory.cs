using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{

    public partial class KlipperFanHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("temperatures")]
        List<double> temperatures = new();

        [ObservableProperty]
        [property: JsonProperty("targets")]
        List<long> targets = new();

        [ObservableProperty]
        [property: JsonProperty("speeds")]
        List<long> speeds = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
