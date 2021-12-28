using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusDisplay
    {
        #region Properties
        [JsonIgnore]
        public int PercentageProgress => GetPercentageProgress();

        [JsonProperty("progress")]
        public double Progress { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
        #endregion

        #region Methods
        int GetPercentageProgress()
        {
            try
            {
                if (Progress <= 0) return 0;
                int calc = Convert.ToInt32(Progress * 100);
                return calc;
            }
            catch (Exception)
            {
                return 0;
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
