using Newtonsoft.Json;
using System.Security;
using System.Xml.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace AndreasReitberger
{
    public partial class KlipperClient
    {
        #region Auth

        [JsonProperty(nameof(LoginRequired))]
        bool _loginRequired = false;
        [JsonIgnore]
        public bool LoginRequired
        {
            get => _loginRequired;
            set
            {
                if (_loginRequired == value) return;
                _loginRequired = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        bool _isLoggedIn = false;
        [JsonIgnore]
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                if (_isLoggedIn == value) return;
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(UserToken))]
        string _userToken = string.Empty;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        string _username;
        [JsonIgnore, XmlIgnore]
        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        SecureString _password;
        [JsonIgnore, XmlIgnore]
        public SecureString Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
