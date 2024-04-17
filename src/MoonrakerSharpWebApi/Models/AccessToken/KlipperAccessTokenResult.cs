using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperAccessTokenResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        //[JsonProperty("result")]
        string result = string.Empty;
        #endregion

        #region Overrides

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
