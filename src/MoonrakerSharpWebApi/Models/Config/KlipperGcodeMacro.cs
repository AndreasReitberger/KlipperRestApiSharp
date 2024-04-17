using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMacro : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonIgnore]
        [property: JsonIgnore]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("rename_existing")]
        string renameExisting = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("description")]
        string description = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("gcode")]
        string gcode = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("variable_extrude", NullValueHandling = NullValueHandling.Ignore)]
        string variableExtrude = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
