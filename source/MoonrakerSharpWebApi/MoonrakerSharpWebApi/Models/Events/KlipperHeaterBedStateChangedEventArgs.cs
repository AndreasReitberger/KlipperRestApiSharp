using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperHeaterBedStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusHeaterBed NewHeaterBedState { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
