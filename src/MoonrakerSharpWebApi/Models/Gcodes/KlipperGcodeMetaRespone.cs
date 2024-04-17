using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMetaRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("result")]
        KlipperGcodeMetaResult? result;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
