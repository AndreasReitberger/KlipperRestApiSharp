using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateCommitsBehind : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("sha")]
        public partial string Sha { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("author")]
        public partial string Author { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("date")]
        public partial long Date { get; set; }

        [ObservableProperty]
        
        [JsonProperty("subject")]
        public partial string Subject { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("message")]
        public partial string Message { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("tag")]
        public partial object? Tag { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
