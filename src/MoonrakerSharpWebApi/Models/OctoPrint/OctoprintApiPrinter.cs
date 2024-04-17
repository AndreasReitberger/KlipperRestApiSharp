using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        bool isOnline = true;

        [ObservableProperty]
        double? extruder1 = 0;

        [ObservableProperty]
        double? extruder2 = 0;

        [ObservableProperty]
        double? heatedBed = 0;

        [ObservableProperty]
        double? chamber = 0;

        [ObservableProperty]
        double progress = 0;

        [ObservableProperty]
        double remainingPrintTime = 0;

        [ObservableProperty]
        string job = string.Empty;

        [ObservableProperty]
        bool isPrinting = false;

        [ObservableProperty]
        bool isPaused = false;

        [ObservableProperty]
        bool isSelected = false;

        [ObservableProperty]
        [property: JsonProperty("axes")]
        OctoprintApiPrinterAxes? axes;

        [ObservableProperty]
        [property: JsonProperty("color")]
        string color = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("current")]
        bool current;

        [ObservableProperty]
        [property: JsonProperty("default")]
        bool defaultDefault;

        [ObservableProperty]
        [property: JsonProperty("extruder")]
        OctoprintApiPrinterExtruder? extruder;

        [ObservableProperty]
        [property: JsonProperty("heatedBed")]
        bool hasHeatedBed;

        [ObservableProperty]
        [property: JsonProperty("heatedChamber")]
        bool hasHeatedChamber;

        [ObservableProperty]
        [property: JsonProperty("id")]
        string id = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("model")]
        string model = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("resource")]
        Uri? resource;

        [ObservableProperty]
        [property: JsonProperty("volume")]
        OctoprintApiPrinterVolume? vVolume;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
