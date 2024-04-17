using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinter : BaseModel
    {
        #region Properties
        [JsonIgnore]
        bool _isOnline = true;
        [JsonIgnore]
        public bool IsOnline
        {
            get { return _isOnline; }
            set { SetProperty(ref _isOnline, value); }
        }
        [JsonIgnore]
        double? _extruder1 = 0;
        [JsonIgnore]
        public double? Extruder1
        {
            get { return _extruder1; }
            set { SetProperty(ref _extruder1, value); }
        }

        [JsonIgnore]
        double? _extruder2 = 0;
        [JsonIgnore]
        public double? Extruder2
        {
            get { return _extruder2; }
            set { SetProperty(ref _extruder2, value); }
        }

        [JsonIgnore]
        double? _heatedBed = 0;
        [JsonIgnore]
        public double? HeatedBed
        {
            get { return _heatedBed; }
            set { SetProperty(ref _heatedBed, value); }
        }

        [JsonIgnore]
        double? _chamber = 0;
        [JsonIgnore]
        public double? Chamber
        {
            get { return _chamber; }
            set { SetProperty(ref _chamber, value); }
        }

        [JsonIgnore]
        double _progress = 0;
        [JsonIgnore]
        public double Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        [JsonIgnore]
        double _remainingPrintTime = 0;
        [JsonIgnore]
        public double RemainingPrintTime
        {
            get { return _remainingPrintTime; }
            set { SetProperty(ref _remainingPrintTime, value); }
        }

        [JsonIgnore]
        string _job = string.Empty;
        [JsonIgnore]
        public string Job
        {
            get { return _job; }
            set { SetProperty(ref _job, value); }
        }

        [JsonIgnore]
        bool _isPrinting = false;
        [JsonIgnore]
        public bool IsPrinting
        {
            get { return _isPrinting; }
            set { SetProperty(ref _isPrinting, value); }
        }
        [JsonIgnore]
        bool _isPaused = false;
        [JsonIgnore]
        public bool IsPaused
        {
            get { return _isPaused; }
            set { SetProperty(ref _isPaused, value); }
        }
        [JsonIgnore]
        bool _isSelected = false;
        [JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        [JsonProperty("axes")]
        public OctoprintApiPrinterAxes? Axes { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; } = string.Empty;

        [JsonProperty("current")]
        public bool Current { get; set; }

        [JsonProperty("default")]
        public bool DefaultDefault { get; set; }

        [JsonProperty("extruder")]
        public OctoprintApiPrinterExtruder? Extruder { get; set; }

        [JsonProperty("heatedBed")]
        public bool HasHeatedBed { get; set; }

        [JsonProperty("heatedChamber")]
        public bool HasHeatedChamber { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("resource")]
        public Uri? Resource { get; set; }

        [JsonProperty("volume")]
        public OctoprintApiPrinterVolume? Volume { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
