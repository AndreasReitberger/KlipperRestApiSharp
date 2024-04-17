using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSdInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("manufacturer_id")]
        string manufacturerId = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("manufacturer")]
        string manufacturer = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("oem_id")]
        long oemId;

        [ObservableProperty]
        [property: JsonProperty("product_name")]
        string productName = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("product_revision")]
        string productRevision = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("serial_number")]
        string serialNumber = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("manufacturer_date")]
        string manufacturerDate = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("capacity")]
        string capacity = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("total_bytes")]
        long totalBytes;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
