using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperSessionChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperEventSession Sesson { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
