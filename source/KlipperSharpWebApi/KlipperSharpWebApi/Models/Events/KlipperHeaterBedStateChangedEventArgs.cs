using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperHeaterBedStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusHeaterBed HeaterBedState { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
