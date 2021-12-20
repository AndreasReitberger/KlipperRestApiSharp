using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperServerConfigChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperServerConfig NewConfiguration { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
