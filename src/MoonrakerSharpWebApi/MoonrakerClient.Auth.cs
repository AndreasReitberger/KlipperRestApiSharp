using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Auth
        /*
        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        new string apiKey = string.Empty;
        partial void OnApiKeyChanged(string value) => AddOrUpdateAuthHeader("usertoken", value);
        */
        

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string userToken = string.Empty;
        partial void OnUserTokenChanged(string value) => AddOrUpdateAuthHeader("usertoken", value);

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string refreshToken = string.Empty;
        partial void OnRefreshTokenChanged(string value) => AddOrUpdateAuthHeader("refreshtoken", value, 1);

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string oneShotToken = string.Empty;
        partial void OnOneShotTokenChanged(string value) => AddOrUpdateAuthHeader("oneshottoken", value, 2);

        #endregion
    }
}
