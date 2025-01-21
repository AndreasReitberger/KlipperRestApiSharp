using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateTemperatureInfo : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        
        [JsonProperty("actual", NullValueHandling = NullValueHandling.Ignore)]
        public partial double Actual { get; set; }

        [ObservableProperty]
        
        [JsonProperty("target")]
        public partial long Target { get; set; }

        [ObservableProperty]
        
        [JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
        public partial long Offset { get; set; }
        [JsonIgnore]
        public OctoprintApiCurrentToolState State { get => GetCurrentState(); }

        #endregion

        #region Static
        public static OctoprintApiPrinterStateTemperatureInfo Default = new()
        {
            Actual = 0,
            Target = 0,
            Offset = 0,
        };
        #endregion

        #region Methods
        OctoprintApiCurrentToolState GetCurrentState()
        {
            if (Target < 0 || Actual < 0)
                return OctoprintApiCurrentToolState.Error;
            else
            {
                if (Target <= 0)
                    return OctoprintApiCurrentToolState.Idle;
                // Check if temperature is reached with a hysteresis
                else if (Target > Actual && Math.Abs(Target - Actual) > 2)
                    return OctoprintApiCurrentToolState.Heating;
                else
                    return OctoprintApiCurrentToolState.Ready;
            }
        }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
