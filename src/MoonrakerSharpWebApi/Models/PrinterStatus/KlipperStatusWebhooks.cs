using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusWebhooks : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("state")]
        string state = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("state_message")]
        string stateMessage = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
