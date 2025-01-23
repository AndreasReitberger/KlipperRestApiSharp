using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusProbe : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("last_z_result")]
        public partial double? LastZResult { get; set; }

        [ObservableProperty]

        [JsonProperty("last_query")]
        public partial bool LastQuery { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
