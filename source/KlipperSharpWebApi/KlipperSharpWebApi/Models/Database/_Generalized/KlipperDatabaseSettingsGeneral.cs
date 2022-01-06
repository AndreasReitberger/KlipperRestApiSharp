using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperDatabaseSettingsGeneral
    {
        #region Properties
        public string Printername { get; set; } = string.Empty;

        public bool DisplayCancelPrint { get; set; }

        public string Locale { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
