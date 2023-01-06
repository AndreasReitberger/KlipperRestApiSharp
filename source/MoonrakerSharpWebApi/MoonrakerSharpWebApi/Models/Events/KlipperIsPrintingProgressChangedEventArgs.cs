using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperIsPrintingProgressChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public double NewPrintProgress { get; set; }
        public double PreviousPrintProgress { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
