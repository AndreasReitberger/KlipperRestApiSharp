using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
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
