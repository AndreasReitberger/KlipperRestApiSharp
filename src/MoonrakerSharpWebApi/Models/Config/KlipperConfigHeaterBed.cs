using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigHeaterBed : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("control")]
        string control= string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pid_kp")]
        double pidKp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pullup_resistor")]
        long pullupResistor;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sensor_pin")]
        string sensorPin= string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heater_pin")]
        string heaterPin= string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_power")]
        long maxPower;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("min_extrude_temp")]
        long minExtrudeTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sensor_type")]
        string sensorType= string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("inline_resistor")]
        long inlineResistor;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pid_kd")]
        double pidKd;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pwm_cycle_time")]
        double pwmCycleTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pid_ki")]
        double pidKi;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("min_temp")]
        long minTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_temp")]
        long maxTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("smooth_time")]
        long smoothTime;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
