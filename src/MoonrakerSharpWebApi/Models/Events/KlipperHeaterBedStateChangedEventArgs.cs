using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperHeaterBedStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperStatusHeaterBed NewHeaterBedState { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperHeaterBedStateChangedEventArgs);
        #endregion
    }
}
