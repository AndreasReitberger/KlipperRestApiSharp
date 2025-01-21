using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVirtualization : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("virt_type")]
        public partial string VirtType { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("virt_identifier")]
        public partial string VirtIdentifier { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
