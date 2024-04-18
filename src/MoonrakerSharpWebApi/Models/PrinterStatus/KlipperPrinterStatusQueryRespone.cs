using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    // Maybe delete later?
    public partial class KlipperPrinterStatusQueryRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("result")]
        KlipperPrinterStatusQueryResult? result;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
