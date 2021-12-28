using Newtonsoft.Json;

namespace AndreasReitberger.Models
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
