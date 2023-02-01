using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperCurrentPrintImageChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public byte[] NewImage { get; set; }
        public byte[] PreviousImage { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
