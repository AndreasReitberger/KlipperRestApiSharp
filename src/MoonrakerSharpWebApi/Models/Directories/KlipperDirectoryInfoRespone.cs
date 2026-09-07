namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryInfoRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperDirectoryInfoResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDirectoryInfoRespone);
        #endregion
    }
}
