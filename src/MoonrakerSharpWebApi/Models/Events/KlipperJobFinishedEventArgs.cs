using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use `JobFinishedEventArgs` instead")]
    public class KlipperJobFinishedEventArgs : KlipperEventArgs
    {
        #region Properties
        //public IPrint3dJobStatus Job { get; set; }
        public KlipperStatusJob Job { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
