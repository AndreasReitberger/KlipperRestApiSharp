using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperProfile
    {
        #region Properties
        public string Name { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
