namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("count")]
        public partial long Count { get; set; }

        [ObservableProperty]

        [JsonPropertyName("nozzleDiameter")]
        public partial double NozzleDiameter { get; set; }

        [ObservableProperty]

        [JsonPropertyName("offsets")]
        public partial long[][] Offsets { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("sharedNozzle")]
        public partial bool SharedNozzle { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
