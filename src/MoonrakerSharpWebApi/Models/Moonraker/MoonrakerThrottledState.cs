using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerThrottledState : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rx_bytes")]
        long rxBytes;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tx_bytes")]
        long txBytes;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bandwidth")]
        double bandwidth;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
