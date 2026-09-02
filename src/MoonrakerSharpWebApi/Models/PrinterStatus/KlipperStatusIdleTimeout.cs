using AndreasReitberger.API.Moonraker.Enum;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusIdleTimeout : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("state")]
        //[field: JsonConverter(typeof(StringEnumConverter), true)]
        public partial KlipperIdleStates State { get; set; }

        [ObservableProperty]
        [JsonPropertyName("printing_time")]
        public partial double? PrintingTime { get; set; }

        [ObservableProperty]
        public partial bool ValidState { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
