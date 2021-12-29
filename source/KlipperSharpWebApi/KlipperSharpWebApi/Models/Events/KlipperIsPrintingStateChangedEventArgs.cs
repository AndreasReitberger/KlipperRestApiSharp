using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperIsPrintingStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public bool IsPrinting { get; set; }
        public bool IsPaused { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
