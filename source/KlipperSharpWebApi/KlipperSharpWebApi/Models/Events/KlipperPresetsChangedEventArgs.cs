using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public class KlipperPresetsChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public List<KlipperDatabaseMainsailValuePreset> NewPresets { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
