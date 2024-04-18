using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeThumbnail : ObservableObject, IGcodeImage
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("width")]
        long width;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("height")]
        long height;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("size")]
        long size;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("relative_path")]
        string path = string.Empty;
        partial void OnPathChanged(string value)
        {
            IsPathRelative = value is not null;
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        bool isPathRelative = true;
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
