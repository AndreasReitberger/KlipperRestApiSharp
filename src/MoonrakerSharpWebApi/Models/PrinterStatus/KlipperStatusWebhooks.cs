using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusWebhooks : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("state")]
        public partial string State { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("state_message")]
        public partial string StateMessage { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
