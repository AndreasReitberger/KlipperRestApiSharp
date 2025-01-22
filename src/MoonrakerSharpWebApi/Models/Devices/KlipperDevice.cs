using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDevice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("device")]
        public partial string Device { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("status")]
        public partial string Status { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("locked_while_printing")]
        public partial bool LockedWhilePrinting { get; set; }

        [ObservableProperty]
        
        [JsonProperty("type")]
        public partial string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
