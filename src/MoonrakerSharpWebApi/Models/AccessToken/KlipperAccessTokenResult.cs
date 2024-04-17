using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperAccessTokenResult
    {
        #region Properties
        [JsonProperty("result")]
        public string Result { get; set; } = string.Empty;
        #endregion

        #region Overrides

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
