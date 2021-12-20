using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperConfigMcu
    {
        #region Properties
        [JsonProperty("baud")]
        public long Baud { get; set; }

        [JsonProperty("serial")]
        public string Serial { get; set; }

        [JsonProperty("max_stepper_error")]
        public double MaxStepperError { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
