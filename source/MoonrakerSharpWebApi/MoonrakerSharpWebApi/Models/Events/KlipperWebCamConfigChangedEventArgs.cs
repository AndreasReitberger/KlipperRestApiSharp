using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperWebCamConfigChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public List<KlipperDatabaseWebcamConfig> NewConfig { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
