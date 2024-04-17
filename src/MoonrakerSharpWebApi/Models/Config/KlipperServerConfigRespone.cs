using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerConfigRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("result")]
        KlipperServerConfigResult? result;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
