using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiFilamentInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
        public partial double Length { get; set; }

        [ObservableProperty]

        [JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        public partial double Volume { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
