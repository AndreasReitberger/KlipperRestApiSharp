using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperLoginEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public string UserName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string UserToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperLoginEventArgs);
        #endregion
    }
}
