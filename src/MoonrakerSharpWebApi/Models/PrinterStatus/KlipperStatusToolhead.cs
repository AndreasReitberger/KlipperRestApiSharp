using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusToolhead : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("square_corner_velocity")]
        double? squareCornerVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_accel")]
        double? maxAccel;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("homed_axes")]
        string homedAxes = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("estimated_print_time")]
        double? estimatedPrintTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_velocity")]
        double? maxVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("print_time")]
        double? printTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_accel_to_decel")]
        double? maxAccelToDecel;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("axis_minimum")]
        List<double> axisMinimum = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("stalls")]
        double? stalls;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("axis_maximum")]
        List<double> axisMaximum = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("position")]
        List<double> position = [];

        [ObservableProperty, JsonIgnore]
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
