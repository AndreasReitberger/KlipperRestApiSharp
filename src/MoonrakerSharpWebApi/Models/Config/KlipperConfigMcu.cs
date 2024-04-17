using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigMcu
    {
        #region Properties
        [JsonProperty("baud")]
        public long Baud { get; set; }

        [JsonProperty("serial")]
        public string Serial { get; set; } = string.Empty;

        [JsonProperty("max_stepper_error")]
        public double MaxStepperError { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
