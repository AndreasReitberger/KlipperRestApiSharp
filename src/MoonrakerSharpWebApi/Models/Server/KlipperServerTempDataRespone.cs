using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerTempDataRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("result")]
        public partial Dictionary<string, KlipperTemperatureSensorHistory> Result { get; set; } = [];

        //public KlipperServerTempData Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperServerTempDataRespone);
        #endregion
    }
}
