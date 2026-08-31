using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("cameras")]
        public partial List<KlipperDatabaseFluiddValueWebcamConfig> Cameras { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
