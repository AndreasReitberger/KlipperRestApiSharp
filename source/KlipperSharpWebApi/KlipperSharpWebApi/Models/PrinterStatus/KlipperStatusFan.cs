using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusFan
    {
        #region Properties
        [JsonIgnore]
        public int Percent => GetPercentageSpeed();

        [JsonProperty("speed")]
        public double? Speed { get; set; }

        [JsonProperty("rpm")]
        public long? Rpm { get; set; }
        #endregion

        #region MyRegion
        int GetPercentageSpeed()
        {
            try
            {
                if (Speed == null || Speed <= 0) return 0;
                int calc = Convert.ToInt32(Speed * 100);
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
