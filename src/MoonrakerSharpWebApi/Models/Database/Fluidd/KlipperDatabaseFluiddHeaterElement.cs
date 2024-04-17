using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddHeaterElement
    {
        #region Properties
        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("value")]
        public long? Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
