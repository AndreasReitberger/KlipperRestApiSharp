using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigMcu : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("baud")]
        public partial long Baud { get; set; }

        [ObservableProperty]

        [JsonProperty("serial")]
        public partial string Serial { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("max_stepper_error")]
        public partial double MaxStepperError { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
