namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDirectoryActionRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperDirectoryActionResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDirectoryActionRespone);
        #endregion
    }
}
