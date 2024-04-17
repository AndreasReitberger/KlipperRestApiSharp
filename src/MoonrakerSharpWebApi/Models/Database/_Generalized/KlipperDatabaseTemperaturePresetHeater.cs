using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseTemperaturePresetHeater : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string name= string.Empty;

        [ObservableProperty]
        bool active;

        [ObservableProperty]
        long? value;

        [ObservableProperty]
        string type = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
