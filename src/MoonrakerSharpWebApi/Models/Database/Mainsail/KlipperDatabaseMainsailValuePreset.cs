using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValuePreset : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("gcode")]
        public partial string Gcode { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("values")]
        public partial Dictionary<string, KlipperDatabaseMainsailHeaterElement> Values { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
