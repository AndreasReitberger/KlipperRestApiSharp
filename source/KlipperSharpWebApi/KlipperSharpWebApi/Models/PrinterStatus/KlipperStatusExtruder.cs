using AndreasReitberger.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusExtruder
    {
        #region Properties
        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("target")]
        public double Target { get; set; }

        [JsonProperty("power")]
        public double Power { get; set; }

        [JsonProperty("pressure_advance")]
        public double PressureAdvance { get; set; }

        [JsonProperty("smooth_time")]
        public double SmoothTime { get; set; }

        [JsonProperty("square_corner_velocity")]
        public double SquareCornerVelocity { get; set; }

        [JsonProperty("max_accel")]
        public double MaxAccel { get; set; }

        [JsonProperty("homed_axes")]
        public string HomedAxes { get; set; }

        [JsonProperty("estimated_print_time")]
        public double EstimatedPrintTime { get; set; }

        [JsonProperty("max_velocity")]
        public double MaxVelocity { get; set; }

        [JsonProperty("print_time")]
        public double PrintTime { get; set; }

        [JsonProperty("max_accel_to_decel")]
        public double MaxAccelToDecel { get; set; }

        [JsonProperty("axis_minimum")]
        public List<double> AxisMinimum { get; set; } = new();

        [JsonProperty("stalls")]
        public double Stalls { get; set; }

        [JsonProperty("axis_maximum")]
        public List<double> AxisMaximum { get; set; } = new();

        [JsonProperty("position")]
        public List<double> Position { get; set; } = new();

        [JsonProperty("extruder")]
        public string Extruder { get; set; }

        [JsonIgnore]
        public bool CanUpdateTarget { get; set; } = false;

        [JsonIgnore]
        public KlipperToolState State { get => GetCurrentState(); }
        #endregion

        #region Methods
        KlipperToolState GetCurrentState()
        {
            try
            {
                return Target <= 0
                    ? KlipperToolState.Idle
                    : Target > Temperature && Math.Abs(Target - Temperature) > 2 ? KlipperToolState.Heating : KlipperToolState.Ready;
            }
            catch (Exception)
            {
                return KlipperToolState.Error;
            }

        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
