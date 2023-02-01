using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperGcodeMetaResultChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperGcodeMetaResult NewResult { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
