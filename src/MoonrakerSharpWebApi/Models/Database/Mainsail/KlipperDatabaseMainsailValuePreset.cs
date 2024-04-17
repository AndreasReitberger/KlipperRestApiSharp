using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValuePreset
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("gcode")]
        public string Gcode { get; set; } = string.Empty;

        [JsonProperty("values")]
        public Dictionary<string, KlipperDatabaseMainsailHeaterElement> Values { get; set; } = [];
        //public KlipperDatabaseMainsailValuePresetValues Values { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
