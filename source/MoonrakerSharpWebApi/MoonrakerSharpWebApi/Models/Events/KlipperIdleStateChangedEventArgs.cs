using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperIdleStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusIdleTimeout NewState { get; set; }
        public KlipperStatusIdleTimeout PreviousState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
