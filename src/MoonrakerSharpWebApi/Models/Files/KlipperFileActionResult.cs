using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileActionResult
    {
        #region Properties
        [JsonProperty("item")]
        public KlipperFileItem Item { get; set; }

        [JsonProperty("print_started")]
        public bool PrintStarted { get; set; }

        [JsonProperty("print_queued")]
        public bool PrintQueued { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
