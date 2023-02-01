using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.Events
{
    public class ClientsChangedDatabaseEventArgs : DatabaseEventArgs
    {
        #region Properties
        public List<MoonrakerClient> Clients { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
