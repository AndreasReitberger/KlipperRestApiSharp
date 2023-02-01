using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperJobListStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public string NewJobListStatus { get; set; } = string.Empty;
        public string PreviousJobListStatus { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
