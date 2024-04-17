using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseSettingsGeneral : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string printername = string.Empty;

        [ObservableProperty]
        bool displayCancelPrint;

        [ObservableProperty]
        string locale = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
