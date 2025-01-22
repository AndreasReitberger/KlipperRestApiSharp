using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDistribution : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("id")]
        public partial string Id { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("version")]
        public partial long Version { get; set; }

        [ObservableProperty]
        
        [JsonProperty("version_parts")]
        public partial KlipperVersion? KlipperVersion { get; set; }

        [ObservableProperty]
        
        [JsonProperty("like")]
        public partial string Like { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("codename")]
        public partial string Codename { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
