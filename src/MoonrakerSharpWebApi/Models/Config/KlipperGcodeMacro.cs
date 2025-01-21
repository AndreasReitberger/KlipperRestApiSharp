using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMacro : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonIgnore]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("rename_existing")]
        public partial string RenameExisting { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("description")]
        public partial string Description { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("gcode")]
        public partial string Gcode { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("variable_extrude", NullValueHandling = NullValueHandling.Ignore)]
        public partial string VariableExtrude { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
