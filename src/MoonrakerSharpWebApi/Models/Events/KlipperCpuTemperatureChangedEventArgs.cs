using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperCpuTemperatureChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public double NewTemperature { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperCpuTemperatureChangedEventArgs);
        #endregion
    }
}
