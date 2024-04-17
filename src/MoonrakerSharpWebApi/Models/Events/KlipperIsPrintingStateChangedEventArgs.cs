using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperIsPrintingStateChangedEventArgs : Print3dBaseEventArgs
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
