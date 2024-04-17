using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueGeneral : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("printername")]
        public string printername = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("displayCancelPrint")]
        public bool displayCancelPrint;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
