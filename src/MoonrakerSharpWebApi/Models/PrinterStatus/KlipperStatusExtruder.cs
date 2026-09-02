using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusExtruder : ObservableObject, IToolhead
    {
        #region Properties
        [ObservableProperty]

        [JsonIgnore]
        public partial Guid Id { get; set; }

        [ObservableProperty]

        [JsonPropertyName("temperature")]
        public partial double? TempRead { get; set; }

        [ObservableProperty]

        [JsonPropertyName("target")]
        public partial double? TempSet { get; set; }

        [ObservableProperty]

        [JsonPropertyName("power")]
        public partial double? Power { get; set; }

        [ObservableProperty]

        [JsonPropertyName("pressure_advance")]
        public partial double? PressureAdvance { get; set; }

        [ObservableProperty]

        [JsonPropertyName("smooth_time")]
        public partial double? SmoothTime { get; set; }

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
        public partial string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public bool CanUpdateTarget = false;

        [JsonIgnore]
        public Printer3dToolHeadState State { get => GetCurrentState(); }

        [ObservableProperty]

        public partial Printer3dHeaterType Type { get; set; } = Printer3dHeaterType.Other;

        #region Interface, unused
        [ObservableProperty]

        [JsonIgnore]
        public partial double X { get; set; } = 0;

        [ObservableProperty]

        [JsonIgnore]
        public partial double Y { get; set; } = 0;

        [ObservableProperty]

        [JsonIgnore]
        public partial double Z { get; set; } = 0;

        [ObservableProperty]

        [JsonIgnore]
        public partial long Error { get; set; }

        #endregion

        #endregion

        #region Methods
        public Printer3dToolHeadState GetCurrentState()
        {
            try
            {
                if (TempSet == null || TempRead == null)
                    return Printer3dToolHeadState.Idle;
                return TempSet <= 0
                    ? Printer3dToolHeadState.Idle
                    : TempSet > TempRead && Math.Abs(Convert.ToDouble(TempSet - TempRead)) > 2 ? Printer3dToolHeadState.Heating : Printer3dToolHeadState.Ready;
            }
            catch (Exception)
            {
                return Printer3dToolHeadState.Error;
            }

        }
        public Task<bool> SetTemperatureAsync(IPrint3dServerClient client, string command, object? data) => client.SetExtruderTemperatureAsync(command, data);
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);

        #endregion
    }
}
