using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use `Print3dBaseEventArgs` instead")]
    public class KlipperEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; }
        public string Printer { get; set; }
        public long CallbackId { get; set; }
        public string SessonId { get; set; }
        public string Token { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
