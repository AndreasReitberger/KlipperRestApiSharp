using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.JSON.System;
using AndreasReitberger.API.REST;
using AndreasReitberger.API.REST.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {

#if DEBUG
        #region Debug

        [ObservableProperty]
        [property: Newtonsoft.Json.JsonIgnore, JsonIgnore, XmlIgnore]
        JsonSerializerOptions jsonSerializerSettings = DefaultJsonSerializerSettings;

        public new static JsonSerializerOptions DefaultJsonSerializerSettings = new()
        {
            // Detect if the json respone has more or less properties than the target class
            //MissingMemberHandling = MissingMemberHandling.Error,
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true,
            Converters =
            {                     
                // Map the converters
                new TypeMappingConverter<IAuthenticationHeader, AuthenticationHeader>(),
                new TypeMappingConverter<IGcodeMeta, KlipperGcodeMetaResult>(),
                new TypeMappingConverter<IGcodeImage, KlipperGcodeThumbnail>(),
                new TypeMappingConverter<IPrint3dJob, KlipperJobQueueItem>(),
                new TypeMappingConverter<IPrint3dJobStatus, KlipperStatusJob>(),
                new TypeMappingConverter<IWebCamConfig, KlipperDatabaseWebcamConfig>(),
                new TypeMappingConverter<IToolhead, KlipperStatusExtruder>(),
                new TypeMappingConverter<IPrint3dFan, KlipperStatusFan>(),
                new TypeMappingConverter<ISensorComponent, KlipperStatusFilamentSensor>(),
            }
        };
        #endregion
#else
        #region Release
        public new static JsonSerializerOptions DefaultJsonSerializerSettings = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true,
            Converters =
            {                     
                // Map the converters
                new TypeMappingConverter<IAuthenticationHeader, AuthenticationHeader>(),
                new TypeMappingConverter<IGcodeMeta, KlipperGcodeMetaResult>(),
                new TypeMappingConverter<IGcodeImage, KlipperGcodeThumbnail>(),
                new TypeMappingConverter<IPrint3dJob, KlipperJobQueueItem>(),
                new TypeMappingConverter<IPrint3dJobStatus, KlipperStatusJob>(),
                new TypeMappingConverter<IWebCamConfig, KlipperDatabaseWebcamConfig>(),
                new TypeMappingConverter<IToolhead, KlipperStatusExtruder>(),
                new TypeMappingConverter<IPrint3dFan, KlipperStatusFan>(),
                new TypeMappingConverter<ISensorComponent, KlipperStatusFilamentSensor>(),
            }
        };
        #endregion
#endif
    }
}
