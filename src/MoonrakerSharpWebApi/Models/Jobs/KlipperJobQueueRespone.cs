namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobQueueRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial KlipperJobQueueResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperJobQueueRespone);
        #endregion
    }
}
