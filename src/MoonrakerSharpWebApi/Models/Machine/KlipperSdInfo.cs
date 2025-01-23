using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSdInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("manufacturer_id")]
        public partial string ManufacturerId { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("manufacturer")]
        public partial string Manufacturer { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("oem_id")]
        public partial long OemId { get; set; }

        [ObservableProperty]

        [JsonProperty("product_name")]
        public partial string ProductName { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("product_revision")]
        public partial string ProductRevision { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("serial_number")]
        public partial string SerialNumber { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("manufacturer_date")]
        public partial string ManufacturerDate { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("capacity")]
        public partial string Capacity { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("total_bytes")]
        public partial long TotalBytes { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
