using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusTemperatureSensor
    {
        #region Properties
        [JsonProperty("temperature")]
        public double? Temperature { get; set; }

        [JsonProperty("measured_max_temp")]
        public double? MeasuredMaxTemperature { get; set; }

        [JsonProperty("measured_min_temp")]
        public double? MeasuredMinTemperature { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
