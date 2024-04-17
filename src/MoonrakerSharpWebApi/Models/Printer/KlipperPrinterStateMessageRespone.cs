using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStateMessageRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperPrinterStateMessageResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
