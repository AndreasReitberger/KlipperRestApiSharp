using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusDriver : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("cs_actual")]
        public partial long? CsActual { get; set; }

        [ObservableProperty]
        
        [JsonProperty("sg_result")]
        public partial long? SgResult { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
