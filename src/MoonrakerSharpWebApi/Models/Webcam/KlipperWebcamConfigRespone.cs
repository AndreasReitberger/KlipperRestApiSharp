using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebcamConfigRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("result")]
        KlipperWebcamConfigResult? result;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion
    }
}
