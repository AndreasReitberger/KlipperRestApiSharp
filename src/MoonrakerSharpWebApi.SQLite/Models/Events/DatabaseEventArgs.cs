using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.Events
{
    public class DatabaseEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; } = string.Empty;
        public TimeSpan? Duration { get; set; } = TimeSpan.FromSeconds(0);
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
