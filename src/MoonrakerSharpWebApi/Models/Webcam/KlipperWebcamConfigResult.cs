using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebcamConfigResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("webcams")]
        public partial List<KlipperDatabaseWebcamConfig> Webcams { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperWebcamConfigResult);

        #endregion
    }
}
