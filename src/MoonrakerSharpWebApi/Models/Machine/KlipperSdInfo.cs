namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSdInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("manufacturer_id")]
        public partial string ManufacturerId { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("manufacturer")]
        public partial string Manufacturer { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("oem_id")]
        public partial long OemId { get; set; }

        [ObservableProperty]

        [JsonPropertyName("product_name")]
        public partial string ProductName { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("product_revision")]
        public partial string ProductRevision { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("serial_number")]
        public partial string SerialNumber { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("manufacturer_date")]
        public partial string ManufacturerDate { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("capacity")]
        public partial string Capacity { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("total_bytes")]
        public partial long TotalBytes { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperSdInfo);
        #endregion
    }
}
