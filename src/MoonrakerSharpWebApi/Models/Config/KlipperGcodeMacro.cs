namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMacro : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonIgnore]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("rename_existing")]
        public partial string RenameExisting { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("description")]
        public partial string Description { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("gcode")]
        public partial string Gcode { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("variable_extrude")]
        public partial string VariableExtrude { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodeMacro);
        #endregion
    }
}
