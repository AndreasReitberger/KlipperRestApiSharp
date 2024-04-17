using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSdInfo
    {
        #region Properties
        [JsonProperty("manufacturer_id")]
        public string ManufacturerId { get; set; } = string.Empty;

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;

        [JsonProperty("oem_id")]
        public long OemId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; } = string.Empty;

        [JsonProperty("product_revision")]
        public string ProductRevision { get; set; } = string.Empty;

        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonProperty("manufacturer_date")]
        public string ManufacturerDate { get; set; } = string.Empty;

        [JsonProperty("capacity")]
        public string Capacity { get; set; } = string.Empty;

        [JsonProperty("total_bytes")]
        public long TotalBytes { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
