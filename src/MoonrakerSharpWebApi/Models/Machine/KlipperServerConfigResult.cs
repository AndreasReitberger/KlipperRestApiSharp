namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMachineInfoResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("system_info")]
        public partial KlipperMachineInfo? SystemInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
