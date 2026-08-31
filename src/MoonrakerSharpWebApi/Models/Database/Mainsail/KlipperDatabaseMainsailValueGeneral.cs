using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueGeneral : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("printername")]
        public partial string Printername { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("displayCancelPrint")]
        public partial bool DisplayCancelPrint { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
