using AndreasReitberger.API.REST.Enums;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Auth

        [ObservableProperty]
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public partial string UserToken { get; set; } = string.Empty;

        partial void OnUserTokenChanged(string value) => AddOrUpdateAuthHeader("Authorization", $"Bearer {value}", AuthenticationHeaderTarget.Header, 0);

        [ObservableProperty]
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public partial string RefreshToken { get; set; } = string.Empty;

        partial void OnRefreshTokenChanged(string value) => AddOrUpdateAuthHeader("Authorization", $"Bearer {value}", AuthenticationHeaderTarget.Header, 1);

        [ObservableProperty]
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public partial string OneShotToken { get; set; } = string.Empty;

        partial void OnOneShotTokenChanged(string value) => AddOrUpdateAuthHeader("Authorization", $"Bearer {value}", AuthenticationHeaderTarget.Header, 2);

        #endregion
    }
}
