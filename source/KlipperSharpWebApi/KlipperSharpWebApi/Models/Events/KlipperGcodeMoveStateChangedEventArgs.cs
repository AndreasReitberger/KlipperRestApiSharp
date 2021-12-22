using Newtonsoft.Json;

namespace AndreasReitberger.Models
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
