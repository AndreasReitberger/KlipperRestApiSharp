using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperGcodeCacheChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public List<KlipperGcode> CachedGcodes { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
