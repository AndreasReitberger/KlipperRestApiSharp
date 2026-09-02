namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseNamespaceListRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("result")]
        public partial KlipperDatabaseNamespaceListResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseNamespaceListRespone);
        #endregion
    }
}
