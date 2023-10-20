using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        [JsonProperty("width")]
        long width;

        [ObservableProperty]
        [JsonProperty("height")]
        long height;

        [ObservableProperty]
        [JsonProperty("size")]
        long size;

        [ObservableProperty]
        [JsonProperty("relative_path")]
        string path;
        partial void OnPathChanged(string value)
        {
            IsPathRelative = value is not null;
        }

        [ObservableProperty]
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
