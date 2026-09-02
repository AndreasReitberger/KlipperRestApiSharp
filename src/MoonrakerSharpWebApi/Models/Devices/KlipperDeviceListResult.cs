using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDeviceListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("devices")]
        public partial List<KlipperDevice> Devices { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDeviceListResult);
        #endregion
    }
}
