namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusQueryEndstops : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("last_query")]
        public partial KlipperEndstopQueryResult? LastQuery { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusQueryEndstops);
        #endregion
    }
}
