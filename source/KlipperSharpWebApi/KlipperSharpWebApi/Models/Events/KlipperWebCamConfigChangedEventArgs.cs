using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public class KlipperWebCamConfigChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public List<KlipperDatabaseMainsailValueWebcamConfig> NewConfig { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
