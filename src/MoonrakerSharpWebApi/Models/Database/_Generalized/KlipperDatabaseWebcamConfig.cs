using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperDatabaseWebcamConfig
    {
        #region Properties
        public Guid Id { get; set; } = Guid.Empty;
        public bool Enabled { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Service { get; set; } = string.Empty;

        public long TargetFps { get; set; }

        public string Url { get; set; } = string.Empty;
        public string UrlSnapshot { get; set; } = string.Empty;

        public bool FlipX { get; set; }

        public bool FlipY { get; set; }
        public int? Rotation { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
