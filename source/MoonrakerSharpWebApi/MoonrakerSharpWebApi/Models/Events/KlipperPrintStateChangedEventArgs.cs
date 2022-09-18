using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperPrintStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusPrintStats NewPrintState { get; set; }
        public KlipperStatusPrintStats PreviousPrintState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
