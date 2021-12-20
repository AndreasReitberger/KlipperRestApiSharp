using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public class KlipprtJsonConvertEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; }
        public string OriginalString { get; set; }
        public Exception Exception { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
