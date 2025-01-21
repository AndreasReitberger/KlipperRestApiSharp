using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerMainsail : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("type")]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("repo")]
        public partial string Repo { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("path")]
        public partial string Path { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("persistent_files")]
        public partial object? PersistentFiles { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
