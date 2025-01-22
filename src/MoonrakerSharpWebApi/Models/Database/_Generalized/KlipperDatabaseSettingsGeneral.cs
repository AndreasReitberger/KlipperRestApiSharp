using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseSettingsGeneral : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Printername { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool DisplayCancelPrint { get; set; }

        [ObservableProperty]
        public partial string Locale { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
