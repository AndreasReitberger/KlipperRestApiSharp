using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Print3dServer.Core;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.JSON;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {

#if DEBUG
        #region Debug

        [ObservableProperty]
        JsonSerializerSettings jsonSerializerSettings = DefaultJsonSerializerSettings;

        public new static JsonSerializerSettings DefaultJsonSerializerSettings = new()
        {
            // Detect if the json respone has more or less properties than the target class
            //MissingMemberHandling = MissingMemberHandling.Error,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            TypeNameHandling = TypeNameHandling.Auto,
            Converters =
            {
                // Map the converters
                new AbstractConverter<KlipperGcodeMetaResult, IGcodeMeta>(),
                new AbstractConverter<KlipperGcodeThumbnail, IGcodeImage>(),
                new AbstractConverter<KlipperJobQueueItem, IPrint3dJob>(),
                new AbstractConverter<AuthenticationHeader, IAuthenticationHeader>(),
            }
        };
        #endregion
#else
        #region Release
        public new static JsonSerializerSettings DefaultJsonSerializerSettings = new()
        {
            // Ignore if the json respone has more or less properties than the target class
            MissingMemberHandling = MissingMemberHandling.Ignore,          
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            Converters =
            {
                // Map the converters
                new AbstractConverter<KlipperGcodeMetaResult, IGcodeMeta>(),
            }
        };
        #endregion
#endif
    }
}
