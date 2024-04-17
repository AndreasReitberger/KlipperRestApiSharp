using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryInfoRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("result")]
        KlipperDirectoryInfoResult? result;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
