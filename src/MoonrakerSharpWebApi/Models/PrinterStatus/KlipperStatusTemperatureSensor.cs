using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusTemperatureSensor : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("temperature")]
        public partial double? Temperature { get; set; }

        [ObservableProperty]
        
        [JsonProperty("measured_max_temp")]
        public partial double? MeasuredMaxTemperature { get; set; }

        [ObservableProperty]
        
        [JsonProperty("measured_min_temp")]
        public partial double? MeasuredMinTemperature { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
