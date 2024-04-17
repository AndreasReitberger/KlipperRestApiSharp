using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueGeneral
    {
        #region Properties
        [JsonProperty("printername")]
        public string Printername { get; set; } = string.Empty;

        [JsonProperty("displayCancelPrint")]
        public bool DisplayCancelPrint { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
