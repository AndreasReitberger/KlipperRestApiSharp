using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusDriverRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("drv_status")]
        KlipperStatusDriver? drvStatus;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
