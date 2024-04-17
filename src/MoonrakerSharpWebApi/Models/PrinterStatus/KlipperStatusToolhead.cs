using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusToolhead : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("square_corner_velocity")]
        double? squareCornerVelocity;

        [ObservableProperty]
        [property: JsonProperty("max_accel")]
        double? maxAccel;

        [ObservableProperty]
        [property: JsonProperty("homed_axes")]
        string homedAxes = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("estimated_print_time")]
        double? estimatedPrintTime;

        [ObservableProperty]
        [property: JsonProperty("max_velocity")]
        double? maxVelocity;

        [ObservableProperty]
        [property: JsonProperty("print_time")]
        double? printTime;

        [ObservableProperty]
        [property: JsonProperty("max_accel_to_decel")]
        double? maxAccelToDecel;

        [ObservableProperty]
        [property: JsonProperty("axis_minimum")]
        List<double> axisMinimum = new();

        [ObservableProperty]
        [property: JsonProperty("stalls")]
        double? stalls;

        [ObservableProperty]
        [property: JsonProperty("axis_maximum")]
        List<double> axisMaximum = new();

        [ObservableProperty]
        [property: JsonProperty("position")]
        List<double> position = new();

        [ObservableProperty]
        [property: JsonProperty("extruder")]
        string extruder = string.Empty;
        #endregion

        #region Methods
        public Dictionary<string, bool> GetHomedAxisStates()
        {
            Dictionary<string, bool> state = new()
            {
                { "x", false },
                { "y", false },
                { "z", false },
            };
            if (!string.IsNullOrEmpty(HomedAxes))
            {
                for (int i = 0; i < HomedAxes.Length; i++)
                {
                    string current = HomedAxes[i].ToString();
                    if (state.ContainsKey(current))
                    {
                        state[current] = true;
                    }
                    else
                    {
                        state.Add(current, true);
                    }
                }
            }
            return state;
        }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
