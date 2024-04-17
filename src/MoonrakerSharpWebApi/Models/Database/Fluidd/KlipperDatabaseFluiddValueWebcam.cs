using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcam
    {
        #region Properties
        [JsonProperty("cameras")]
        public List<KlipperDatabaseFluiddValueWebcamConfig> Cameras { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
