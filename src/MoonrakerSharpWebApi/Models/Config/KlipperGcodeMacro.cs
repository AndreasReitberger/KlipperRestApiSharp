using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMacro : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rename_existing")]
        string renameExisting = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("description")]
        string description = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode")]
        string gcode = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("variable_extrude", NullValueHandling = NullValueHandling.Ignore)]
        string variableExtrude = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
