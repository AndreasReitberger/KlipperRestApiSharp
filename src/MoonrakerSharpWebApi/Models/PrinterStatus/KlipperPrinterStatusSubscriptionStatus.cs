using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusSubscriptionStatus : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("objects")]
        public partial Dictionary<string, string> Objects { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperPrinterStatusSubscriptionStatus);
        #endregion
    }
}
