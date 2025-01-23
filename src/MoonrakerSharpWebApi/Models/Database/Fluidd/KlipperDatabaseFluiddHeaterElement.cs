using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddHeaterElement : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("active")]
        public partial bool Active { get; set; }

        [ObservableProperty]

        [JsonProperty("value")]
        public partial long? Value { get; set; }

        [ObservableProperty]

        [JsonProperty("type")]
        public partial string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
