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
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string userToken = string.Empty;
        partial void OnUserTokenChanged(string value) => AddOrUpdateAuthHeader("Authorization", $"Bearer {value}", AuthenticationHeaderTarget.Header, 0);

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string refreshToken = string.Empty;
        partial void OnRefreshTokenChanged(string value) => AddOrUpdateAuthHeader("Authorization", $"Bearer {value}", AuthenticationHeaderTarget.Header, 1);

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string oneShotToken = string.Empty;
        partial void OnOneShotTokenChanged(string value) => AddOrUpdateAuthHeader("Authorization", $"Bearer {value}", AuthenticationHeaderTarget.Header, 2);

        #endregion
    }
}
