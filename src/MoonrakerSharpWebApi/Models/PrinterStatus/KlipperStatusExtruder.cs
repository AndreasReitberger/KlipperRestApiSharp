using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusExtruder : ObservableObject, IToolhead
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [JsonProperty("temperature")]
        public double? TempRead { get; set; }

        [JsonProperty("target")]
        public double? TempSet { get; set; }

        [JsonProperty("power")]
        public double? Power { get; set; }

        [JsonProperty("pressure_advance")]
        public double? PressureAdvance { get; set; }

        [JsonProperty("smooth_time")]
        public double? SmoothTime { get; set; }

        [JsonProperty("square_corner_velocity")]
        public double? SquareCornerVelocity { get; set; }

        [JsonProperty("max_accel")]
        public double? MaxAccel { get; set; }

        [JsonProperty("homed_axes")]
        public string HomedAxes { get; set; }

        [JsonProperty("estimated_print_time")]
        public double? EstimatedPrintTime { get; set; }

        [JsonProperty("max_velocity")]
        public double? MaxVelocity { get; set; }

        [JsonProperty("print_time")]
        public double? PrintTime { get; set; }

        [JsonProperty("max_accel_to_decel")]
        public double? MaxAccelToDecel { get; set; }

        [JsonProperty("axis_minimum")]
        public List<double> AxisMinimum { get; set; } = new();

        [JsonProperty("stalls")]
        public double? Stalls { get; set; }

        [JsonProperty("axis_maximum")]
        public List<double> AxisMaximum { get; set; } = new();

        [JsonProperty("position")]
        public List<double> Position { get; set; } = new();

        [JsonProperty("extruder")]
        public string Name { get; set; }

        [JsonIgnore]
        public bool CanUpdateTarget { get; set; } = false;

        [JsonIgnore]
        public KlipperToolState State { get => GetCurrentState(); }

        [ObservableProperty]
        Printer3dHeaterType type = Printer3dHeaterType.Other;

        #region Interface, unused
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double x = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double y = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double z = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long error;

        #endregion

        #endregion

        #region Methods
        KlipperToolState GetCurrentState()
        {
            try
            {
                if (TempSet == null || TempRead == null)
                    return KlipperToolState.Idle;
                return TempSet <= 0
                    ? KlipperToolState.Idle
                    : TempSet > TempRead && Math.Abs(Convert.ToDouble(TempSet - TempRead)) > 2 ? KlipperToolState.Heating : KlipperToolState.Ready;
            }
            catch (Exception)
            {
                return KlipperToolState.Error;
            }

        }
        public Task<bool> SetTemperatureAsync(IPrint3dServerClient client, string command, object? data) => client.SetExtruderTemperatureAsync(command, data);
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion
    }
}
