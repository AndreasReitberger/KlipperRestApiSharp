using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusDriverRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("drv_status")]
        public partial KlipperStatusDriver? DrvStatus { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
