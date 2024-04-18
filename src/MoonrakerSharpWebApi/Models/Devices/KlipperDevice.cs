using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDevice : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("device")]
        public string device = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("status")]
        public string status = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("locked_while_printing")]
        public bool lockedWhilePrinting;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("type")]
        public string type = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
