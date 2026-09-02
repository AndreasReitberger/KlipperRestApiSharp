using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterState : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("text")]
        public partial string Text { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("flags")]
        public partial Dictionary<string, bool> Flags { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
