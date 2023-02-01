using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperGcodeMoveStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusGcodeMove NewState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
