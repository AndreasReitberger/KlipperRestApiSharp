namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperActionListRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperActionListResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperActionListRespone);
        #endregion
    }
}
