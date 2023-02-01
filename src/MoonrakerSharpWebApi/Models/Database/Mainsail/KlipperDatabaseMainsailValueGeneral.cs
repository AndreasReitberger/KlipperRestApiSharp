using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueGeneral
    {
        #region Properties
        [JsonProperty("printername")]
        public string Printername { get; set; }

        [JsonProperty("displayCancelPrint")]
        public bool DisplayCancelPrint { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
