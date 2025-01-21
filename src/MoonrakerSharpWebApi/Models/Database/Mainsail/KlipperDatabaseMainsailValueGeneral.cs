using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueGeneral : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("printername")]
        public partial string Printername { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("displayCancelPrint")]
        public partial bool DisplayCancelPrint { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
