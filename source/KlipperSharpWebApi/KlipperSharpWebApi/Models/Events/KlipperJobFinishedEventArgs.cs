using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperJobFinishedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusJob Job { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
