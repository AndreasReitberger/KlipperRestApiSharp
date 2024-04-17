using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMacro
    {
        #region Properties
        [JsonIgnore]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("rename_existing")]
        public string RenameExisting { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("gcode")]
        public string Gcode { get; set; } = string.Empty;

        [JsonProperty("variable_extrude", NullValueHandling = NullValueHandling.Ignore)]
        public string VariableExtrude { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
