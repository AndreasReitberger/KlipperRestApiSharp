using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseTemperaturePresetHeater : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool Active { get; set; }

        [ObservableProperty]
        public partial long? Value { get; set; }

        [ObservableProperty]
        public partial string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
