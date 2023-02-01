using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperDatabaseRemotePrinter
    {
        #region Properties
        public string Hostname { get; set; }

        public long Port { get; set; }

        public long WebPort { get; set; }

        public object Settings { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
