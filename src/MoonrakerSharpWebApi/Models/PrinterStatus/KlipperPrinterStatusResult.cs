using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("status")]
        object? status;

        [ObservableProperty]
        [property: JsonProperty("eventtime")]
        double? eventtime;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
