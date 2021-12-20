using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperApiRequestRespone
    {
        #region Properties
        public string Result { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = false;
        public bool Succeeded { get; set; } = false;
        public bool HasAuthenticationError { get; set; } = false;

        public KlipperRestEventArgs EventArgs { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
