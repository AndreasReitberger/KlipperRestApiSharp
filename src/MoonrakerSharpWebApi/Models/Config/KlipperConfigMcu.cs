using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigMcu : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("baud")]
        long baud;

        [ObservableProperty]
        [property: JsonProperty("serial")]
        string serial = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("max_stepper_error")]
        double maxStepperError;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
