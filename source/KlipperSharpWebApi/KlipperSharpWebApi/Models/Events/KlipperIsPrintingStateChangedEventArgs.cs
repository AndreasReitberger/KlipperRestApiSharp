using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperIsPrintingStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public bool NewPrintState { get; set; }
        public bool PreviousPrintState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
