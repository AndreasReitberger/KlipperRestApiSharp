using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        public partial bool IsOnline { get; set; } = true;

        [ObservableProperty]

        public partial double? Extruder1 { get; set; } = 0;

        [ObservableProperty]

        public partial double? Extruder2 { get; set; } = 0;

        [ObservableProperty]

        public partial double? HeatedBed { get; set; } = 0;

        [ObservableProperty]

        public partial double? Chamber { get; set; } = 0;

        [ObservableProperty]

        public partial double Progress { get; set; } = 0;

        [ObservableProperty]

        public partial double RemainingPrintTime { get; set; } = 0;

        [ObservableProperty]

        public partial string Job { get; set; } = string.Empty;

        [ObservableProperty]

        public partial bool IsPrinting { get; set; } = false;

        [ObservableProperty]

        public partial bool IsPaused { get; set; } = false;

        [ObservableProperty]

        public partial bool IsSelected { get; set; } = false;

        [ObservableProperty]

        [JsonPropertyName("axes")]
        public partial OctoprintApiPrinterAxes? Axes { get; set; }

        [ObservableProperty]

        [JsonPropertyName("color")]
        public partial string Color { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("current")]
        public partial bool Current { get; set; }

        [ObservableProperty]

        [JsonPropertyName("default")]
        public partial bool DefaultDefault { get; set; }

        [ObservableProperty]

        [JsonPropertyName("extruder")]
        public partial OctoprintApiPrinterExtruder? Extruder { get; set; }

        [ObservableProperty]

        [JsonPropertyName("heatedBed")]
        public partial bool HasHeatedBed { get; set; }

        [ObservableProperty]

        [JsonPropertyName("heatedChamber")]
        public partial bool HasHeatedChamber { get; set; }

        [ObservableProperty]

        [JsonPropertyName("id")]
        public partial string Id { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("model")]
        public partial string Model { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("resource")]
        public partial Uri? Resource { get; set; }

        [ObservableProperty]

        [JsonPropertyName("volume")]
        public partial OctoprintApiPrinterVolume? VVolume { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.OctoprintApiPrinter);
        #endregion
    }
}
