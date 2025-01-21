using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusSubscriptionStatus : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("objects")]
        public partial Dictionary<string, string> Objects { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
