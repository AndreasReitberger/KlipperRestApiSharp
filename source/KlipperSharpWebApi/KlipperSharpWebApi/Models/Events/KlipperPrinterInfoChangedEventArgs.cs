using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperPrinterInfoChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperPrinterStateMessageResult NewPrinterInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
