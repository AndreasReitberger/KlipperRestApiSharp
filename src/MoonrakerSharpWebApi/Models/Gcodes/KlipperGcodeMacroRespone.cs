using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMacroRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("result")]
        KlipperGcodeMacroResult? result;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
