using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodesResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("gcode_store")]
        public partial List<KlipperGcode> Gcodes { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodesResult);
        #endregion
    }
}
