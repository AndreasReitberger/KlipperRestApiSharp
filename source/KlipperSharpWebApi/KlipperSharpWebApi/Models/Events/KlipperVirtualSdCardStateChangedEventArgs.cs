using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperVirtualSdCardStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusVirtualSdcard NewState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
