namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerConfigResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("config")]
        public partial KlipperServerConfig? Config { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperServerConfigResult);
        
        #endregion
    }
}
