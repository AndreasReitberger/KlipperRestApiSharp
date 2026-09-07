using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperPrinterInfoChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperPrinterStateMessageResult? NewPrinterInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperPrinterInfoChangedEventArgs);
        #endregion
    }
}
