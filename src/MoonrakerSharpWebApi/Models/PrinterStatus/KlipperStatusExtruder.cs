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

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperature")]
        double? tempRead;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("target")]
        double? tempSet;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("power")]
        double? power;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pressure_advance")]
        double? pressureAdvance;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("smooth_time")]
        double? smoothTime;

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
        string name = string.Empty;

        [JsonIgnore]
        public bool CanUpdateTarget = false;

        [JsonIgnore]
        public KlipperToolState State { get => GetCurrentState(); }

        [ObservableProperty, JsonIgnore]
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
