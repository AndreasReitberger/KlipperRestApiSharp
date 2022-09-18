using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcam
    {
        #region Properties
        [JsonProperty("configs")]
        public List<KlipperDatabaseMainsailValueWebcamConfig> Configs { get; set; }

        [JsonProperty("boolNavi")]
        public bool BoolNavi { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
