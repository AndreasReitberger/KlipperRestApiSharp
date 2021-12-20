using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Enum;
using AndreasReitberger.Interfaces;
using AndreasReitberger.Models;
using AndreasReitberger.Models.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebSocket4Net;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace AndreasReitberger
{
    // Needs: https://github.com/Arksine/moonraker/blob/master/docs/web_api.md
    // Docs: https://moonraker.readthedocs.io/en/latest/configuration/
    public class KlipperClient : IRestApiClient
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Variables
        static HttpClient client = new();
        int _retries = 0;
        #endregion

        #region Id
        [JsonProperty(nameof(Id))]
        Guid _id = Guid.Empty;
        [JsonIgnore]
        public Guid Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Instance
        static KlipperClient _instance = null;
        static readonly object Lock = new();
        public static KlipperClient Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new KlipperClient();
                }
                return _instance;
            }

            set
            {
                if (_instance == value) return;
                lock (Lock)
                {
                    _instance = value;
                }
            }

        }

        bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value)
                    return;
                _isActive = value;
                OnPropertyChanged();
            }
        }

        bool _updateInstance = false;
        public bool UpdateInstance
        {
            get => _updateInstance;
            set
            {
                if (_updateInstance == value)
                    return;
                _updateInstance = value;
                // Update the instance to the latest settings
                if (_updateInstance)
                    InitInstance(ServerAddress, Port, API, IsSecure);

                OnPropertyChanged();
            }
        }

        bool _isInitialized = false;
        public bool IsInitialized
        {
            get => _isInitialized;
            set
            {
                if (_isInitialized == value)
                    return;
                _isInitialized = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region RefreshTimer
        [JsonIgnore]
        [XmlIgnore]
        Timer _timer;
        [JsonIgnore]
        [XmlIgnore]
        public Timer Timer
        {
            get => _timer;
            set
            {
                if (_timer == value) return;
                _timer = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(RefreshInterval))]
        int _refreshInterval = 3;
        [JsonIgnore]
        public int RefreshInterval
        {
            get => _refreshInterval;
            set
            {
                if (_refreshInterval == value) return;
                _refreshInterval = value;
                if (IsListening)
                {
                    StopListening();
                    StartListening();
                }
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        bool _isListening = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool IsListening
        {
            get => _isListening;
            set
            {
                if (_isListening == value) return;
                _isListening = value;
                OnListeningChanged(new KlipperEventListeningChangedEventArgs()
                {
                    SessonId = SessionId,
                    IsListening = value,
                    IsListeningToWebSocket = IsListeningToWebsocket,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        bool _initialDataFetched = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool InitialDataFetched
        {
            get => _initialDataFetched;
            set
            {
                if (_initialDataFetched == value) return;
                _initialDataFetched = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Properties

        #region Connection
        [JsonIgnore]
        [XmlIgnore]
        HttpMessageHandler _httpHandler;
        [JsonIgnore]
        [XmlIgnore]
        public HttpMessageHandler HttpHandler
        {
            get => _httpHandler;
            set
            {
                if (_httpHandler == value) return;
                _httpHandler = value;
                UpdateWebClientInstance();
                OnPropertyChanged();

            }
        }

        [JsonIgnore]
        [XmlIgnore]
        string _sessionId = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string SessionId
        {
            get => _sessionId;
            set
            {
                if (_sessionId == value) return;
                _sessionId = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        string _userToken = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
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

        [JsonIgnore]
        [XmlIgnore]
        string _refreshToken = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
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

        [JsonProperty(nameof(ServerAddress))]
        string _address = string.Empty;
        [JsonIgnore]
        public string ServerAddress
        {
            get => _address;
            set
            {
                if (_address == value) return;
                _address = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsSecure))]
        bool _isSecure = false;
        [JsonIgnore]
        public bool IsSecure
        {
            get => _isSecure;
            set
            {
                if (_isSecure == value) return;
                _isSecure = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(API))]
        string _api = string.Empty;
        [JsonIgnore]
        public string API
        {
            get => _api;
            set
            {
                if (_api == value) return;
                _api = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Port))]
        int _port = 1880;
        [JsonIgnore]
        public int Port
        {
            get => _port;
            set
            {
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged();
                }
            }
        }
        /*
        [JsonProperty(nameof(Proxy))]
        WebProxy _proxy;
        [JsonIgnore]
        public WebProxy Proxy
        {
            get => _proxy;
            set
            {
                if (_proxy == value) return;             
                _proxy = value;
                OnPropertyChanged();                
            }
        }
        */

        [JsonProperty(nameof(OverrideValidationRules))]
        [XmlAttribute(nameof(OverrideValidationRules))]
        bool _overrideValidationRules = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool OverrideValidationRules
        {
            get => _overrideValidationRules;
            set
            {
                if (_overrideValidationRules == value)
                    return;
                _overrideValidationRules = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsOnline))]
        [XmlAttribute(nameof(IsOnline))]
        bool _isOnline = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                if (_isOnline == value) return;
                _isOnline = value;
                // Notify subscribres 
                if (IsOnline)
                {
                    OnServerWentOnline(new KlipperEventArgs()
                    {
                        SessonId = SessionId,
                    });
                }
                else
                {
                    OnServerWentOffline(new KlipperEventArgs()
                    {
                        SessonId = SessionId,
                    });
                }
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsConnecting))]
        [XmlAttribute(nameof(IsConnecting))]
        bool _isConnecting = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool IsConnecting
        {
            get => _isConnecting;
            set
            {
                if (_isConnecting == value) return;
                _isConnecting = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(AuthenticationFailed))]
        [XmlAttribute(nameof(AuthenticationFailed))]
        bool _authenticationFailed = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool AuthenticationFailed
        {
            get => _authenticationFailed;
            set
            {
                if (_authenticationFailed == value) return;
                _authenticationFailed = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsRefreshing))]
        [XmlAttribute(nameof(IsRefreshing))]
        bool _isRefreshing = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing == value) return;
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(RetriesWhenOffline))]
        [XmlAttribute(nameof(RetriesWhenOffline))]
        int _retriesWhenOffline = 2;
        [JsonIgnore]
        [XmlIgnore]
        public int RetriesWhenOffline
        {
            get => _retriesWhenOffline;
            set
            {
                if (_retriesWhenOffline == value) return;
                _retriesWhenOffline = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region General
        [JsonIgnore]
        [XmlIgnore]
        bool _updateAvailable = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool UpdateAvailable
        {
            get => _updateAvailable;
            private set
            {
                if (_updateAvailable == value) return;
                _updateAvailable = value;
                if (_updateAvailable)
                    // Notify on update available
                    OnServerUpdateAvailable(new KlipperEventArgs()
                    {
                        SessonId = this.SessionId,
                    });
                OnPropertyChanged();
            }
        }

        #endregion

        #region Proxy
        [JsonProperty(nameof(EnableProxy))]
        [XmlAttribute(nameof(EnableProxy))]
        bool _enableProxy = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool EnableProxy
        {
            get => _enableProxy;
            set
            {
                if (_enableProxy == value) return;
                _enableProxy = value;
                OnPropertyChanged();
                UpdateWebClientInstance();
            }
        }

        [JsonProperty(nameof(ProxyUseDefaultCredentials))]
        [XmlAttribute(nameof(ProxyUseDefaultCredentials))]
        bool _proxyUseDefaultCredentials = true;
        [JsonIgnore]
        [XmlIgnore]
        public bool ProxyUseDefaultCredentials
        {
            get => _proxyUseDefaultCredentials;
            set
            {
                if (_proxyUseDefaultCredentials == value) return;
                _proxyUseDefaultCredentials = value;
                OnPropertyChanged();
                UpdateWebClientInstance();
            }
        }

        [JsonProperty(nameof(SecureProxyConnection))]
        [XmlAttribute(nameof(SecureProxyConnection))]
        bool _secureProxyConnection = true;
        [JsonIgnore]
        [XmlIgnore]
        public bool SecureProxyConnection
        {
            get => _secureProxyConnection;
            private set
            {
                if (_secureProxyConnection == value) return;
                _secureProxyConnection = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyAddress))]
        [XmlAttribute(nameof(ProxyAddress))]
        string _proxyAddress = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string ProxyAddress
        {
            get => _proxyAddress;
            private set
            {
                if (_proxyAddress == value) return;
                _proxyAddress = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyPort))]
        [XmlAttribute(nameof(ProxyPort))]
        int _proxyPort = 443;
        [JsonIgnore]
        [XmlIgnore]
        public int ProxyPort
        {
            get => _proxyPort;
            private set
            {
                if (_proxyPort == value) return;
                _proxyPort = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyUser))]
        [XmlAttribute(nameof(ProxyUser))]
        string _proxyUser = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string ProxyUser
        {
            get => _proxyUser;
            private set
            {
                if (_proxyUser == value) return;
                _proxyUser = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyPassword))]
        [XmlAttribute(nameof(ProxyPassword))]
        SecureString _proxyPassword;
        [JsonIgnore]
        [XmlIgnore]
        public SecureString ProxyPassword
        {
            get => _proxyPassword;
            private set
            {
                if (_proxyPassword == value) return;
                _proxyPassword = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DiskSpace
        [JsonProperty(nameof(FreeDiskSpace))]
        [XmlAttribute(nameof(FreeDiskSpace))]
        long _freeDiskspace = 0;
        [JsonIgnore]
        [XmlIgnore]
        public long FreeDiskSpace
        {
            get => _freeDiskspace;
            set
            {
                if (_freeDiskspace == value) return;
                _freeDiskspace = value;
                OnPropertyChanged();

            }
        }

        [JsonProperty(nameof(UsedDiskSpace))]
        [XmlAttribute(nameof(UsedDiskSpace))]
        long _usedDiskSpace = 0;
        [JsonIgnore]
        [XmlIgnore]
        public long UsedDiskSpace
        {
            get => _usedDiskSpace;
            set
            {
                if (_usedDiskSpace == value) return;
                _usedDiskSpace = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(TotalDiskSpace))]
        [XmlAttribute(nameof(TotalDiskSpace))]
        long _totalDiskSpace = 0;
        [JsonIgnore]
        [XmlIgnore]
        public long TotalDiskSpace
        {
            get => _totalDiskSpace;
            set
            {
                if (_totalDiskSpace == value) return;
                _totalDiskSpace = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Files
        [JsonIgnore]
        [XmlIgnore]
        ObservableCollection<KlipperFile> _files = new();
        [JsonIgnore]
        [XmlIgnore]
        public ObservableCollection<KlipperFile> Files
        {
            get => _files;
            set
            {
                if (_files == value) return;
                _files = value;
                OnKlipperFilesChanged(new KlipperFilesChangedEventArgs()
                {
                    NewFiles = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Token = !string.IsNullOrEmpty(UserToken) ? UserToken : API,
                });
                OnPropertyChanged();
            }
        }
        #endregion

        #region Jobs
        [JsonIgnore]
        [XmlIgnore]
        ObservableCollection<KlipperJobQueueItem> _jobList = new();
        [JsonIgnore]
        [XmlIgnore]
        public ObservableCollection<KlipperJobQueueItem> JobList
        {
            get => _jobList;
            set
            {
                if (_jobList == value) return;
                _jobList = value;
                OnKlipperJobListChanged(new KlipperJobListChangedEventArgs()
                {
                    NewJobList = value,
                    NewJobListStatus = JobQueueStatus,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Token = !string.IsNullOrEmpty(UserToken) ? UserToken : API,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        string _jobQueueStatus = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string JobQueueStatus
        {
            get => _jobQueueStatus;
            set
            {
                if (_jobQueueStatus == value) return;
                _jobQueueStatus = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region State & Config
        [JsonIgnore]
        [XmlIgnore]
        string _klipperState;
        [JsonIgnore]
        [XmlIgnore]
        public string KlipperState
        {
            get => _klipperState;
            set
            {
                if (_klipperState == value) return;
                OnKlipperStateChanged(new KlipperStateChangedEventArgs()
                {
                    NewState = value,
                    PreviousState = _klipperState,
                });
                _klipperState = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        KlipperServerConfig _config;
        [JsonIgnore]
        [XmlIgnore]
        public KlipperServerConfig Config
        {
            get => _config;
            set
            {
                if (_config == value) return;
                _config = value;
                OnKlipperServerConfigChanged(new KlipperServerConfigChangedEventArgs()
                {
                    NewConfiguration = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                UpdateServerConfig(value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        KlipperPrinterStateMessageResult _printerInfo;
        [JsonIgnore]
        [XmlIgnore]
        public KlipperPrinterStateMessageResult PrinterInfo
        {
            get => _printerInfo;
            set
            {
                if (_printerInfo == value) return;
                _printerInfo = value;
                OnKlipperPrinterInfoChanged(new KlipperPrinterInfoChangedEventArgs()
                {
                    NewPrinterInfo = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                UpdatePrinterInfo(value);
                OnPropertyChanged();
            }
        }

        [XmlIgnore, JsonIgnore]
        SerializableDictionary<int, KlipperStatusExtruder> _extruders = new();
        [XmlIgnore, JsonIgnore]
        public SerializableDictionary<int, KlipperStatusExtruder> Extruders
        {
            get => _extruders;
            set
            {
                if (_extruders == value) return;
                _extruders = value;
                OnKlipperExtruderStatesChanged(new KlipperExtruderStatesChangedEventArgs()
                {
                    ExtruderStates = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        KlipperStatusHeaterBed _heaterBed;
        [JsonIgnore]
        [XmlIgnore]
        public KlipperStatusHeaterBed HeaterBed
        {
            get => _heaterBed;
            set
            {
                if (_heaterBed == value) return;
                _heaterBed = value;
                OnKlipperHeaterBedStateChanged(new KlipperHeaterBedStateChangedEventArgs()
                {
                    HeaterBedState = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }


        #endregion

        #region ReadOnly

        public string FullWebAddress
        {
            get => $"{(IsSecure ? "https" : "http")}://{ServerAddress}:{Port}";
        }

        public bool IsReady
        {
            get
            {
                return (
                    !string.IsNullOrEmpty(ServerAddress)) && Port > 0 &&
                    (
                        // Address
                        Regex.IsMatch(ServerAddress, RegexHelper.IPv4AddressRegex) || Regex.IsMatch(ServerAddress, RegexHelper.IPv6AddressRegex) || Regex.IsMatch(ServerAddress, RegexHelper.Fqdn)
                        ||
                        // Or validation rules are overriden
                        OverrideValidationRules
                    )
                    ;
            }
        }
        #endregion

        #endregion

        #region WebSocket
        [JsonIgnore]
        [XmlIgnore]
        WebSocket _webSocket;
        [JsonIgnore]
        [XmlIgnore]
        public WebSocket WebSocket
        {
            get => _webSocket;
            set
            {
                if (_webSocket == value) return;
                _webSocket = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        long? _webSocketConnectionId;
        [JsonIgnore]
        [XmlIgnore]
        public long? WebSocketConnectionId
        {
            get => _webSocketConnectionId;
            set
            {
                if (_webSocketConnectionId == value) return;
                _webSocketConnectionId = value;
                OnWebSocketConnectionIdChanged(new KlipperWebSocketConnectionChangedEventArgs()
                {
                    ConnectionId = value,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        Timer _pingTimer;
        [JsonIgnore]
        [XmlIgnore]
        public Timer PingTimer
        {
            get => _pingTimer;
            set
            {
                if (_pingTimer == value) return;
                _pingTimer = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        int _pingCounter = 0;
        [JsonIgnore]
        [XmlIgnore]
        public int PingCounter
        {
            get => _pingCounter;
            set
            {
                if (_pingCounter == value) return;
                _pingCounter = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        int _refreshCounter = 0;
        [JsonIgnore]
        [XmlIgnore]
        public int RefreshCounter
        {
            get => _refreshCounter;
            set
            {
                if (_refreshCounter == value) return;
                _refreshCounter = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        bool _isListeningToWebSocket = false;
        [JsonIgnore]
        [XmlIgnore]
        public bool IsListeningToWebsocket
        {
            get => _isListeningToWebSocket;
            set
            {
                if (_isListeningToWebSocket == value) return;
                _isListeningToWebSocket = value;
                OnListeningChanged(new KlipperEventListeningChangedEventArgs()
                {
                    SessonId = SessionId,
                    IsListening = IsListening,
                    IsListeningToWebSocket = value,
                });
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public KlipperClient()
        {
            Id = Guid.NewGuid();
            UpdateWebClientInstance();
        }

        public KlipperClient(string serverAddress, string api, int port = 80, bool isSecure = false)
        {
            Id = Guid.NewGuid();
            InitInstance(serverAddress, port, api, isSecure);
            UpdateWebClientInstance();
        }

        public KlipperClient(string serverAddress, int port = 80, bool isSecure = false)
        {
            Id = Guid.NewGuid();
            InitInstance(serverAddress, port, "", isSecure);
            UpdateWebClientInstance();
        }
        #endregion

        #region Destructor
        ~KlipperClient()
        {
            if (WebSocket != null)
            {
                /* SharpWebSocket
                if (WebSocket.ReadyState == WebSocketState.Open)
                    WebSocket.Close();
                WebSocket = null;
                */
            }
        }
        #endregion

        #region Init
        public void InitInstance()
        {
            try
            {
                Instance = this;
                if (Instance != null)
                {
                    Instance.UpdateInstance = false;
                    Instance.IsInitialized = true;
                }
                UpdateInstance = false;
                IsInitialized = true;
            }
            catch (Exception exc)
            {
                //UpdateInstance = true;
                OnError(new UnhandledExceptionEventArgs(exc, false));
                IsInitialized = false;
            }
        }
        public static void UpdateSingleInstance(KlipperClient Inst)
        {
            try
            {
                Instance = Inst;
            }
            catch (Exception)
            {
                //OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public void InitInstance(string serverAddress, int port = 3344, string api = "", bool isSecure = false)
        {
            try
            {
                ServerAddress = serverAddress;
                API = api;
                Port = port;
                IsSecure = isSecure;

                Instance = this;

                if (Instance != null)
                {
                    Instance.UpdateInstance = false;
                    Instance.IsInitialized = true;
                }
                UpdateInstance = false;
                IsInitialized = true;
            }
            catch (Exception exc)
            {
                //UpdateInstance = true;
                OnError(new UnhandledExceptionEventArgs(exc, false));
                IsInitialized = false;
            }
        }
        #endregion

        #region EventHandlerss

        #region WebSocket

        public event EventHandler<KlipperEventArgs> WebSocketConnected;
        protected virtual void OnWebSocketConnected(KlipperEventArgs e)
        {
            WebSocketConnected?.Invoke(this, e);
        }

        public event EventHandler<KlipperEventArgs> WebSocketDisconnected;
        protected virtual void OnWebSocketDisconnected(KlipperEventArgs e)
        {
            WebSocketDisconnected?.Invoke(this, e);
        }

        public event EventHandler<ErrorEventArgs> WebSocketError;
        protected virtual void OnWebSocketError(ErrorEventArgs e)
        {
            WebSocketError?.Invoke(this, e);
        }

        public event EventHandler<KlipperEventArgs> WebSocketDataReceived;
        protected virtual void OnWebSocketDataReceived(KlipperEventArgs e)
        {
            WebSocketDataReceived?.Invoke(this, e);
        }
        public event EventHandler<KlipperWebSocketConnectionChangedEventArgs> WebSocketConnectionIdChanged;
        protected virtual void OnWebSocketConnectionIdChanged(KlipperWebSocketConnectionChangedEventArgs e)
        {
            WebSocketConnectionIdChanged?.Invoke(this, e);
        }
        /*
        public event EventHandler<KlipperLoginRequiredEventArgs> LoginResultReceived;
        protected virtual void OnLoginResultReceived(KlipperLoginRequiredEventArgs e)
        {
            LoginResultReceived?.Invoke(this, e);
        }
        */
        #endregion

        #region ServerConnectionState

        public event EventHandler<KlipperEventArgs> ServerWentOffline;
        protected virtual void OnServerWentOffline(KlipperEventArgs e)
        {
            ServerWentOffline?.Invoke(this, e);
        }

        public event EventHandler<KlipperEventArgs> ServerWentOnline;
        protected virtual void OnServerWentOnline(KlipperEventArgs e)
        {
            ServerWentOnline?.Invoke(this, e);
        }

        public event EventHandler<KlipperEventArgs> ServerUpdateAvailable;
        protected virtual void OnServerUpdateAvailable(KlipperEventArgs e)
        {
            ServerUpdateAvailable?.Invoke(this, e);
        }
        #endregion

        #region State & Config
        public event EventHandler<KlipperStateChangedEventArgs> KlipperStateChanged;
        protected virtual void OnKlipperStateChanged(KlipperStateChangedEventArgs e)
        {
            KlipperStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperServerConfigChangedEventArgs> KlipperServerConfigChanged;
        protected virtual void OnKlipperServerConfigChanged(KlipperServerConfigChangedEventArgs e)
        {
            KlipperServerConfigChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperPrinterInfoChangedEventArgs> KlipperPrinterInfoChanged;
        protected virtual void OnKlipperPrinterInfoChanged(KlipperPrinterInfoChangedEventArgs e)
        {
            KlipperPrinterInfoChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperExtruderStatesChangedEventArgs> KlipperExtruderStatesChanged;
        protected virtual void OnKlipperExtruderStatesChanged(KlipperExtruderStatesChangedEventArgs e)
        {
            KlipperExtruderStatesChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperHeaterBedStateChangedEventArgs> KlipperHeaterBedStateChanged;
        protected virtual void OnKlipperHeaterBedStateChanged(KlipperHeaterBedStateChangedEventArgs e)
        {
            KlipperHeaterBedStateChanged?.Invoke(this, e);
        }
        #endregion

        #region Files
        public event EventHandler<KlipperFilesChangedEventArgs> KlipperFilesChanged;
        protected virtual void OnKlipperFilesChanged(KlipperFilesChangedEventArgs e)
        {
            KlipperFilesChanged?.Invoke(this, e);
        }
        #endregion

        #region Jobs & Queue
        public event EventHandler<KlipperJobListChangedEventArgs> KlipperJobListChanged;
        protected virtual void OnKlipperJobListChanged(KlipperJobListChangedEventArgs e)
        {
            KlipperJobListChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperJobFinishedEventArgs> JobFinished;
        protected virtual void OnJobFinished(KlipperJobFinishedEventArgs e)
        {
            JobFinished?.Invoke(this, e);
        }
        #endregion

        #region Errors

        public event EventHandler Error;
        protected virtual void OnError()
        {
            Error?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnError(ErrorEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(UnhandledExceptionEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(KlipprtJsonConvertEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        public event EventHandler<KlipperRestEventArgs> RestApiError;
        protected virtual void OnRestApiError(KlipperRestEventArgs e)
        {
            RestApiError?.Invoke(this, e);
        }

        public event EventHandler<KlipperRestEventArgs> RestApiAuthenticationError;
        protected virtual void OnRestApiAuthenticationError(KlipperRestEventArgs e)
        {
            RestApiAuthenticationError?.Invoke(this, e);
        }
        public event EventHandler<KlipperRestEventArgs> RestApiAuthenticationSucceeded;
        protected virtual void OnRestApiAuthenticationSucceeded(KlipperRestEventArgs e)
        {
            RestApiAuthenticationSucceeded?.Invoke(this, e);
        }

        public event EventHandler<KlipprtJsonConvertEventArgs> RestJsonConvertError;
        protected virtual void OnRestJsonConvertError(KlipprtJsonConvertEventArgs e)
        {
            RestJsonConvertError?.Invoke(this, e);
        }

        #endregion

        #region ServerStateChanges

        public event EventHandler<KlipperEventListeningChangedEventArgs> ListeningChanged;
        protected virtual void OnListeningChanged(KlipperEventListeningChangedEventArgs e)
        {
            ListeningChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperSessionChangedEventArgs> SessionChanged;
        protected virtual void OnSessionChanged(KlipperSessionChangedEventArgs e)
        {
            SessionChanged?.Invoke(this, e);
        }
        #endregion

        #endregion

        #region WebSocket
        void PingServer()
        {
            try
            {
                if (WebSocket != null)
                    if (WebSocket.State == WebSocketState.Open)
                    {
                        string infoCommand = $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.info\",\"params\":{{}},\"id\":1}}";
                        //string pingCommand = $"{{}}";
                        WebSocket.Send(infoCommand);
                    }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }

        }

        public void ConnectWebSocket()
        {
            try
            {
                //if (!IsReady) return;
                if (!string.IsNullOrEmpty(FullWebAddress) && (
                    Regex.IsMatch(FullWebAddress, RegexHelper.IPv4AddressRegex) ||
                    Regex.IsMatch(FullWebAddress, RegexHelper.IPv6AddressRegex) ||
                    Regex.IsMatch(FullWebAddress, RegexHelper.Fqdn)))
                {
                    return;
                }
                //if (!IsReady || IsListeningToWebsocket) return;

                DisconnectWebSocket();
                // https://github.com/Arksine/moonraker/blob/master/docs/web_api.md#appendix
                // ws://host:port/websocket?token={32 character base32 string}
                //string target = $"ws://192.168.10.113:80/websocket?token={API}";
                string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(API) ? $"?token={API}" : "")}";
                WebSocket = new WebSocket(target)
                {
                    EnableAutoSendPing = false
                };

                if (IsSecure)
                {
                    // https://github.com/sta/websocket-sharp/issues/219#issuecomment-453535816
                    SslProtocols sslProtocolHack = (SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
                    //Avoid TlsHandshakeFailure
                    if (WebSocket.Security.EnabledSslProtocols != sslProtocolHack)
                    {
                        WebSocket.Security.EnabledSslProtocols = sslProtocolHack;
                    }
                }

                WebSocket.MessageReceived += WebSocket_MessageReceived;
                //WebSocket.DataReceived += WebSocket_DataReceived;
                WebSocket.Opened += WebSocket_Opened;
                WebSocket.Closed += WebSocket_Closed;
                WebSocket.Error += WebSocket_Error;

#if NETSTANDARD
                WebSocket.OpenAsync();
#else
                WebSocket.Open();
#endif

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public void DisconnectWebSocket()
        {
            try
            {
                if (WebSocket != null)
                {
                    if (WebSocket.State == WebSocketState.Open)
#if NETSTANDARD
                        WebSocket.CloseAsync();
#else
                        WebSocket.Close();
#endif
                    StopPingTimer();

                    WebSocket.MessageReceived -= WebSocket_MessageReceived;
                    //WebSocket.DataReceived -= WebSocket_DataReceived;
                    WebSocket.Opened -= WebSocket_Opened;
                    WebSocket.Closed -= WebSocket_Closed;
                    WebSocket.Error -= WebSocket_Error;

                    WebSocket = null;
                }
                //WebSocket = null;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        private void WebSocket_Error(object sender, ErrorEventArgs e)
        {
            IsListeningToWebsocket = false;
            OnWebSocketError(e);
            OnError(e);
        }

        private void WebSocket_Closed(object sender, EventArgs e)
        {
            IsListeningToWebsocket = false;
            StopPingTimer();
            OnWebSocketDisconnected(new KlipperEventArgs()
            {
                Message = $"WebSocket connection to {WebSocket} closed. Connection state while closing was '{(IsOnline ? "online" : "offline")}'",
            });
        }

        private void WebSocket_Opened(object sender, EventArgs e)
        {
            // Get ready state from klipper
            string infoCommand = $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.info\",\"params\":{{}},\"id\":1}}";
            WebSocket.Send(infoCommand);

            // Get the websocket Id of the current connection
            string connectionId = $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.websocket.id\",\"params\":{{}},\"id\":2}}";
            WebSocket.Send(connectionId);

            // No ping needed to keep connection alive
            //PingTimer = new Timer((action) => PingServer(), null, 0, 2500);

            IsListeningToWebsocket = true;
            OnWebSocketConnected(new KlipperEventArgs()
            {
                Message = $"WebSocket connection to {WebSocket} established. Connection state while opening was '{(IsOnline ? "online" : "offline")}'",
            });
        }

        private void WebSocket_DataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        private void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                if (e.Message == null || string.IsNullOrEmpty(e.Message))
                    return;

                if (e.Message.ToLower().Contains("method"))
                {
                    try
                    {
                        List<string> allowedTags = new()
                        {
                            "mcu",
                            "toolhead",
                            "extruder",
                            "moonraker_stats"
                        };
                        KlipperWebSocketMessage method = JsonConvert.DeserializeObject<KlipperWebSocketMessage>(e.Message);
                        for (int i = 0; i < method?.Params?.Count; i++)
                        {
                            string current = method.Params[i]?.ToString();
                            if (!allowedTags.Any(current.Contains))
                                continue;

                            if (current.ToLower().Contains("moonraker_stats"))
                            {
                                try
                                {
                                    KlipperWebSocketNotifyProcStatUpdateRespone notifyProcState =
                                        JsonConvert.DeserializeObject<KlipperWebSocketNotifyProcStatUpdateRespone>(current);
                                    // If found, contine with next element
                                    continue;
                                }
                                catch (Exception exc)
                                { }
                            }
                            else if (current.ToLower().Contains("mcu"))
                            {
                                try
                                {
                                    KlipperWebSocketMcuRespone mcuState =
                                        JsonConvert.DeserializeObject<KlipperWebSocketMcuRespone>(current);
                                    // If found, contine with next element
                                    continue;
                                }
                                catch (Exception exc)
                                { }
                            }
                            else if (current.ToLower().Contains("toolhead") && current.ToLower().Contains("extruder"))
                            {
                                try
                                {
                                    KlipperWebSocketExtruderRespone toolheadState =
                                        JsonConvert.DeserializeObject<KlipperWebSocketExtruderRespone>(current);
                                    SerializableDictionary<int, KlipperStatusExtruder> stats = new();
                                    if (toolheadState.Extruder != null)
                                    {
                                        stats.Add(0, toolheadState.Extruder);
                                    }
                                    if (toolheadState.Extruder1 != null)
                                    {
                                        stats.Add(1, toolheadState.Extruder1);
                                    }
                                    if (toolheadState.Extruder2 != null)
                                    {
                                        stats.Add(2, toolheadState.Extruder2);
                                    }
                                    if (toolheadState.Extruder3 != null)
                                    {
                                        stats.Add(3, toolheadState.Extruder3);
                                    }
                                    if (stats.Count > 0)
                                    {
                                        Extruders = stats;
                                    }
                                    // If found, contine with next element
                                    continue;
                                }
                                catch (Exception exc)
                                { }
                            }
                            else if (current.ToLower().Contains("toolhead") && current.ToLower().Contains("heater_bed"))
                            {
                                try
                                {
                                    KlipperWebSocketHeaterBedRespone toolheadHeaterBedState =
                                        JsonConvert.DeserializeObject<KlipperWebSocketHeaterBedRespone>(current);
                                    HeaterBed = toolheadHeaterBedState.HeaterBed;
                                    // If found, contine with next element
                                    continue;
                                }
                                catch (Exception exc)
                                { }
                            }
#if DEBUG
                            Console.WriteLine($"No Json object found for '{current}'");
#else
                            OnError(new KlipprtJsonConvertEventArgs()
                            {
                                Exception = null,
                                OriginalString = current,
                                Message = "No Json object found for this string",
                            });
#endif
                        }
                    }
                    catch (JsonException jecx)
                    {
                        OnError(new KlipprtJsonConvertEventArgs()
                        {
                            Exception = jecx,
                            OriginalString = e.Message,
                            Message = jecx.Message,
                        });
                    }
                    catch (Exception exc)
                    {
                        OnError(new UnhandledExceptionEventArgs(exc, false));
                    }
                }
                else if (e.Message.ToLower().Contains("error"))
                {
                    //Session = JsonConvert.DeserializeObject<EventSession>(e.Message);
                }
                else if (e.Message.ToLower().Contains("result"))
                {
                    try
                    {
                        KlipperWebSocketResult result = JsonConvert.DeserializeObject<KlipperWebSocketResult>(e.Message);
                        //var type = result?.Result?.GetType();
                        string jsonObject = result?.Result?.ToString();
                        // Try to get the result
                        try
                        {
                            KlipperWebSocketIdRespone wsId = JsonConvert.DeserializeObject<KlipperWebSocketIdRespone>(jsonObject);
                            WebSocketConnectionId = wsId?.WebsocketId;
                        }
                        catch (Exception) { }
                        try
                        {
                            KlipperWebSocketStateRespone state = JsonConvert.DeserializeObject<KlipperWebSocketStateRespone>(jsonObject);
                            KlipperState = state?.KlippyState;
                        }
                        catch (Exception) { }
                    }
                    catch (JsonException jecx)
                    {
                        OnError(new KlipprtJsonConvertEventArgs()
                        {
                            Exception = jecx,
                            OriginalString = e.Message,
                            Message = jecx.Message,
                        });
                    }
                    catch (Exception exc)
                    {
                        OnError(new UnhandledExceptionEventArgs(exc, false));
                    }
                }
                else
                {

                }
                OnWebSocketDataReceived(new KlipperEventArgs()
                {
                    CallbackId = PingCounter,
                    Message = e.Message,
                    SessonId = SessionId,
                });
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = e.Message,
                    Message = jecx.Message,
                });
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #region Methods

        #region Private

        #region ValidateResult

        bool GetQueryResult(string result, bool EmptyResultIsValid = false)
        {
            try
            {
                if ((string.IsNullOrEmpty(result) || result == "{}") && EmptyResultIsValid)
                    return true;
                KlipperActionResult actionResult = JsonConvert.DeserializeObject<KlipperActionResult>(result);
                if (actionResult != null)
                    return actionResult.Result.ToLower() == "ok";
                else
                    return false;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result,
                    Message = jecx.Message,
                });
                return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region RestApi
        async Task<KlipperApiRequestRespone> SendRestApiRequestAsync(
            MoonRakerCommandBase commandBase, Method method, string command, CancellationTokenSource cts, string jsonDataString = "",
            Dictionary<string, string> urlSegments = null, string requestTargetUri = "")
        {
            KlipperApiRequestRespone apiRsponeResult = new() { IsOnline = IsOnline };
            if (!IsOnline) return apiRsponeResult;

            try
            {
                // https://github.com/Arksine/moonraker/blob/master/docs/web_api.md
                RestClient client = new(FullWebAddress);
                if (EnableProxy)
                {
                    WebProxy proxy = GetCurrentProxy();
                    client.Proxy = proxy;
                }
                RestRequest request = new(
                    $"{(string.IsNullOrEmpty(requestTargetUri) ? commandBase.ToString() : requestTargetUri)}/{command}")
                {
                    RequestFormat = DataFormat.Json,
                    Method = method
                };
                if (!string.IsNullOrEmpty(UserToken))
                {
                    request.AddHeader("Authorization", $"Bearer {UserToken}");
                }
                if (urlSegments != null)
                {
                    foreach (KeyValuePair<string, string> pair in urlSegments)
                    {
                        request.AddParameter(pair.Key, pair.Value, ParameterType.QueryString);
                    }
                }

                //request.AddParameter("a", Command, ParameterType.QueryString);
                //request.AddParameter("", command, ParameterType.QueryString);
                if (!string.IsNullOrEmpty(jsonDataString))
                {
                    request.AddJsonBody(jsonDataString);
                }

                if (!string.IsNullOrEmpty(API))
                {
                    request.AddParameter("token", API, ParameterType.QueryString);
                }
                else if (!string.IsNullOrEmpty(SessionId))
                {
                    request.AddParameter("token", SessionId, ParameterType.QueryString);
                }

                Uri fullUri = client.BuildUri(request);
                try
                {
                    IRestResponse respone = await client.ExecuteAsync(request, cts.Token).ConfigureAwait(false);

                    if (respone.StatusCode == HttpStatusCode.OK && respone.ResponseStatus == ResponseStatus.Completed)
                    {
                        apiRsponeResult.IsOnline = true;
                        AuthenticationFailed = false;
                        apiRsponeResult.Result = respone.Content;
                        apiRsponeResult.Succeeded = true;
                        apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation || respone.StatusCode == HttpStatusCode.Forbidden)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = true;
                        apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else
                    {
                        OnRestApiError(new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        });
                        //throw respone.ErrorException;
                    }
                }
                catch (TaskCanceledException texp)
                {
                    if (!IsOnline)
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                }

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<KlipperApiRequestRespone> SendRestApiRequestAsync(
            MoonRakerCommandBase commandBase, Method method, string command, string jsonDataString = "", int timeout = 10000, Dictionary<string, string> urlSegments = null, string requestTargetUri = "")
        {
            CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
            return await SendRestApiRequestAsync(commandBase, method, command, cts, jsonDataString, urlSegments, requestTargetUri)
                .ConfigureAwait(false);
        }

        async Task<KlipperApiRequestRespone> SendRestApiRequestAsync(
            MoonRakerCommandBase commandBase, Method method, string command, object jsonData, int timeout = 10000, Dictionary<string, string> urlSegments = null, string requestTargetUri = "")
        {
            return await SendRestApiRequestAsync(commandBase, method, command, JsonConvert.SerializeObject(jsonData), timeout, urlSegments, requestTargetUri)
                .ConfigureAwait(false);
        }

        async Task<KlipperApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            string filePath,
            string root = "gcodes",
            string path = "",
            int timeout = 100000)
        {
            KlipperApiRequestRespone apiRsponeResult = new();
            if (!IsOnline) return apiRsponeResult;

            try
            {
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                RestClient client = new(FullWebAddress);

                RestRequest request = new("/server/files/upload");
                if (!string.IsNullOrEmpty(UserToken))
                {
                    request.AddHeader("Authorization", $"Bearer {UserToken}");
                }
                else
                {
                    request.AddHeader("X-Api-Key", API);
                }
                request.RequestFormat = DataFormat.Json;
                request.Method = Method.POST;
                request.AlwaysMultipartFormData = true;

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFile("file", filePath, "application/octet-stream");
                request.AddParameter("root", root, "multipart/form-data", ParameterType.GetOrPost);
                request.AddParameter("path", path, "multipart/form-data", ParameterType.GetOrPost);

                Uri fullUri = client.BuildUri(request);
                try
                {
                    IRestResponse respone = await client.ExecuteAsync(request, cts.Token);
                    if ((
                        respone.StatusCode == HttpStatusCode.OK || respone.StatusCode == HttpStatusCode.NoContent) &&
                        respone.ResponseStatus == ResponseStatus.Completed)
                    {
                        apiRsponeResult.IsOnline = true;
                        AuthenticationFailed = false;
                        apiRsponeResult.Result = respone.Content;
                        apiRsponeResult.Succeeded = true;
                        apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation || respone.StatusCode == HttpStatusCode.Forbidden)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = true;
                        apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    // For instance if printer is not connected
                    else if (respone.StatusCode == HttpStatusCode.Conflict)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = false;
                        apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else
                    {
                        OnRestApiError(new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        });
                        //throw respone.ErrorException;
                    }
                }
                catch (TaskCanceledException texp)
                {
                    if (!IsOnline)
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<KlipperApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            string fileName,
            byte[] file,
            string root = "gcodes",
            string path = "",
            int timeout = 100000)
        {
            KlipperApiRequestRespone apiRsponeResult = new();
            if (!IsOnline) return apiRsponeResult;

            try
            {
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                RestClient client = new(FullWebAddress);

                RestRequest request = new("/server/files/upload");
                if (!string.IsNullOrEmpty(UserToken))
                {
                    request.AddHeader("Authorization", $"Bearer {UserToken}");
                }
                else
                {
                    request.AddHeader("X-Api-Key", API);
                }
                request.RequestFormat = DataFormat.Json;
                request.Method = Method.POST;
                request.AlwaysMultipartFormData = true;

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFileBytes("file", file, fileName, "application/octet-stream");
                request.AddParameter("root", root, "multipart/form-data", ParameterType.GetOrPost);
                request.AddParameter("path", path, "multipart/form-data", ParameterType.GetOrPost);

                Uri fullUri = client.BuildUri(request);
                try
                {
                    IRestResponse respone = await client.ExecuteAsync(request, cts.Token);
                    if ((
                        respone.StatusCode == HttpStatusCode.OK || respone.StatusCode == HttpStatusCode.NoContent) &&
                        respone.ResponseStatus == ResponseStatus.Completed)
                    {
                        apiRsponeResult.IsOnline = true;
                        AuthenticationFailed = false;
                        apiRsponeResult.Result = respone.Content;
                        apiRsponeResult.Succeeded = true;
                        apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation || respone.StatusCode == HttpStatusCode.Forbidden)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = true;
                        apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    // For instance if printer is not connected
                    else if (respone.StatusCode == HttpStatusCode.Conflict)
                    {
                        apiRsponeResult.IsOnline = true;
                        apiRsponeResult.HasAuthenticationError = false;
                        apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        };
                    }
                    else
                    {
                        OnRestApiError(new KlipperRestEventArgs()
                        {
                            Status = respone.ResponseStatus.ToString(),
                            Exception = respone.ErrorException,
                            Message = respone.ErrorMessage,
                            Uri = fullUri,
                        });
                        //throw respone.ErrorException;
                    }
                }
                catch (TaskCanceledException texp)
                {
                    if (!IsOnline)
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }
        #endregion

        #region Download
        byte[] DownloadFileFromUri(Uri downloadUri, int Timeout = 5000)
        {
            try
            {
                RestClient client = new(downloadUri);
                RestRequest request = new("");
                request.AddHeader("X-Api-Key", API);
                request.RequestFormat = DataFormat.Json;
                request.Method = Method.GET;
                request.Timeout = Timeout;

                Uri fullUrl = client.BuildUri(request);
                byte[] respone = client.DownloadData(request);

                return respone;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }
        #endregion

        #region Proxy
        Uri GetProxyUri()
        {
            return ProxyAddress.StartsWith("http://") || ProxyAddress.StartsWith("https://") ? new Uri($"{ProxyAddress}:{ProxyPort}") : new Uri($"{(SecureProxyConnection ? "https" : "http")}://{ProxyAddress}:{ProxyPort}");
        }

        WebProxy GetCurrentProxy()
        {
            var proxy = new WebProxy()
            {
                Address = GetProxyUri(),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = ProxyUseDefaultCredentials,
            };
            if (ProxyUseDefaultCredentials && !string.IsNullOrEmpty(ProxyUser))
                proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
            else
                proxy.UseDefaultCredentials = ProxyUseDefaultCredentials;
            return proxy;
        }
        void UpdateWebClientInstance()
        {
            if (EnableProxy && !string.IsNullOrEmpty(ProxyAddress))
            {
                //var proxy = GetCurrentProxy();
                HttpMessageHandler handler = HttpHandler ?? new HttpClientHandler()
                {
                    UseProxy = true,
                    Proxy = GetCurrentProxy(),
                    AllowAutoRedirect = true,
                };

                client = new HttpClient(handler: handler, disposeHandler: true);
            }
            else
            {
                if (HttpHandler == null)
                    client = new HttpClient();
                else
                    client = new HttpClient(handler: HttpHandler, disposeHandler: true);
            }
        }
        #endregion

        #region Timers
        void StopPingTimer()
        {
            if (PingTimer != null)
            {
                try
                {
                    PingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    PingTimer = null;
                    IsListeningToWebsocket = false;
                }
                catch (ObjectDisposedException)
                {
                    //PingTimer = null;
                }
            }
        }
        void StopTimer()
        {
            if (Timer != null)
            {
                try
                {
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);
                    Timer = null;
                    IsListening = false;
                }
                catch (ObjectDisposedException)
                {
                    //PingTimer = null;
                }
            }
        }
        #endregion

        #region State & Config
        void UpdateServerConfig(KlipperServerConfig newConfig)
        {
            try
            {
                if (newConfig == null) return;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        void UpdatePrinterInfo(KlipperPrinterStateMessageResult newPrinterInfo)
        {
            try
            {
                if (newPrinterInfo == null) return;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #endregion

        #region Public

        #region Proxy
        public void SetProxy(bool Secure, string Address, int Port, bool Enable = true)
        {
            EnableProxy = Enable;
            ProxyUseDefaultCredentials = true;
            ProxyAddress = Address;
            ProxyPort = Port;
            ProxyUser = string.Empty;
            ProxyPassword = null;
            SecureProxyConnection = Secure;
            UpdateWebClientInstance();
        }
        public void SetProxy(bool Secure, string Address, int Port, string User = "", SecureString Password = null, bool Enable = true)
        {
            EnableProxy = Enable;
            ProxyUseDefaultCredentials = false;
            ProxyAddress = Address;
            ProxyPort = Port;
            ProxyUser = User;
            ProxyPassword = Password;
            SecureProxyConnection = Secure;
            UpdateWebClientInstance();
        }
        #endregion

        #region Refresh
        public void StartListening(bool StopActiveListening = false)
        {
            if (IsListening)// avoid multiple sessions
            {
                if (StopActiveListening)
                {
                    StopListening();
                }
                else
                {
                    return; // StopListening();
                }
            }
            ConnectWebSocket();
            Timer = new Timer(async (action) =>
            {
                // Do not check the online state ever tick
                if (RefreshCounter > 5)
                {
                    RefreshCounter = 0;
                    await CheckOnlineAsync(3500).ConfigureAwait(false);
                }
                else RefreshCounter++;
                if (IsOnline)
                {
                    List<Task> tasks = new()
                    {
                        //CheckServerOnlineAsync(),
                        //RefreshPrinterStateAsync(),
                        //RefreshCurrentPrintInfosAsync(),
                    };
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                else if (IsListening)
                {
                    StopListening();
                }
            }, null, 0, RefreshInterval * 1000);
            IsListening = true;
        }
        public void StopListening()
        {
            CancelCurrentRequests();
            StopPingTimer();
            StopTimer();

            if (IsListeningToWebsocket)
                DisconnectWebSocket();
            IsListening = false;
        }
        public async Task RefreshAllAsync()
        {
            try
            {
                // Avoid multiple calls
                if (IsRefreshing) return;
                IsRefreshing = true;
                //await RefreshPrinterListAsync();
                List<Task> task = new()
                {
                    //Task.Delay(10),
                    RefreshServerConfigAsync(),
                    RefreshPrinterInfoAsync(),
                    RefreshAvailableFilesAsync(),
                    RefreshJobQueueStatusAsync(),
                    RefreshAvailableFilesAsync(),
                    RefreshDirectoryInformationAsync(),
                    //CheckForServerUpdateAsync(),
                };
                await Task.WhenAll(task).ConfigureAwait(false);
                if (!InitialDataFetched)
                    InitialDataFetched = true;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsRefreshing = false;
        }
        #endregion

        #region Login

        /*
        public void Login(string UserName, SecureString Password, string SessionId, bool remember = true)
        {
            if (string.IsNullOrEmpty(SessionId)) SessionId = this.SessionId;
            if (string.IsNullOrEmpty(SessionId) || !IsListeningToWebsocket)
                throw new Exception($"Current session is null! Please start the Listener first to establish a WebSocket connection!");

            // Password is MD5(sessionId + MD5(login + password))
            string encryptedPassword = EncryptPassword(UserName, Password, SessionId);
            string command =
                $"{{\"action\":\"login\",\"data\":{{\"login\":\"{UserName}\",\"password\":\"{encryptedPassword}\",\"rememberMe\":{(remember ? "true" : "false")}}},\"printer\":\"{GetActivePrinterSlug()}\",\"callback_id\":{99}}}";
            if (WebSocket.State == WebSocketState.Open)
            {
                WebSocket.Send(command);
            }
        }

        public async Task LogoutAsync()
        {
            if (string.IsNullOrEmpty(SessionId) || !IsListeningToWebsocket)
                throw new Exception($"Current session is null! Please start the Listener first to establish a WebSocket connection!");
            _ = await SendRestApiRequestAsync("", "logout").ConfigureAwait(false);
        }

        public void Logout()
        {
            if (string.IsNullOrEmpty(SessionId) || !IsListeningToWebsocket)
                throw new Exception($"Current session is null! Please start the Listener first to establish a WebSocket connection!");

            string command =
                $"{{\"action\":\"logout\",\"data\":{{}},\"printer\":\"{GetActivePrinterSlug()}\",\"callback_id\":{PingCounter++}}}";

            if (WebSocket.State == WebSocketState.Open)
            {
                WebSocket.Send(command);
            }
        }
        */

        string EncryptPassword(string UserName, SecureString Password, string SessionId)
        {
            // Password is MD5(sessionId + MD5(login + password))
            // Source: https://www.godo.dev/tutorials/csharp-md5/
            using MD5 md5 = MD5.Create();
            string credentials = $"{UserName}{SecureStringHelper.ConvertToString(Password)}";
            // Hash credentials first
            md5.ComputeHash(Encoding.UTF8.GetBytes(credentials));
            List<byte> inputBuffer = Encoding.UTF8.GetBytes(SessionId).ToList();

            string hexHash = BitConverter.ToString(md5.Hash).Replace("-", string.Empty).ToLowerInvariant();
            inputBuffer.AddRange(Encoding.UTF8.GetBytes(hexHash));

            md5.ComputeHash(inputBuffer.ToArray());

            // Get hash result after compute it  
            byte[] hashedCredentials = md5
                .Hash;

            StringBuilder strBuilder = new();
            for (int i = 0; i < hashedCredentials.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(hashedCredentials[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
        #endregion

        #region Cancel
        public void CancelCurrentRequests()
        {
            try
            {
                if (client != null)
                {
                    client.CancelPendingRequests();
                    UpdateWebClientInstance();
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #region CheckOnline

        public async Task<bool> CheckOnlineWithApiCallAsync(int Timeout = 10000)
        {
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                if (IsReady)
                {
                    // Send an empty command to check the respone
                    string pingCommand = "{}";
                    KlipperApiRequestRespone respone = await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "ping", pingCommand, Timeout).ConfigureAwait(false);
                    if (respone != null)
                        isReachable = respone.IsOnline;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsConnecting = false;
            IsOnline = isReachable;
            return isReachable;
        }

        public async Task CheckOnlineAsync(int Timeout = 10000)
        {
            CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, Timeout));
            await CheckOnlineAsync(cts).ConfigureAwait(false);
        }

        public async Task CheckOnlineAsync(CancellationTokenSource cts)
        {
            if (IsConnecting) return; // Avoid multiple calls
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                // Cancel after timeout
                //var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, Timeout));
                //string uriString = string.Format("{0}{1}:{2}", httpProtocol, ServerAddress, Port);
                string uriString = FullWebAddress; // $"{(IsSecure ? "https" : "http")}://{ServerAddress}:{Port}";
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uriString, cts.Token).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    if (response != null)
                    {
                        isReachable = response.IsSuccessStatusCode;
                    }
                }
                catch (InvalidOperationException iexc)
                {
                    OnError(new UnhandledExceptionEventArgs(iexc, false));
                }
                catch (HttpRequestException rexc)
                {
                    OnError(new UnhandledExceptionEventArgs(rexc, false));
                }
                catch (TaskCanceledException)
                {
                    // Throws an exception on timeout, not actually an error
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsConnecting = false;
            // Avoid offline message for short connection loss
            if (isReachable || _retries > RetriesWhenOffline)
            {
                _retries = 0;
                IsOnline = isReachable;
            }
            else
            {
                // Retry with shorter timeout to see if the connection loss is real
                _retries++;
                await CheckOnlineAsync(2000).ConfigureAwait(false);
            }
        }

        public async Task<bool> CheckIfApiIsValidAsync(int Timeout = 10000)
        {
            try
            {
                if (IsOnline)
                {
                    // Send an empty command to check the respone
                    string pingCommand = "{}";
                    KlipperApiRequestRespone respone = await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "ping", pingCommand, Timeout).ConfigureAwait(false);
                    if (respone.HasAuthenticationError)
                    {
                        AuthenticationFailed = true;
                        OnRestApiAuthenticationError(respone.EventArgs);
                    }
                    else
                    {
                        AuthenticationFailed = false;
                        OnRestApiAuthenticationSucceeded(respone.EventArgs);
                    }
                    return AuthenticationFailed;
                }
                else
                    return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task CheckServerIfApiIsValidAsync(int Timeout = 10000)
        {
            await CheckIfApiIsValidAsync(Timeout).ConfigureAwait(false);
        }
        #endregion

        #region WebCam
        public string GetWebCamUri(int index = 0)
        {
            try
            {
                return $"{FullWebAddress}/webcam/?action=stream?t={(!string.IsNullOrEmpty(API) ? API : SessionId)}";
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return "";
            }
        }
        #endregion

        #region Updates

        #endregion

        #region DetectChanges
        public bool CheckIfConfigurationHasChanged(KlipperClient temp)
        {
            try
            {
                if (temp == null) return false;
                else return
                    !(ServerAddress == temp.ServerAddress &&
                        Port == temp.Port &&
                        API == temp.API &&
                        IsSecure == temp.IsSecure
                        )
                    ;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region AccessToken
        public async Task<KlipperAccessTokenResult> GetOneshotTokenAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperAccessTokenResult resultObject = null;
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.GET, "oneshot_token").ConfigureAwait(false);
                KlipperAccessTokenResult accessToken = JsonConvert.DeserializeObject<KlipperAccessTokenResult>(result.Result);
                SessionId = accessToken?.Result;
                return accessToken;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperAccessTokenResult> GetApiKeyAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperAccessTokenResult resultObject = null;
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.GET, "api_key").ConfigureAwait(false);
                KlipperAccessTokenResult accessToken = JsonConvert.DeserializeObject<KlipperAccessTokenResult>(result.Result);
                SessionId = accessToken?.Result;
                return accessToken;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        #endregion

        #region Printer Administration

        public async Task RefreshPrinterInfoAsync()
        {
            try
            {
                KlipperPrinterStateMessageResult result = await GetPrinterInfoAsync().ConfigureAwait(false);
                PrinterInfo = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                PrinterInfo = null;
            }
        }

        public async Task<KlipperPrinterStateMessageResult> GetPrinterInfoAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperPrinterStateMessageResult resultObject = null;
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.GET, "info")
                    .ConfigureAwait(false);

                KlipperPrinterStateMessageRespone state = JsonConvert.DeserializeObject<KlipperPrinterStateMessageRespone>(result.Result);
                return state?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<bool> EmergencyStopPrinterAsync()
        {
            try
            {
                //object cmd = new { name = ScriptName };
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "emergency_stop")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> RestartPrinterAsync()
        {
            try
            {
                //object cmd = new { name = ScriptName };
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "restart")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> RestartFirmwareAsync()
        {
            try
            {
                //object cmd = new { name = ScriptName };
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "firmware_restart")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetHeaterBedTargetAsync(int target)
        {
            try
            {
                string cmd = $"SET_HEATER_TEMPERATURE HEATER=heater_bed TARGET={target}";
                bool result = await SendGcodeCommandAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetExtruderTargetAsync(int target, int extruder = 0)
        {
            try
            {
                string cmd = $"SET_HEATER_TEMPERATURE HEATER=extruder{(extruder <= 0 ? "" : $"{extruder}")} TARGET={target}";
                bool result = await SendGcodeCommandAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetFanSpeedTargetAsync(int target, bool isPercentage, int fanId = 0)
        {
            try
            {
                int setSpeed = target;
                if (!isPercentage)
                {
                    // Avoid invalid ranges
                    if (target > 255)
                        setSpeed = 255;
                    else if (target < 0)
                        setSpeed = 0;
                }
                else
                    setSpeed = Convert.ToInt32((target) * 255f / 100f);
                string cmd = $"M106 S{setSpeed}";
                bool result = await SendGcodeCommandAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Printer Status
        public async Task<List<string>> GetPrinterObjectListAsync()
        {
            KlipperApiRequestRespone result = new();
            List<string> resultObject = new();
            try
            {
                //object cmd = new { name = ScriptName };
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.GET, "objects/list")
                    .ConfigureAwait(false);

                KlipperActionListRespone state = JsonConvert.DeserializeObject<KlipperActionListRespone>(result.Result);
                return state?.Result?.Objects;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task RefreshPrinterStateAsync()
        {
            try
            {
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add("webhooks", "");
                queryObjects.Add("virtual_sdcarde", "");
                queryObjects.Add("toolhead", "");
                queryObjects.Add("extruder", "");
                queryObjects.Add("heater_bed", "");
                queryObjects.Add("fan", "");
                queryObjects.Add("print_stats", "");

                KlipperPrinterStatus result = await QueryPrinterObjectStatusAsync(queryObjects).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        public async Task<KlipperPrinterStatus> QueryPrinterObjectStatusAsync(Dictionary<string, string> objects)
        {
            KlipperApiRequestRespone result = new();
            KlipperPrinterStatus resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                foreach (KeyValuePair<string, string> obj in objects)
                {
                    // Do not query macros here, there is an extra method for this.
                    if (obj.Key.StartsWith("gcode_macro")) continue;
                    urlSegments.Add(obj.Key, obj.Value);
                }
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.GET, "objects/query", "", 10000, urlSegments)
                    .ConfigureAwait(false);

                KlipperPrinterStatusQueryRespone queryResult = JsonConvert.DeserializeObject<KlipperPrinterStatusQueryRespone>(result.Result);
                return queryResult?.Result?.Status;

            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, KlipperGcodeMacro>> GetGcodeMacrosAsync(Dictionary<string, string> macros)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, KlipperGcodeMacro> resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                foreach (KeyValuePair<string, string> obj in macros)
                {
                    urlSegments.Add(obj.Key, obj.Value);
                }
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.GET, "objects/query", "", 10000, urlSegments)
                    .ConfigureAwait(false);

                KlipperGcodeMacroRespone queryResult = JsonConvert.DeserializeObject<KlipperGcodeMacroRespone>(result.Result);
                return queryResult?.Result?.Status;

            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperStatusPrintStats> GetPrintStatusAsync()
        {
            KlipperStatusPrintStats resultObject = null;
            try
            {
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add("print_stats", "");

                KlipperPrinterStatus result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                return result?.PrintStats;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperStatusExtruder> GetExtruderStatusAsync(int index = 0)
        {
            KlipperStatusExtruder resultObject = null;
            try
            {
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add($"extruder{(index > 0 ? index : "")}", "");

                KlipperPrinterStatus result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                resultObject = index switch
                {
                    1 => result.Extruder1,
                    2 => result.Extruder2,
                    3 => result.Extruder3,
                    _ => result.Extruder,
                };
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperStatusFan> GetFanStatusAsync()
        {
            KlipperStatusFan resultObject = null;
            try
            {
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add("fan", "");

                KlipperPrinterStatus result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                return result?.Fan;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<string> SubscribePrinterObjectStatusAsync(long? connectionId, List<string> objects)
        {
            return await SubscribePrinterObjectStatusAsync((long)connectionId, objects).ConfigureAwait(false);
        }
        public async Task<string> SubscribePrinterObjectStatusAsync(long connectionId, List<string> objects)
        {
            KlipperApiRequestRespone result = new();
            string resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("connection_id", $"{connectionId}");
                //urlSegments.Add("objects", string.Join("&", objects));
                for (int i = 0; i < objects.Count; i++)
                {
                    urlSegments.Add(objects[i], string.Empty);
                }

                // POST /printer/objects/subscribe?connection_id=123456789&gcode_move&extruder`
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "objects/subscribe", "", 10000, urlSegments).ConfigureAwait(false);
                return result?.Result;
                /*
                KlipperPrinterStatusSubscriptionRespone queryResult = JsonConvert.DeserializeObject<KlipperPrinterStatusSubscriptionRespone>(result.Result);
                return queryResult?.Result?.Status?.Objects;
                */
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<string> SubscribeAllPrinterObjectStatusAsync(long? connectionId)
        {
            List<string> objects = await GetPrinterObjectListAsync().ConfigureAwait(false);
            return await SubscribePrinterObjectStatusAsync((long)connectionId, objects).ConfigureAwait(false);
        }
        public async Task<KlipperEndstopQueryResult> QueryEndstopsAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperEndstopQueryResult resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.GET, "query_endstops/status").ConfigureAwait(false);
                KlipperEndstopQueryRespone queryResult = JsonConvert.DeserializeObject<KlipperEndstopQueryRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #region ServerConfig
        public async Task RefreshServerConfigAsync()
        {
            try
            {
                KlipperServerConfig result = await GetServerConfigAsync().ConfigureAwait(false);
                Config = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Config = null;
            }
        }

        public async Task<KlipperServerConfig> GetServerConfigAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperServerConfig resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, "config").ConfigureAwait(false);
                KlipperServerConfigRespone config = JsonConvert.DeserializeObject<KlipperServerConfigRespone>(result.Result);
                return config?.Result?.Config;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperServerTempData> GetServerCachedTemperatureDataAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperServerTempData resultObject = null;
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, "temperature_store").ConfigureAwait(false);
                KlipperServerTempDataRespone tempData = JsonConvert.DeserializeObject<KlipperServerTempDataRespone>(result.Result);
                return tempData?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<List<KlipperGcode>> GetServerCachedGcodesAsync(long count = 100)
        {
            KlipperApiRequestRespone result = new();
            List<KlipperGcode> resultObject = new();
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, $"gcode_store?count={count}").ConfigureAwait(false);
                KlipperGcodesRespone tempData = JsonConvert.DeserializeObject<KlipperGcodesRespone>(result.Result);
                return tempData?.Result?.Gcodes;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<bool> RestartServerAsync()
        {
            try
            {
                //object cmd = new { name = ScriptName };
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.POST, "restart")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region WebSocket

        public void SendWebSocketCommand(string command)
        {
            try
            {
                //string infoCommand = $"{{\"jsonrpc\":\"2.0\",\"method\":\"server.info\",\"params\":{{}},\"id\":1}}";
                if (WebSocket?.State == WebSocketState.Open)
                {
                    WebSocket.Send(command);
                }
            }
            catch (Exception exc)
            {
                OnWebSocketError(new ErrorEventArgs(exc));
            }
        }

        /* Not available for HTTP
        public async Task<KlipperAccessTokenResult> GetWebSocketIdAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperAccessTokenResult resultObject = null;
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, "websocket_id").ConfigureAwait(false);
                KlipperAccessTokenResult accessToken = JsonConvert.DeserializeObject<KlipperAccessTokenResult>(result.Result);
                SessionId = accessToken?.Result;
                return accessToken;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipperJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        */
        #endregion

        #region Gcode API
        public async Task<bool> RunGcodeScriptAsync(string script)
        {
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("script", script);

                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "gcode/script", "", 10000, urlSegements)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<Dictionary<string, string>> GetGcodeHelpAsync()
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, string> resultObject = new();
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.GET, "gcode/help").ConfigureAwait(false);
                KlipperGcodeHelpRespone config = JsonConvert.DeserializeObject<KlipperGcodeHelpRespone>(result.Result);
                return config?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #region Print Management
        public async Task<bool> PrintFileAsync(string fileName)
        {
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("filename", fileName);

                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "print/start", "", 10000, urlSegements)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> PausePrintAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "print/pause")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> ResumePrintAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "print/resume")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> CancelPrintAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.printer, Method.POST, "print/resume")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        #endregion

        #region Movement
        public async Task<bool> HomeAxesAsync(bool X, bool Y, bool Z)
        {
            try
            {
                bool result = false;
                if (X && Y && Z)
                {
                    result = await RunGcodeScriptAsync("G28").ConfigureAwait(false);
                }
                else
                {
                    string cmd = string.Format("G28{0}{1}{2}", X ? " X0 " : "", Y ? " Y0 " : "", Z ? " Z0 " : "");
                    result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                }
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return false;
        }

        public async Task<bool> MoveAxesAsync(
            double speed = 6000,
            double x = double.PositiveInfinity,
            double y = double.PositiveInfinity,
            double z = double.PositiveInfinity,
            double e = double.PositiveInfinity,
            bool relative = false)
        {
            try
            {
                //string gcode = $"G1 X+10 F{Speed}";
                StringBuilder gcodeCommand = new();
                if (x != double.PositiveInfinity) gcodeCommand.Append($"G1 X{(relative ? "" : "+")}{x} F{speed};");
                if (y != double.PositiveInfinity) gcodeCommand.Append($"G1 Y{(relative ? "" : "+")}{y} F{speed};");
                if (z != double.PositiveInfinity) gcodeCommand.Append($"G1 Y{(relative ? "" : "+")}{z} F{speed};");
                if (e != double.PositiveInfinity) gcodeCommand.Append($"G1 E+{e} F{speed};");

                bool result = await RunGcodeScriptAsync(gcodeCommand.ToString()).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return false;
        }

        #endregion

        #region Machine Commands
        public async Task<KlipperMachineInfo> GetMachineSystemInfoAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperMachineInfo resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.GET, "system_info").ConfigureAwait(false);
                KlipperMachineInfoRespone config = JsonConvert.DeserializeObject<KlipperMachineInfoRespone>(result.Result);
                return config?.Result?.SystemInfo;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<bool> MachineShutdownAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, "shutdown")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> MachineRebootAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, "reboot")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> RestartSystemServiceAsync(string service)
        {
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("service", service);

                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, "services/restart", "", 10000, urlSegements)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> RestartSystemServiceAsync(KlipperServices service)
        {
            return await RestartSystemServiceAsync(service.ToString()).ConfigureAwait(false);
        }

        public async Task<bool> StopSystemServiceAsync(string service)
        {
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("service", service);

                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, "services/stop", "", 10000, urlSegements)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> StopSystemServiceAsync(KlipperServices service)
        {
            return await StopSystemServiceAsync(service.ToString()).ConfigureAwait(false);
        }

        public async Task<bool> StartSystemServiceAsync(string service)
        {
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("service", service);

                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, "services/start", "", 10000, urlSegements)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> StartSystemServiceAsync(KlipperServices service)
        {
            return await StartSystemServiceAsync(service.ToString()).ConfigureAwait(false);
        }

        public async Task<KlipperMoonrakerProcessStatsResult> GetMoonrakerProcessStatsAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperMoonrakerProcessStatsResult resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.GET, "proc_stats").ConfigureAwait(false);
                KlipperMoonrakerProcessStatsRespone config = JsonConvert.DeserializeObject<KlipperMoonrakerProcessStatsRespone>(result.Result);
                return config?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #region File Operations
        // Doc: https://github.com/Arksine/moonraker/blob/master/docs/web_api.md#list-available-files

        public async Task RefreshAvailableFilesAsync(string rootPath = "", bool includeGcodeMeta = true)
        {
            try
            {
                ObservableCollection<KlipperFile> files = new();
                Files = await GetAvailableFilesAsync(rootPath, includeGcodeMeta).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Files = new ObservableCollection<KlipperFile>();
            }
        }

        public async Task<ObservableCollection<KlipperFile>> GetAvailableFilesAsync(string rootPath = "", bool includeGcodeMeta = true)
        {
            KlipperApiRequestRespone result = new();
            ObservableCollection<KlipperFile> resultObject = new();
            try
            {
                Dictionary<string, string> urlSegements = new();
                if (!string.IsNullOrEmpty(rootPath))
                {
                    urlSegements.Add("root", rootPath);
                }

                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, "files/list", "", 10000, urlSegements).ConfigureAwait(false);
                KlipperFileListRespone files = JsonConvert.DeserializeObject<KlipperFileListRespone>(result.Result);
                if (includeGcodeMeta)
                {
                    for (int i = 0; i < files?.Result?.Count; i++)
                    {
                        KlipperFile current = files?.Result[i];
                        current.GcodeMeta = await GetGcodeMetadataAsync(current.Path).ConfigureAwait(false);
                        if (current.GcodeMeta.Thumbnails?.Count > 0)
                        {
                            current.Image = GetGcodeThumbnailImage(current.GcodeMeta, 1);
                        }
                    }
                }
                return new(files?.Result);
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<List<KlipperFile>> GetAvailableFilesAsListAsync(string rootPath = "")
        {
            List<KlipperFile> resultObject = new();
            try
            {
                ObservableCollection<KlipperFile> result = await GetAvailableFilesAsync(rootPath).ConfigureAwait(false);
                return result?.ToList();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperGcodeMetaResult> GetGcodeMetadataAsync(string fileName)
        {
            KlipperApiRequestRespone result = new();
            KlipperGcodeMetaResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("filename", fileName);

                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, "files/metadata", "", 10000, urlSegements).ConfigureAwait(false);
                KlipperGcodeMetaRespone queryResult = JsonConvert.DeserializeObject<KlipperGcodeMetaRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public byte[] GetGcodeThumbnailImage(string relativePath, int timeout = 10000)
        {
            try
            {
                Uri target = new($"{FullWebAddress}/server/files/gcodes/{relativePath}");
                byte[] thumb = DownloadFileFromUri(target, timeout);

                return thumb;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new byte[0];
            }
        }
        public byte[] GetGcodeThumbnailImage(KlipperGcodeMetaResult gcodeMeta, int index = 0, int timeout = 10000)
        {
            string path = gcodeMeta.Thumbnails.Count > index ?
                gcodeMeta.Thumbnails[index].RelativePath : gcodeMeta.Thumbnails.FirstOrDefault().RelativePath;
            if (string.IsNullOrEmpty(path)) return null;
            return GetGcodeThumbnailImage(path, timeout);
        }
        public async Task RefreshDirectoryInformationAsync(string path = "", bool extended = true)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    path = "gcodes";
                KlipperDirectoryInfoResult result = await GetDirectoryInformationAsync(path, extended).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        public async Task<KlipperDirectoryInfoResult> GetDirectoryInformationAsync(string path, bool extended = true)
        {
            KlipperApiRequestRespone result = new();
            KlipperDirectoryInfoResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("path", path);
                urlSegements.Add("extended", extended ? "true" : "false");

                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, "files/directory", "", 10000, urlSegements).ConfigureAwait(false);

                KlipperDirectoryInfoRespone queryResult = JsonConvert.DeserializeObject<KlipperDirectoryInfoRespone>(result.Result);
                if (queryResult?.Result?.DiskUsage != null)
                {
                    FreeDiskSpace = queryResult.Result.DiskUsage.Free;
                    TotalDiskSpace = queryResult.Result.DiskUsage.Total;
                    UsedDiskSpace = queryResult.Result.DiskUsage.Used;
                }
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperDirectoryActionResult> CreateDirectoryAsync(string directory)
        {
            KlipperApiRequestRespone result = new();
            KlipperDirectoryActionResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("path", directory);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.POST, $"files/directory", "", 10000, urlSegments)
                    .ConfigureAwait(false);

                KlipperDirectoryActionRespone queryResult = JsonConvert.DeserializeObject<KlipperDirectoryActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperDirectoryActionResult> DeleteDirectoryAsync(string directory, bool force = false)
        {
            KlipperApiRequestRespone result = new();
            KlipperDirectoryActionResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("path", directory);
                urlSegments.Add("force", force ? "true" : "false");

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.DELETE, $"files/directory", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperDirectoryActionRespone queryResult = JsonConvert.DeserializeObject<KlipperDirectoryActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperDirectoryActionResult> MoveDirectoryOrFileAsync(string source, string destination)
        {
            KlipperApiRequestRespone result = new();
            KlipperDirectoryActionResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("source", source);
                urlSegments.Add("dest", destination);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.POST, $"files/move", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperDirectoryActionRespone queryResult = JsonConvert.DeserializeObject<KlipperDirectoryActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperDirectoryActionResult> CopyDirectoryOrFileAsync(string source, string destination)
        {
            KlipperApiRequestRespone result = new();
            KlipperDirectoryActionResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("source", source);
                urlSegments.Add("dest", destination);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.POST, $"files/copy", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperDirectoryActionRespone queryResult = JsonConvert.DeserializeObject<KlipperDirectoryActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public byte[] DownloadFile(string relativeFilePath)
        {
            try
            {
                Uri uri = new($"{FullWebAddress}/server/files/{relativeFilePath}");
                byte[] file = DownloadFileFromUri(uri);
                return file;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }

        public async Task<KlipperFileActionResult> UploadFileAsync(string file, string root = "gcodes", string path = "", int timeout = 100000)
        {
            KlipperApiRequestRespone result = new();
            KlipperFileActionResult resultObject = null;
            try
            {
                result = await SendMultipartFormDataFileRestApiRequestAsync(file, root, path, timeout).ConfigureAwait(false);
                KlipperFileActionResult queryResult = JsonConvert.DeserializeObject<KlipperFileActionResult>(result.Result);
                return queryResult;
                //return result?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }

        public async Task<KlipperFileActionResult> UploadFileAsync(string fileName, byte[] file, string root = "gcodes", string path = "", int timeout = 10000)
        {
            KlipperApiRequestRespone result = new();
            KlipperFileActionResult resultObject = null;
            try
            {
                result = await SendMultipartFormDataFileRestApiRequestAsync(fileName, file, root, path, timeout).ConfigureAwait(false);
                KlipperFileActionResult queryResult = JsonConvert.DeserializeObject<KlipperFileActionResult>(result.Result);
                return queryResult;
                //return result?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }

        public async Task<KlipperDirectoryActionResult> DeleteFileAsync(string root, string filePath)
        {
            KlipperApiRequestRespone result = new();
            KlipperDirectoryActionResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.DELETE, $"files/{root}/{filePath}")
                    .ConfigureAwait(false);
                KlipperDirectoryActionRespone queryResult = JsonConvert.DeserializeObject<KlipperDirectoryActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<KlipperDirectoryActionResult> DeleteFileAsync(string filePath)
        {
            KlipperApiRequestRespone result = new();
            KlipperDirectoryActionResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.DELETE, $"files/{filePath}")
                    .ConfigureAwait(false);
                KlipperDirectoryActionRespone queryResult = JsonConvert.DeserializeObject<KlipperDirectoryActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public byte[] DownloadLogFile(KlipperLogFileTypes logType)
        {
            try
            {
                Uri uri = new($"{FullWebAddress}/server/files/{logType.ToString().ToLower()}.log");
                byte[] file = DownloadFileFromUri(uri);
                return file;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }
        #endregion

        #region Authorization

        public async Task<KlipperUserActionResult> LoginUserAsync(string username, string password)
        {
            KlipperApiRequestRespone result = new();
            KlipperUserActionResult resultObject = null;
            try
            {
                object cmd = new
                {
                    username = username,
                    password = password
                };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.POST, "login", cmd).ConfigureAwait(false);
                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);
                UserToken = queryResult?.Result?.Token;
                RefreshToken = queryResult?.Result?.RefreshToken;

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperUserActionResult> RefreshJSONWebTokenAsync(string refreshToken = "")
        {
            KlipperApiRequestRespone result = new();
            KlipperUserActionResult resultObject = null;
            try
            {
                string token = !string.IsNullOrEmpty(refreshToken) ? refreshToken : RefreshToken;
                object cmd = new
                {
                    refresh_token = token,
                };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.POST, "refresh_jwt", cmd).ConfigureAwait(false);
                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);

                UserToken = queryResult?.Result?.Token;
                queryResult.Result.RefreshToken = token;

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperUserActionResult> ResetUserPasswordAsync(string password, string newPassword)
        {
            KlipperApiRequestRespone result = new();
            KlipperUserActionResult resultObject = null;
            try
            {
                object cmd = new
                {
                    password = password,
                    new_password = newPassword,
                };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.POST, "user/password", cmd).ConfigureAwait(false);
                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperUserActionResult> LogoutCurrentUserAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperUserActionResult resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.POST, "logout").ConfigureAwait(false);
                UserToken = string.Empty;

                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperUser> GetCurrentUserAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperUser resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.GET, "user").ConfigureAwait(false);
                KlipperUserRespone queryResult = JsonConvert.DeserializeObject<KlipperUserRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperUserActionResult> CreateUserAsync(string username, string password)
        {
            KlipperApiRequestRespone result = new();
            KlipperUserActionResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("username", username);
                urlSegements.Add("password", password);

                object cmd = new
                {
                    username = username,
                    password = password
                };

                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.POST, "user", cmd).ConfigureAwait(false);
                //return result?.Result;
                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);
                return queryResult?.Result;

            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperUserActionResult> DeleteUserAsync(string username)
        {
            KlipperApiRequestRespone result = new();
            KlipperUserActionResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("username", username);

                object cmd = new
                {
                    username = username,
                };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.DELETE, "user", cmd).ConfigureAwait(false);

                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<List<KlipperUser>> ListAvailableUsersAsync()
        {
            KlipperApiRequestRespone result = new();
            List<KlipperUser> resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.access, Method.GET, "users/list").ConfigureAwait(false);
                KlipperUserListRespone queryResult = JsonConvert.DeserializeObject<KlipperUserListRespone>(result.Result);
                return queryResult?.Result?.Users;

            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #region Database APIs
        public async Task<List<string>> ListDatabaseNamespacesAsync()
        {
            KlipperApiRequestRespone result = new();
            List<string> resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, $"database/list")
                    .ConfigureAwait(false);
                KlipperDatabaseNamespaceListRespone queryResult = JsonConvert.DeserializeObject<KlipperDatabaseNamespaceListRespone>(result.Result);
                return queryResult?.Result?.Namespaces;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<Dictionary<string, object>> GetDatabaseItemAsync(string namespaceName, string key = "")
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, object> resultObject = null;
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("namespace", namespaceName);
                if (!string.IsNullOrEmpty(key)) urlSegements.Add("key", key);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, $"database/item", "", 10000, urlSegements)
                    .ConfigureAwait(false);
                KlipperDatabaseItemRespone queryResult = JsonConvert.DeserializeObject<KlipperDatabaseItemRespone>(result.Result);
                if (queryResult != null)
                {
                    resultObject = new();
                    resultObject.Add($"{namespaceName}{(!string.IsNullOrEmpty(key) ? $"|{key}" : "")}", queryResult?.Result?.Value);
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperDatabaseMainsailValueWebcam> GetWebCamSettingsAsync()
        {
            KlipperDatabaseMainsailValueWebcam resultObject = null;
            try
            {
                Dictionary<string, object> result = await GetDatabaseItemAsync("mainsail", "webcam").ConfigureAwait(false);
                resultObject = JsonConvert.DeserializeObject<KlipperDatabaseMainsailValueWebcam>(result.FirstOrDefault().Value.ToString());
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<List<KlipperDatabaseMainsailValuePreset>> GetDashboardPresetsAsync()
        {
            List<KlipperDatabaseMainsailValuePreset> resultObject = null;
            try
            {
                Dictionary<string, object> result = await GetDatabaseItemAsync("mainsail", "presets").ConfigureAwait(false);
                resultObject = JsonConvert.DeserializeObject<List<KlipperDatabaseMainsailValuePreset>>(result.FirstOrDefault().Value.ToString());
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<List<KlipperDatabaseMainsailValuePreset>> GetMeshHeightMapAsync()
        {
            List<KlipperDatabaseMainsailValuePreset> resultObject = null;
            try
            {
                Dictionary<string, object> result = await GetDatabaseItemAsync("mainsail", "heightmap").ConfigureAwait(false);
                resultObject = JsonConvert.DeserializeObject<List<KlipperDatabaseMainsailValuePreset>>(result.FirstOrDefault().Value.ToString());
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, object>> AddDatabaseItemAsync(string namespaceName, string key, object value)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, object> resultObject = null;
            try
            {
                object cmd = new
                {
                    @namespace = namespaceName,
                    key = key,
                    value = value,
                };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.POST, "database/item", cmd).ConfigureAwait(false);
                KlipperDatabaseItemRespone queryResult = JsonConvert.DeserializeObject<KlipperDatabaseItemRespone>(result.Result);
                if (queryResult != null)
                {
                    resultObject = new();
                    resultObject.Add($"{namespaceName}{(!string.IsNullOrEmpty(key) ? $"|{key}" : "")}", queryResult?.Result?.Value);
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, object>> DeleteDatabaseItemAsync(string namespaceName, string key)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, object> resultObject = null;
            try
            {
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("namespace", namespaceName);
                if (!string.IsNullOrEmpty(key)) urlSegements.Add("key", key);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.DELETE, $"database/item", "", 10000, urlSegements)
                    .ConfigureAwait(false);
                KlipperDatabaseItemRespone queryResult = JsonConvert.DeserializeObject<KlipperDatabaseItemRespone>(result.Result);
                if (queryResult != null)
                {
                    resultObject = new();
                    resultObject.Add($"{namespaceName}{(!string.IsNullOrEmpty(key) ? $"|{key}" : "")}", queryResult?.Result?.Value);
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #region Job Queue APIs

        public async Task RefreshJobQueueStatusAsync()
        {
            try
            {
                ObservableCollection<KlipperJobQueueItem> jobList = new();
                KlipperJobQueueResult result = await GetJobQueueStatusAsync().ConfigureAwait(false);
                JobQueueStatus = result?.QueueState;
                JobList = result != null ? new(result.QueuedJobs) : jobList;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                JobQueueStatus = "";
                JobList = new();
            }
        }

        public async Task<KlipperJobQueueResult> GetJobQueueStatusAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, "job_queue/status").ConfigureAwait(false);

                KlipperJobQueueRespone queryResult = JsonConvert.DeserializeObject<KlipperJobQueueRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperJobQueueResult> EnqueueJobsAsync(string[] jobs)
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                object cmd = new
                {
                    filenames = jobs,
                };
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.POST, "job_queue/job", cmd).ConfigureAwait(false);
                KlipperJobQueueRespone queryResult = JsonConvert.DeserializeObject<KlipperJobQueueRespone>(result.Result);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperJobQueueResult> RemoveAllJobAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("all", "true");

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.DELETE, $"job_queue/job", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperJobQueueRespone queryResult = JsonConvert.DeserializeObject<KlipperJobQueueRespone>(result.Result);

                return queryResult?.Result;
                //return GetQueryResult(result.Result);
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperJobQueueResult> RemoveJobAsync(string[] jobIds)
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("job_ids", string.Join(",", jobIds));

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.DELETE, $"job_queue/job", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperJobQueueRespone queryResult = JsonConvert.DeserializeObject<KlipperJobQueueRespone>(result.Result);

                return queryResult?.Result;
                //return GetQueryResult(result.Result);
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperJobQueueResult> PauseJobQueueAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.POST, $"job_queue/pause")
                    .ConfigureAwait(false);
                KlipperJobQueueRespone queryResult = JsonConvert.DeserializeObject<KlipperJobQueueRespone>(result.Result);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperJobQueueResult> StartJobQueueAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.POST, $"job_queue/start")
                    .ConfigureAwait(false);
                KlipperJobQueueRespone queryResult = JsonConvert.DeserializeObject<KlipperJobQueueRespone>(result.Result);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #region Update Manager API
        public async Task<KlipperUpdateStatusResult> GetUpdateStatusAsync(bool refresh = false)
        {
            KlipperApiRequestRespone result = new();
            KlipperUpdateStatusResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("refresh", refresh ? "true" : "false");

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.GET, $"update/status", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperUpdateStatusRespone queryResult = JsonConvert.DeserializeObject<KlipperUpdateStatusRespone>(result.Result);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<bool> PerformFullUpdateAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"update/full")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> UpdateMoonrakerAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"update/moonraker")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> UpdateKlipperAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"update/klipper")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> UpdateClientAsync(string clientName)
        {
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("name", clientName);

                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"update/client", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> UpdateSystemAsync()
        {
            try
            {
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"update/system")
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> RecoverCorruptRepoAsync(string repoName, bool hard = false)
        {
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("name", repoName);
                urlSegments.Add("hard", hard ? "true" : "false");

                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"update/recover", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Power APIs
        public async Task<List<KlipperDevice>> GetDeviceListAsync()
        {
            KlipperApiRequestRespone result = new();
            List<KlipperDevice> resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.GET, $"device_power/devices")
                    .ConfigureAwait(false);
                KlipperDeviceListRespone queryResult = JsonConvert.DeserializeObject<KlipperDeviceListRespone>(result.Result);

                return queryResult?.Result?.Devices;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, string>> GetDeviceStatusAsync(string device)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, string> resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("device", device);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.GET, $"device_power/device", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperDeviceStatusRespone queryResult = JsonConvert.DeserializeObject<KlipperDeviceStatusRespone>(result.Result);

                return queryResult?.DeviceStates;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, string>> SetDeviceStateAsync(string device, KlipperDeviceActions action)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, string> resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("device", device);
                urlSegments.Add("action", action.ToString().ToLower());

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"device_power/device", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperDeviceStatusRespone queryResult = JsonConvert.DeserializeObject<KlipperDeviceStatusRespone>(result.Result);

                return queryResult?.DeviceStates;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, string>> GetBatchDeviceStatusAsync(string[] devices)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, string> resultObject = null;
            try
            {
                StringBuilder deviceList = new();
                for (int i = 0; i < devices.Length; i++)
                {
                    deviceList.Append(devices[i]);
                    if (i < devices.Length - 1)
                        deviceList.Append("&");
                }

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.GET, $"device_power/status?{deviceList}")
                    .ConfigureAwait(false);
                KlipperDeviceStatusRespone queryResult = JsonConvert.DeserializeObject<KlipperDeviceStatusRespone>(result.Result);

                return queryResult?.DeviceStates;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, string>> SetBatchDeviceOnAsync(string[] devices)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, string> resultObject = null;
            try
            {
                StringBuilder deviceList = new();
                for (int i = 0; i < devices.Length; i++)
                {
                    deviceList.Append(devices[i]);
                    if (i < devices.Length - 1)
                        deviceList.Append("&");
                }

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"device_power/on?{deviceList}")
                    .ConfigureAwait(false);
                KlipperDeviceStatusRespone queryResult = JsonConvert.DeserializeObject<KlipperDeviceStatusRespone>(result.Result);

                return queryResult?.DeviceStates;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<Dictionary<string, string>> SetBatchDeviceOffAsync(string[] devices)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, string> resultObject = null;
            try
            {
                StringBuilder deviceList = new();
                for (int i = 0; i < devices.Length; i++)
                {
                    deviceList.Append(devices[i]);
                    if (i < devices.Length - 1)
                        deviceList.Append("&");
                }

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.machine, Method.POST, $"device_power/off?{deviceList}")
                    .ConfigureAwait(false);
                KlipperDeviceStatusRespone queryResult = JsonConvert.DeserializeObject<KlipperDeviceStatusRespone>(result.Result);

                return queryResult?.DeviceStates;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #region Octoprint API emulation
        public async Task<OctoprintApiVersionResult> GetVersionInfoAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiVersionResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.api, Method.GET, $"version")
                    .ConfigureAwait(false);
                OctoprintApiVersionResult queryResult = JsonConvert.DeserializeObject<OctoprintApiVersionResult>(result.Result);

                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<OctoprintApiServerStatusResult> GetServerStatusAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiServerStatusResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.api, Method.GET, $"server")
                    .ConfigureAwait(false);
                OctoprintApiServerStatusResult queryResult = JsonConvert.DeserializeObject<OctoprintApiServerStatusResult>(result.Result);

                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<OctoprintApiServerStatusResult> GetUserInformationAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiServerStatusResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.api, Method.GET, $"login")
                    .ConfigureAwait(false);
                OctoprintApiServerStatusResult queryResult = JsonConvert.DeserializeObject<OctoprintApiServerStatusResult>(result.Result);

                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<OctoprintApiSettingsResult> GetSettingsAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiSettingsResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.api, Method.GET, $"settings")
                    .ConfigureAwait(false);
                OctoprintApiSettingsResult queryResult = JsonConvert.DeserializeObject<OctoprintApiSettingsResult>(result.Result);

                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<OctoprintApiJobResult> GetJobStatusAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiJobResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.api, Method.GET, $"job")
                    .ConfigureAwait(false);
                OctoprintApiJobResult queryResult = JsonConvert.DeserializeObject<OctoprintApiJobResult>(result.Result);

                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<OctoprintApiPrinterStatusResult> GetPrinterStatusAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiPrinterStatusResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.api, Method.GET, $"printer")
                    .ConfigureAwait(false);
                OctoprintApiPrinterStatusResult queryResult = JsonConvert.DeserializeObject<OctoprintApiPrinterStatusResult>(result.Result);

                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<bool> SendGcodeCommandAsync(string[] commands)
        {
            try
            {
                object cmd = new
                {
                    commands = commands
                };
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.api, Method.POST, "printer/command", cmd)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result, true);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SendGcodeCommandAsync(string command)
        {
            return await SendGcodeCommandAsync(new string[] { command }).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, OctoprintApiPrinter>> GetPrinterProfilesAsync()
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, OctoprintApiPrinter> resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.api, Method.GET, $"printerprofiles")
                    .ConfigureAwait(false);
                OctoprintApiPrinterProfilesResult queryResult = JsonConvert.DeserializeObject<OctoprintApiPrinterProfilesResult>(result.Result);

                return queryResult?.Profiles;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #region History APIs
        public async Task<KlipperHistoryResult> GetHistoryJobListAsync(int limit = 100, int start = 0, double since = -1, double before = -1, string order = "asc")
        {
            KlipperApiRequestRespone result = new();
            KlipperHistoryResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("limit", $"{limit}");
                urlSegments.Add("start", $"{start}");
                if (since >= 0) urlSegments.Add("since", $"{since}");
                if (before >= 0) urlSegments.Add("before", $"{before}");
                urlSegments.Add("order", order);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, $"history/list", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperHistoryRespone queryResult = JsonConvert.DeserializeObject<KlipperHistoryRespone>(result.Result);

                return queryResult?.Result;
                //return GetQueryResult(result.Result);
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<KlipperHistoryJobTotalsResult> GetHistoryTotalJobsAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperHistoryJobTotalsResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, $"history/totals")
                    .ConfigureAwait(false);
                KlipperHistoryTotalRespone queryResult = JsonConvert.DeserializeObject<KlipperHistoryTotalRespone>(result.Result);

                return queryResult?.Result?.JobTotals;
                //return GetQueryResult(result.Result);
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<KlipperHistoryJobTotalsResult> ResetHistoryTotalJobsAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperHistoryJobTotalsResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, $"history/reset_totals")
                    .ConfigureAwait(false);
                KlipperHistoryTotalRespone queryResult = JsonConvert.DeserializeObject<KlipperHistoryTotalRespone>(result.Result);

                return queryResult?.Result?.JobTotals;
                //return GetQueryResult(result.Result);
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<KlipperJobItem> GetHistoryJobAsync(string uid)
        {
            KlipperApiRequestRespone result = new();
            KlipperJobItem resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("uid", uid);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.GET, $"history/job", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperHistorySingleJobRespone queryResult = JsonConvert.DeserializeObject<KlipperHistorySingleJobRespone>(result.Result);

                return queryResult?.Result?.Job;
                //return GetQueryResult(result.Result);
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<List<string>> DeleteHistoryJobAsync(string uid)
        {
            KlipperApiRequestRespone result = new();
            List<string> resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("uid", uid);

                result =
                    await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.DELETE, $"history/job", "", 10000, urlSegments)
                    .ConfigureAwait(false);
                KlipperHistoryJobDeletedRespone queryResult = JsonConvert.DeserializeObject<KlipperHistoryJobDeletedRespone>(result.Result);

                return queryResult?.Result?.DeletedJobs;
                //return GetQueryResult(result.Result);
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Overrides
        public override string ToString()
        {
            try
            {
                return FullWebAddress;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return string.Empty;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is not KlipperClient item)
                return false;
            return Id.Equals(item.Id);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
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
                StopListening();
                DisconnectWebSocket();
            }
        }
        #endregion
    }
}
