using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusToolhead : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("square_corner_velocity")]
        public partial double? SquareCornerVelocity { get; set; }

        [ObservableProperty]

        [JsonPropertyName("max_accel")]
        public partial double? MaxAccel { get; set; }

        [ObservableProperty]

        [JsonPropertyName("homed_axes")]
        public partial string HomedAxes { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("estimated_print_time")]
        public partial double? EstimatedPrintTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("max_velocity")]
        public partial double? MaxVelocity { get; set; }

        [ObservableProperty]

        [JsonPropertyName("print_time")]
        public partial double? PrintTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("max_accel_to_decel")]
        public partial double? MaxAccelToDecel { get; set; }

        [ObservableProperty]

        [JsonPropertyName("axis_minimum")]
        public partial List<double> AxisMinimum { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("stalls")]
        public partial double? Stalls { get; set; }

        [ObservableProperty]

        [JsonPropertyName("axis_maximum")]
        public partial List<double> AxisMaximum { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("position")]
        public partial List<double> Position { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("extruder")]
        public partial string Extruder { get; set; } = string.Empty;
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
