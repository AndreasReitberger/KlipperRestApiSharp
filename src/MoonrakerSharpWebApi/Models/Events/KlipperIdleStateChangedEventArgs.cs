using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperIdleStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperStatusIdleTimeout? NewState { get; set; }
        public KlipperStatusIdleTimeout? PreviousState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
