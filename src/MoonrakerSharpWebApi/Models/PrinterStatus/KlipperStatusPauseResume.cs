using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusPauseResume : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("is_paused")]
        public partial bool IsPaused { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
