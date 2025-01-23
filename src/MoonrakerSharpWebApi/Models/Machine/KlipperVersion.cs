using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVersion : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("major")]
        public partial long Major { get; set; }

        [ObservableProperty]

        [JsonProperty("minor")]
        public partial string Minor { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("build_number")]
        public partial string BuildNumber { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
