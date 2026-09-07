namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEndstopQueryRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperEndstopQueryResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperEndstopQueryRespone);
        #endregion
    }
}
