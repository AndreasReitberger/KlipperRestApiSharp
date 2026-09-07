namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueDashboardExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("feedamount")]
        public partial long Feedamount { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseMainsailValueDashboardExtruder);
        #endregion
    }
}
