using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperIsPrintingProgressChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public double NewPrintProgress { get; set; }
        public double PreviousPrintProgress { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperIsPrintingProgressChangedEventArgs);
        #endregion
    }
}
