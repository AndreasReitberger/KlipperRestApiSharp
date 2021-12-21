using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperFilamentSensorsRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperFilamentSensorsResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
