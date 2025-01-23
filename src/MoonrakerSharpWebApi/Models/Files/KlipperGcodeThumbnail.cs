using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeThumbnail : ObservableObject, IGcodeImage
    {
        #region Properties
        [ObservableProperty]

        [JsonIgnore]
        public partial Guid Id { get; set; }

        [ObservableProperty]

        [JsonProperty("width")]
        public partial long Width { get; set; }

        [ObservableProperty]

        [JsonProperty("height")]
        public partial long Height { get; set; }

        [ObservableProperty]

        [JsonProperty("size")]
        public partial long Size { get; set; }

        [ObservableProperty]

        [JsonProperty("relative_path")]
        public partial string Path { get; set; } = string.Empty;

        partial void OnPathChanged(string value)
        {
            IsPathRelative = value is not null;
        }

        [ObservableProperty]

        [JsonIgnore]
        public partial bool IsPathRelative { get; set; } = true;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            // Ordinarily, we release unmanaged resources here;
            // but all are wrapped by safe handles.

            // Release disposable objects.
            if (disposing)
            {
                // Nothing to do here
            }
        }
        #endregion

        #region Clone

        public object Clone() => MemberwiseClone();

        #endregion
    }
}
