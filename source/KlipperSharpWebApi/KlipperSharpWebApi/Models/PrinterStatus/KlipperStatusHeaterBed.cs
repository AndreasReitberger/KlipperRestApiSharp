using AndreasReitberger.Enum;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusHeaterBed
    {
        #region Properties
        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("target")]
        public double Target { get; set; }

        [JsonProperty("power")]
        public double Power { get; set; }

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
