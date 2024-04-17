using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
#if ConcurrentDictionary
using System.Collections.Concurrent;
#else
using System.Collections.Generic;
#endif

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperCpuUsageChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
#if ConcurrentDictionary
        public ConcurrentDictionary<string, double?> CpuUsage { get; set; } = new();
#else
        public Dictionary<string, double?> CpuUsage { get; set; } = new();
#endif
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
