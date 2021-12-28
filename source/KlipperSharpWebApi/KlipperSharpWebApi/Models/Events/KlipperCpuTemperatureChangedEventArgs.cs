using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperCpuTemperatureChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public double NewTemperature { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
