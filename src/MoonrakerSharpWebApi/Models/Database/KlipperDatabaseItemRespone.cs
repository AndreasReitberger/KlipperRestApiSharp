namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseItemRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("result")]
        public partial KlipperDatabaseItemResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseItemRespone);
        #endregion
    }
}
