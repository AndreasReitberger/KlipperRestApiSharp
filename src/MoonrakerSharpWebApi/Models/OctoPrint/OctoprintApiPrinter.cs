using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        bool isOnline = true;

        [ObservableProperty, JsonIgnore]
        double? extruder1 = 0;

        [ObservableProperty, JsonIgnore]
        double? extruder2 = 0;

        [ObservableProperty, JsonIgnore]
        double? heatedBed = 0;

        [ObservableProperty, JsonIgnore]
        double? chamber = 0;

        [ObservableProperty, JsonIgnore]
        double progress = 0;

        [ObservableProperty, JsonIgnore]
        double remainingPrintTime = 0;

        [ObservableProperty, JsonIgnore]
        string job = string.Empty;

        [ObservableProperty, JsonIgnore]
        bool isPrinting = false;

        [ObservableProperty, JsonIgnore]
        bool isPaused = false;

        [ObservableProperty, JsonIgnore]
        bool isSelected = false;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("axes")]
        OctoprintApiPrinterAxes? axes;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("color")]
        string color = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("current")]
        bool current;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("default")]
        bool defaultDefault;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder")]
        OctoprintApiPrinterExtruder? extruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heatedBed")]
        bool hasHeatedBed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heatedChamber")]
        bool hasHeatedChamber;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("id")]
        string id = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("model")]
        string model = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("resource")]
        Uri? resource;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("volume")]
        OctoprintApiPrinterVolume? vVolume;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
