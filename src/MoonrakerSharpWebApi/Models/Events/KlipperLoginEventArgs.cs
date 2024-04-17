using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperLoginEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public string UserName { get; set; }
        public string Action { get; set; }
        public string UserToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Succeeded { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
