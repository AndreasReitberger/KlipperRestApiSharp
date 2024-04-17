using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigHeaterBed : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("control")]
        string control= string.Empty;

        [ObservableProperty]
        [property: JsonProperty("pid_kp")]
        double pidKp;

        [ObservableProperty]
        [property: JsonProperty("pullup_resistor")]
        long pullupResistor;

        [ObservableProperty]
        [property: JsonProperty("sensor_pin")]
        string sensorPin= string.Empty;

        [ObservableProperty]
        [property: JsonProperty("heater_pin")]
        string heaterPin= string.Empty;

        [ObservableProperty]
        [property: JsonProperty("max_power")]
        long maxPower;

        [ObservableProperty]
        [property: JsonProperty("min_extrude_temp")]
        long minExtrudeTemp;

        [ObservableProperty]
        [property: JsonProperty("sensor_type")]
        string sensorType= string.Empty;

        [ObservableProperty]
        [property: JsonProperty("inline_resistor")]
        long inlineResistor;

        [ObservableProperty]
        [property: JsonProperty("pid_kd")]
        double pidKd;

        [ObservableProperty]
        [property: JsonProperty("pwm_cycle_time")]
        double pwmCycleTime;

        [ObservableProperty]
        [property: JsonProperty("pid_ki")]
        double pidKi;

        [ObservableProperty]
        [property: JsonProperty("min_temp")]
        long minTemp;

        [ObservableProperty]
        [property: JsonProperty("max_temp")]
        long maxTemp;

        [ObservableProperty]
        [property: JsonProperty("smooth_time")]
        long smoothTime;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
