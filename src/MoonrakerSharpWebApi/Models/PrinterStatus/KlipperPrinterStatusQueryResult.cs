using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusQueryResult : ObservableObject
    {
        // Maybe delete later?
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("status")]
        KlipperPrinterStatus? status;

        [ObservableProperty]
        [property: JsonProperty("eventtime")]
        double eventtime;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
