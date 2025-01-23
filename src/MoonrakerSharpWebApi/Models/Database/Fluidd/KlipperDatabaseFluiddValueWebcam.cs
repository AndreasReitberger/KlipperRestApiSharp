using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("cameras")]
        public partial List<KlipperDatabaseFluiddValueWebcamConfig> Cameras { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
