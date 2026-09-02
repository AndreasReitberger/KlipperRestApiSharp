namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueDashboard : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("extruder")]
        public partial KlipperDatabaseMainsailValueDashboardExtruder? Extruder { get; set; }

        [ObservableProperty]
        [JsonPropertyName("control")]
        public partial KlipperDatabaseMainsailValueDashboardControl? Control { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseMainsailValueDashboard);
        #endregion
    }
}
