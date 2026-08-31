using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigMcu : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("baud")]
        public partial long Baud { get; set; }

        [ObservableProperty]
        [JsonPropertyName("serial")]
        public partial string Serial { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("max_stepper_error")]
        public partial double MaxStepperError { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
