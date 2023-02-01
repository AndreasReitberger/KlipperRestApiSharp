using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSdInfo
    {
        #region Properties
        [JsonProperty("manufacturer_id")]
        public string ManufacturerId { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("oem_id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long OemId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("product_revision")]
        public string ProductRevision { get; set; }

        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }

        [JsonProperty("manufacturer_date")]
        public string ManufacturerDate { get; set; }

        [JsonProperty("capacity")]
        public string Capacity { get; set; }

        [JsonProperty("total_bytes")]
        public long TotalBytes { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
