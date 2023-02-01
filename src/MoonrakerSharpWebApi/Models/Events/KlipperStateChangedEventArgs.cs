using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public string NewState { get; set; }
        public string PreviousState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
