using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperExtruderHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("temperatures")]
        public partial List<double> Temperatures { get; set; } = new();

        [ObservableProperty]
        
        [JsonProperty("targets")]
        public partial List<long> Targets { get; set; } = new();

        [ObservableProperty]
        
        [JsonProperty("powers")]
        public partial List<long> Powers { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
