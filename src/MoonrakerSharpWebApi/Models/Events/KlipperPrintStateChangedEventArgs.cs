using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperPrintStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperStatusPrintStats? NewPrintState { get; set; }
        public KlipperStatusPrintStats? PreviousPrintState { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperPrintStateChangedEventArgs);
        #endregion
    }
}
