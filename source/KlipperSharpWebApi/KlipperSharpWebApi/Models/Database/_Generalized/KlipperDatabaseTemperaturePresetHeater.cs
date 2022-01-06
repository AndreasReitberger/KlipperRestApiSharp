using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperDatabaseTemperaturePresetHeater
    {
        #region Properties
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; }

        public long? Value { get; set; }

        public string Type { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
