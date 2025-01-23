using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusSubscriptionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("status")]
        public partial KlipperPrinterStatusSubscriptionStatus? Status { get; set; }

        [ObservableProperty]

        [JsonProperty("eventtime")]
        public partial double? Eventtime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
