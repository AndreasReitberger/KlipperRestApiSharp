using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusSubscriptionRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("result")]
        public partial KlipperPrinterStatusSubscriptionResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
