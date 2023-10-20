using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Auth

        [JsonProperty(nameof(UserToken))]
        string _userToken = string.Empty;
        [JsonIgnore, XmlIgnore]
        [Obsolete("Use SessionId for this")]
        public string UserToken
        {
            get => _userToken;
            set
            {
                if (_userToken == value) return;
                _userToken = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(OneShotToken))]
        string _oneShotToken = string.Empty;
        [JsonIgnore, XmlIgnore]
        public string OneShotToken
        {
            get => _oneShotToken;
            set
            {
                if (_oneShotToken == value) return;
                _oneShotToken = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(RefreshToken))]
        string _refreshToken = string.Empty;
        [JsonIgnore, XmlIgnore]
        public string RefreshToken
        {
            get => _refreshToken;
            set
            {
                if (_refreshToken == value) return;
                _refreshToken = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
