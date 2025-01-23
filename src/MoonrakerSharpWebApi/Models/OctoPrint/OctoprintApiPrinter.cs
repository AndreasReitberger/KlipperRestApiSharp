using Newtonsoft.Json;
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

        [JsonProperty("axes")]
        public partial OctoprintApiPrinterAxes? Axes { get; set; }

        [ObservableProperty]

        [JsonProperty("color")]
        public partial string Color { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("current")]
        public partial bool Current { get; set; }

        [ObservableProperty]

        [JsonProperty("default")]
        public partial bool DefaultDefault { get; set; }

        [ObservableProperty]

        [JsonProperty("extruder")]
        public partial OctoprintApiPrinterExtruder? Extruder { get; set; }

        [ObservableProperty]

        [JsonProperty("heatedBed")]
        public partial bool HasHeatedBed { get; set; }

        [ObservableProperty]

        [JsonProperty("heatedChamber")]
        public partial bool HasHeatedChamber { get; set; }

        [ObservableProperty]

        [JsonProperty("id")]
        public partial string Id { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("model")]
        public partial string Model { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("resource")]
        public partial Uri? Resource { get; set; }

        [ObservableProperty]

        [JsonProperty("volume")]
        public partial OctoprintApiPrinterVolume? VVolume { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
