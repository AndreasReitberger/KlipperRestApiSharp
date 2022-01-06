using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseFluiddValueWebcam
    {
        #region Properties
        [JsonProperty("cameras")]
        public List<KlipperDatabaseFluiddValueWebcamConfig> Cameras { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
