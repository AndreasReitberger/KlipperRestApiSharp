using AndreasReitberger.Core.Enums;
using AndreasReitberger.Core.Interfaces;
using AndreasReitberger.Core.Utilities;
using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Moonraker.Models;
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

namespace AndreasReitberger.API.Moonraker
{
    // Needs: https://github.com/Arksine/moonraker/blob/master/docs/web_api.md
    // Docs: https://moonraker.readthedocs.io/en/latest/configuration/
    public partial class MoonrakerClient : IRestApiClient
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Variables
        RestClient restClient;
        HttpClient httpClient;
        int _retries = 0;

        readonly bool _enableCooldown = true;
        readonly int _cooldownFallback = 4;

        int _cooldownExtruder = 4;
        int _cooldownTemperatureSensor = 4;
        int _cooldownCpuUsage = 4;
        int _cooldownSystemMemory = 4;
        int _cooldownHeaterBed = 4;

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
        static MoonrakerClient _instance = null;
        static readonly object Lock = new();
        public static MoonrakerClient Instance
        {
            get
            {
                lock (Lock)
                {
                    _instance ??= new MoonrakerClient();
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

        #region Debug
        [JsonIgnore]
        [XmlIgnore]
        Dictionary<string, string> _ignoredJsonResults = new();
        [JsonIgnore]
        [XmlIgnore]
        public Dictionary<string, string> IgnoredJsonResults
        {
            get => _ignoredJsonResults;
            set
            {
                if (_ignoredJsonResults == value) return;
                _ignoredJsonResults = value;
                OnKlipperIgnoredJsonResultsChanged(new KlipperIgnoredJsonResultsChangedEventArgs()
                {
                    NewIgnoredJsonResults = value,
                });
                OnPropertyChanged();
            }
        }
        #endregion

        #region Connection

        [JsonIgnore, XmlIgnore]
        string _sessionId = string.Empty;
        [JsonIgnore, XmlIgnore]
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

        [JsonProperty(nameof(OperatingSystem))]
        MoonrakerOperatingSystems _operatingSystem = MoonrakerOperatingSystems.MainsailOS;
        [JsonIgnore, XmlIgnore]
        public MoonrakerOperatingSystems OperatingSystem
        {
            get => _operatingSystem;
            set
            {
                if (_operatingSystem == value) return;
                _operatingSystem = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(HostName))]
        string _hostName = string.Empty;
        [JsonIgnore]
        public string HostName
        {
            get => _hostName;
            set
            {
                if (_hostName == value) return;
                _hostName = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ServerName))]
        string _serverName= string.Empty;
        [JsonIgnore]
        public string ServerName
        {
            get => _serverName;
            set
            {
                if (_serverName == value) return;
                _serverName = value;
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
                UpdateRestClientInstance();
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
                UpdateRestClientInstance();
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
        int _port = 80;
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
                    UpdateRestClientInstance();
                }
            }
        }

        [JsonProperty(nameof(DefaultTimeout))]
        int _defaultTimeout = 10000;
        [JsonIgnore]
        public int DefaultTimeout
        {
            get => _defaultTimeout;
            set
            {
                if (_defaultTimeout != value)
                {
                    _defaultTimeout = value;
                    OnPropertyChanged();
                }
            }
        }

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

        [JsonIgnore, XmlIgnore]
        bool _isOnline = false;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        bool _isConnecting = false;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        bool _authenticationFailed = false;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        bool _isRefreshing = false;
        [JsonIgnore, XmlIgnore]
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

        [JsonProperty(nameof(RefreshHeatersDirectly)), XmlAttribute(nameof(RefreshHeatersDirectly))]
        bool _refreshHeatersDirectly = true;
        [JsonIgnore, XmlIgnore]
        public bool RefreshHeatersDirectly
        {
            get => _refreshHeatersDirectly;
            private set
            {
                if (_refreshHeatersDirectly == value) return;
                _refreshHeatersDirectly = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Api & Version

        [JsonIgnore]
        [XmlIgnore]
        string _moonrakerVersion = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string MoonrakerVersion
        {
            get => _moonrakerVersion;
            set
            {
                if (_moonrakerVersion == value) return;
                _moonrakerVersion = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        List<string> _registeredDirectories = new();
        [JsonIgnore]
        [XmlIgnore]
        public List<string> RegisteredDirectories
        {
            get => _registeredDirectories;
            set
            {
                if (_registeredDirectories == value) return;
                _registeredDirectories = value;
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
                //UpdateWebClientInstance();
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
                //UpdateWebClientInstance();
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

        public Task DeleteHistoryListItemAsync(KlipperJobItem item)
        {
            throw new NotImplementedException();
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

        #region RemotePrinters
        [JsonIgnore, XmlIgnore]
        ObservableCollection<KlipperDatabaseRemotePrinter> _printers = new();
        [JsonIgnore, XmlIgnore]
        public ObservableCollection<KlipperDatabaseRemotePrinter> Printers
        {
            get => _printers;
            set
            {
                if (_printers == value) return;
                _printers = value;
                OnKlipperRemotePrinterChanged(new KlipperRemotePrintersChangedEventArgs()
                {
                    NewPrinters = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Token = !string.IsNullOrEmpty(UserToken) ? UserToken : API,
                });
                OnPropertyChanged();
            }
        }
        #endregion

        #region Files
        [JsonIgnore, XmlIgnore]
        ObservableCollection<KlipperFile> _files = new();
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        ObservableCollection<KlipperDirectory> _availableDirectories = new();
        [JsonIgnore, XmlIgnore]
        public ObservableCollection<KlipperDirectory> AvailableDirectories
        {
            get => _availableDirectories;
            set
            {
                if (_availableDirectories == value) return;
                _availableDirectories = value;
                /*
                OnKlipperFilesChanged(new KlipperFilesChangedEventArgs()
                {
                    NewFiles = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Token = !string.IsNullOrEmpty(UserToken) ? UserToken : API,
                });
                */
                OnPropertyChanged();
            }
        }
        #endregion

        #region Jobs
        [JsonIgnore, XmlIgnore]
        KlipperStatusJob _jobStatus;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusJob JobStatus
        {
            get => _jobStatus;
            set
            {
                if (_jobStatus == value) return;
                OnKlipperJobStatusChanged(new KlipperJobStatusChangedEventArgs()
                {
                    NewJobStatus = value,
                    PreviousJobStatus = _jobStatus,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Token = !string.IsNullOrEmpty(UserToken) ? UserToken : API,
                });
                _jobStatus = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        string _jobListState = string.Empty;
        [JsonIgnore]
        [XmlIgnore]
        public string JobListState
        {
            get => _jobListState;
            set
            {
                if (_jobListState == value) return;
                OnKlipperJobListStateChanged(new KlipperJobListStateChangedEventArgs()
                {
                    NewJobListStatus = value,
                    PreviousJobListStatus = _jobListState,
                    SessonId = SessionId,
                    CallbackId = -1,
                    Token = !string.IsNullOrEmpty(UserToken) ? UserToken : API,
                });
                _jobListState = value;
                OnPropertyChanged();
            }
        }
        
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
                    SessonId = SessionId,
                    CallbackId = -1,
                    Token = !string.IsNullOrEmpty(UserToken) ? UserToken : API,
                });
                OnPropertyChanged();
            }
        }
        #endregion

        #region State & Config
        [JsonIgnore, XmlIgnore]
        string _klipperState;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        double _cpuTemp = 0;
        [JsonIgnore, XmlIgnore]
        public double CpuTemp
        {
            get => _cpuTemp;
            set
            {
                if (_cpuTemp == value) return;
                _cpuTemp = value;
                OnKlipperCpuTemperatureChanged(new KlipperCpuTemperatureChangedEventArgs()
                {
                    NewTemperature = value,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperServerConfig _config;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        Dictionary<string, KlipperTemperatureSensorHistory> _temperatureCache = new();
        [JsonIgnore, XmlIgnore]
        public Dictionary<string, KlipperTemperatureSensorHistory> TemperatureCache
        {
            get => _temperatureCache;
            set
            {
                if (_temperatureCache == value) return;
                _temperatureCache = value;
                OnKlipperServerTemperatureCacheChanged(new KlipperTemperatureCacheChangedEventArgs()
                {
                    CachedTemperatures = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperPrinterStateMessageResult _printerInfo;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        KlipperGcodeMetaResult _gcodeMeta;
        [JsonIgnore, XmlIgnore]
        public KlipperGcodeMetaResult GcodeMeta
        {
            get => _gcodeMeta;
            set
            {
                if (_gcodeMeta == value) return;
                _gcodeMeta = value;
                OnKlipperGcodeMetaResultChanged(new KlipperGcodeMetaResultChangedEventArgs()
                {
                    NewResult = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusGcodeMove _gcodeMove;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusGcodeMove GcodeMove
        {
            get => _gcodeMove;
            set
            {
                if (_gcodeMove == value) return;
                _gcodeMove = value;
                OnKlipperGcodeMoveStateChanged(new KlipperGcodeMoveStateChangedEventArgs()
                {
                    NewState = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusVirtualSdcard _virtualSdCard;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusVirtualSdcard VirtualSdCard
        {
            get => _virtualSdCard;
            set
            {
                if (_virtualSdCard == value) return;
                _virtualSdCard = value;
                OnKlipperVirtualSdCardStateChanged(new KlipperVirtualSdCardStateChangedEventArgs()
                {
                    NewState = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [XmlIgnore, JsonIgnore]
        Dictionary<string, KlipperStatusTemperatureSensor> _temperatureSensors = new();
        [XmlIgnore, JsonIgnore]
        public Dictionary<string, KlipperStatusTemperatureSensor> TemperatureSensors
        {
            get => _temperatureSensors;
            set
            {
                if (_temperatureSensors == value) return;
                _temperatureSensors = value;
                // WebSocket is updating this property in a high frequency, so a cooldown can be enabled
                if (_enableCooldown)
                {
                    if (_cooldownTemperatureSensor > 0)
                        _cooldownTemperatureSensor--;
                    else
                    {
                        _cooldownTemperatureSensor = _cooldownFallback;
                        OnKlipperTemperatureSensorStatesChanged(new KlipperTemperatureSensorStatesChangedEventArgs()
                        {
                            TemperatureStates = value,
                            SessonId = SessionId,
                            CallbackId = -1,
                        });
                    }
                }
                else
                {
                    OnKlipperTemperatureSensorStatesChanged(new KlipperTemperatureSensorStatesChangedEventArgs()
                    {
                        TemperatureStates = value,
                        SessonId = SessionId,
                        CallbackId = -1,
                    });
                }
                OnPropertyChanged();
            }
        }


        [JsonIgnore, XmlIgnore]
        Dictionary<string, double?> _cpuUsage = new();
        [JsonIgnore, XmlIgnore]
        public Dictionary<string, double?> CpuUsage
        {
            get => _cpuUsage;
            set
            {
                if (_cpuUsage == value) return;
                _cpuUsage = value;
                // WebSocket is updating this property in a high frequency, so a cooldown can be enabled
                if (_enableCooldown)
                {
                    if (_cooldownCpuUsage > 0)
                        _cooldownCpuUsage--;
                    else
                    {
                        _cooldownCpuUsage = _cooldownFallback;
                        OnKlipperServerCpuUsageChanged(new KlipperCpuUsageChangedEventArgs()
                        {
                            CpuUsage = value,
                            SessonId = SessionId,
                            CallbackId = -1,
                        });
                    }
                }
                else
                {
                    OnKlipperServerCpuUsageChanged(new KlipperCpuUsageChangedEventArgs()
                    {
                        CpuUsage = value,
                        SessonId = SessionId,
                        CallbackId = -1,
                    });
                }
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        Dictionary<string, long?> _systemMemory = new();
        [JsonIgnore, XmlIgnore]
        public Dictionary<string, long?> SystemMemory
        {
            get => _systemMemory;
            set
            {
                if (_systemMemory == value) return;
                _systemMemory = value;
                // WebSocket is updating this property in a high frequency, so a cooldown can be enabled
                if (_enableCooldown)
                {
                    if (_cooldownSystemMemory > 0)
                        _cooldownSystemMemory--;
                    else
                    {
                        _cooldownSystemMemory = _cooldownFallback;
                        OnKlipperServerSystemMemoryChanged(new KlipperSystemMemoryChangedEventArgs()
                        {
                            SystemMemory = value,
                            SessonId = SessionId,
                            CallbackId = -1,
                        });
                    }
                }
                else
                {
                    OnKlipperServerSystemMemoryChanged(new KlipperSystemMemoryChangedEventArgs()
                    {
                        SystemMemory = value,
                        SessonId = SessionId,
                        CallbackId = -1,
                    });
                }
                OnPropertyChanged();
            }
        }

        [XmlIgnore, JsonIgnore]
        Dictionary<int, KlipperStatusExtruder> _extruders = new();
        [XmlIgnore, JsonIgnore]
        public Dictionary<int, KlipperStatusExtruder> Extruders
        {
            get => _extruders;
            set
            {
                if (_extruders == value) return;
                _extruders = value;
                // WebSocket is updating this property in a high frequency, so a cooldown can be enabled
                if (_enableCooldown && !RefreshHeatersDirectly)
                {
                    if (_cooldownExtruder > 0)
                        _cooldownExtruder--;
                    else
                    {
                        _cooldownExtruder = _cooldownFallback;
                        OnKlipperExtruderStatesChanged(new KlipperExtruderStatesChangedEventArgs()
                        {
                            ExtruderStates = value,
                            SessonId = SessionId,
                            CallbackId = -1,
                        });
                    }
                }
                else
                {
                    OnKlipperExtruderStatesChanged(new KlipperExtruderStatesChangedEventArgs()
                    {
                        ExtruderStates = value,
                        SessonId = SessionId,
                        CallbackId = -1,
                    });
                }
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusHeaterBed _heaterBed;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusHeaterBed HeaterBed
        {
            get => _heaterBed;
            set
            {
                if (_heaterBed == value) return;
                _heaterBed = value;
                // WebSocket is updating this property in a high frequency, so a cooldown can be enabled
                if (_enableCooldown && !RefreshHeatersDirectly)
                {
                    if (_cooldownHeaterBed > 0)
                        _cooldownHeaterBed--;
                    else
                    {
                        _cooldownHeaterBed = _cooldownFallback;
                        OnKlipperHeaterBedStateChanged(new KlipperHeaterBedStateChangedEventArgs()
                        {
                            NewHeaterBedState = value,
                            SessonId = SessionId,
                            CallbackId = -1,
                        });
                    }
                }
                else
                {
                    OnKlipperHeaterBedStateChanged(new KlipperHeaterBedStateChangedEventArgs()
                    {
                        NewHeaterBedState = value,
                        SessonId = SessionId,
                        CallbackId = -1,
                    });
                }
                OnPropertyChanged();
            }
        }

        [XmlIgnore, JsonIgnore]
        Dictionary<string, KlipperStatusFan> _fans = new();
        [XmlIgnore, JsonIgnore]
        public Dictionary<string, KlipperStatusFan> Fans
        {
            get => _fans;
            set
            {
                if (_fans == value) return;
                _fans = value;
                // WebSocket is updating this property in a high frequency, so a cooldown can be enabled
                /*
                if (_enableCooldown && !RefreshHeatersDirectly)
                {
                    if (_cooldownExtruder > 0)
                        _cooldownExtruder--;
                    else
                    {
                        _cooldownExtruder = _cooldownFallback;

                        OnKlipperExtruderStatesChanged(new KlipperExtruderStatesChangedEventArgs()
                        {
                            ExtruderStates = value,
                            SessonId = SessionId,
                            CallbackId = -1,
                        });
                    }
                }
                else
                {
                    OnKlipperExtruderStatesChanged(new KlipperExtruderStatesChangedEventArgs()
                    {
                        ExtruderStates = value,
                        SessonId = SessionId,
                        CallbackId = -1,
                    });
                }
                */
                OnPropertyChanged();
            }
        }

        [XmlIgnore, JsonIgnore]
        Dictionary<string, KlipperStatusDriver> _drivers = new();
        [XmlIgnore, JsonIgnore]
        public Dictionary<string, KlipperStatusDriver> Drivers
        {
            get => _drivers;
            set
            {
                if (_drivers == value) return;
                _drivers = value;
                // WebSocket is updating this property in a high frequency, so a cooldown can be enabled
                /*
                if (_enableCooldown && !RefreshHeatersDirectly)
                {
                    if (_cooldownExtruder > 0)
                        _cooldownExtruder--;
                    else
                    {
                        _cooldownExtruder = _cooldownFallback;

                        OnKlipperExtruderStatesChanged(new KlipperExtruderStatesChangedEventArgs()
                        {
                            ExtruderStates = value,
                            SessonId = SessionId,
                            CallbackId = -1,
                        });
                    }
                }
                else
                {
                    OnKlipperExtruderStatesChanged(new KlipperExtruderStatesChangedEventArgs()
                    {
                        ExtruderStates = value,
                        SessonId = SessionId,
                        CallbackId = -1,
                    });
                }
                */
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        bool _isPrinting = false;
        [JsonIgnore, XmlIgnore]
        public bool IsPrinting
        {
            get => _isPrinting;
            set
            {
                if (_isPrinting == value) return;
                _isPrinting = value;
                UpdateCurrentPrintDependencies(value);
                OnKlipperIsPrintingStateChanged(new KlipperIsPrintingStateChangedEventArgs()
                {
                    IsPrinting = value,
                    IsPaused = IsPaused,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        bool _isPaused = false;
        [JsonIgnore, XmlIgnore]
        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (_isPaused == value) return;
                _isPaused = value;
                OnKlipperIsPrintingStateChanged(new KlipperIsPrintingStateChangedEventArgs()
                {
                    IsPrinting = IsPrinting,
                    IsPaused = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusPrintStats _printStats;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusPrintStats PrintStats
        {
            get => _printStats;
            set
            {
                if (_printStats == value) return;
                OnKlipperPrintStateChanged(new KlipperPrintStateChangedEventArgs()
                {
                    NewPrintState = value,
                    PreviousPrintState = _printStats,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                _printStats = value;
                if(_printStats?.ValidPrintState == true)
                {
                    IsPrinting = _printStats?.State == KlipperPrintStates.Printing;
                    ActiveJobName = _printStats?.Filename;
                }
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusFan _fan;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusFan Fan
        {
            get => _fan;
            set
            {
                if (_fan == value) return;
                _fan = value;
                OnKlipperFanStateChanged(new KlipperFanStateChangedEventArgs()
                {
                    NewFanState = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusMotionReport _motionReport;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusMotionReport MotionReport
        {
            get => _motionReport;
            set
            {
                if (_motionReport == value) return;
                OnKlipperMotionReportChanged(new KlipperMotionReportChangedEventArgs()
                {
                    NewState = value,
                    PreviousState = _motionReport,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                _motionReport = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusIdleTimeout _idleState;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusIdleTimeout IdleState
        {
            get => _idleState;
            set
            {
                if (_idleState == value) return;
                OnKlipperIdleStateChanged(new KlipperIdleStateChangedEventArgs()
                {
                    NewState = value,
                    PreviousState = _idleState,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                _idleState = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusToolhead _toolHead;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusToolhead ToolHead
        {
            get => _toolHead;
            set
            {
                if (_toolHead == value) return;
                _toolHead = value;
                OnKlipperToolHeadStateChanged(new KlipperToolHeadStateChangedEventArgs()
                {
                    NewToolheadState = value,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        string _activeJobName;
        [JsonIgnore, XmlIgnore]
        public string ActiveJobName
        {
            get => _activeJobName;
            set
            {
                if (_activeJobName == value) return;
                OnKlipperActiveJobStateChanged(new KlipperActiveJobStateChangedEventArgs()
                {
                    NewJobState = value,
                    PreviousJobState = _activeJobName,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                _activeJobName = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusDisplay _displayStatus;
        [JsonIgnore, XmlIgnore]
        public KlipperStatusDisplay DisplayStatus
        {
            get => _displayStatus;
            set
            {
                if (_displayStatus == value) return;
                OnKlipperDisplayStatusChanged(new KlipperDisplayStatusChangedEventArgs()
                {
                    NewDisplayStatus = value,
                    PreviousDisplayStatus = _displayStatus,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                _displayStatus = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        KlipperStatusFilamentSensor _filamentSensor = new() { FilamentDetected = false };
        [JsonIgnore, XmlIgnore]
        public KlipperStatusFilamentSensor FilamentSensor
        {
            get => _filamentSensor;
            set
            {
                if (_filamentSensor == value) return;
                OnKlipperFSensorChanged(new KlipperFSensorStateChangedEventArgs()
                {
                    NewFSensorState = value,
                    PreviousFSensorState = _filamentSensor,
                    SessonId = SessionId,
                    CallbackId = -1,
                });
                _filamentSensor = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        List<KlipperDatabaseWebcamConfig> _webCamConfigs = new();
        [JsonIgnore, XmlIgnore]
        public List<KlipperDatabaseWebcamConfig> WebCamConfigs
        {
            get => _webCamConfigs;
            set
            {
                if (_webCamConfigs == value) return;
                OnKlipperWebCamConfigChanged(new KlipperWebCamConfigChangedEventArgs()
                {
                    NewConfig = value,
                });
                _webCamConfigs = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore, XmlIgnore]
        List<KlipperDatabaseTemperaturePreset> _presets = new();
        [JsonIgnore, XmlIgnore]
        public List<KlipperDatabaseTemperaturePreset> Presets
        {
            get => _presets;
            set
            {
                if (_presets == value) return;
                OnKlipperPresetsChanged(new KlipperPresetsChangedEventArgs()
                {
                    NewPresets = value,
                });
                _presets = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Database

        [JsonIgnore, XmlIgnore]
        List<string> _availableNamespaces = new();
        [JsonIgnore, XmlIgnore]
        public List<string> AvailableNamespaces
        {
            get => _availableNamespaces;
            set
            {
                if (_availableNamespaces == value) return;
                _availableNamespaces = value;
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
        [JsonIgnore, XmlIgnore]
        WebSocket _webSocket;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        long? _webSocketConnectionId;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        Timer _pingTimer;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        int _pingCounter = 0;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        int _refreshCounter = 0;
        [JsonIgnore, XmlIgnore]
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

        [JsonIgnore, XmlIgnore]
        bool _isListeningToWebSocket = false;
        [JsonIgnore, XmlIgnore]
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
        public MoonrakerClient()
        {
            Id = Guid.NewGuid();
            UpdateRestClientInstance();
        }

        /// <summary>
        /// Creates a new instance of KlipperClient. If using this constructor.
        /// </summary>
        /// <param name="serverAddress">Host address</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="port">Port</param>
        /// <param name="isSecure">True if https is used</param>
        public MoonrakerClient(string serverAddress, string api, int port = 80, bool isSecure = false)
        {
            Id = Guid.NewGuid();
            InitInstance(serverAddress, port, api, isSecure);
            UpdateRestClientInstance();
        }

        /// <summary>
        /// Creates a new instance of KlipperClient. If using this constructor, call LoginUserForApiKey to set the api key afterwards
        /// </summary>
        /// <param name="serverAddress">Host address</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="port">Port</param>
        /// <param name="isSecure">True if https is used</param>
        public MoonrakerClient(string serverAddress, string username, SecureString password, int port = 80, bool isSecure = false)
        {
            Id = Guid.NewGuid();
            InitInstance(serverAddress, port, "", isSecure);
            LoginRequired = true;
            Username = username;
            Password = password;
            UpdateRestClientInstance();
        }
        #endregion

        #region Destructor
        ~MoonrakerClient()
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
        public static void UpdateSingleInstance(MoonrakerClient Inst)
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
        public void InitInstance(string serverAddress, int port = 80, string api = "", bool isSecure = false)
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

        #region Debug
        public event EventHandler<KlipperIgnoredJsonResultsChangedEventArgs> KlipperIgnoredJsonResultsChanged;
        protected virtual void OnKlipperIgnoredJsonResultsChanged(KlipperIgnoredJsonResultsChangedEventArgs e)
        {
            KlipperIgnoredJsonResultsChanged?.Invoke(this, e);
        }
        #endregion

        #region State & Config
        public event EventHandler<KlipperStateChangedEventArgs> KlipperStateChanged;
        protected virtual void OnKlipperStateChanged(KlipperStateChangedEventArgs e)
        {
            KlipperStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperCpuTemperatureChangedEventArgs> KlipperCpuTemperatureChanged;
        protected virtual void OnKlipperCpuTemperatureChanged(KlipperCpuTemperatureChangedEventArgs e)
        {
            KlipperCpuTemperatureChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperServerConfigChangedEventArgs> KlipperServerConfigChanged;
        protected virtual void OnKlipperServerConfigChanged(KlipperServerConfigChangedEventArgs e)
        {
            KlipperServerConfigChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperTemperatureCacheChangedEventArgs> KlipperServerTemperatureCacheChanged;
        protected virtual void OnKlipperServerTemperatureCacheChanged(KlipperTemperatureCacheChangedEventArgs e)
        {
            KlipperServerTemperatureCacheChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperCpuUsageChangedEventArgs> KlipperServerCpuUsageChanged;
        protected virtual void OnKlipperServerCpuUsageChanged(KlipperCpuUsageChangedEventArgs e)
        {
            KlipperServerCpuUsageChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperSystemMemoryChangedEventArgs> KlipperServerSystemMemoryChanged;
        protected virtual void OnKlipperServerSystemMemoryChanged(KlipperSystemMemoryChangedEventArgs e)
        {
            KlipperServerSystemMemoryChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperPrinterInfoChangedEventArgs> KlipperPrinterInfoChanged;
        protected virtual void OnKlipperPrinterInfoChanged(KlipperPrinterInfoChangedEventArgs e)
        {
            KlipperPrinterInfoChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperGcodeMetaResultChangedEventArgs> KlipperGcodeMetaResultChanged;
        protected virtual void OnKlipperGcodeMetaResultChanged(KlipperGcodeMetaResultChangedEventArgs e)
        {
            KlipperGcodeMetaResultChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperGcodeMoveStateChangedEventArgs> KlipperGcodeMoveStateChanged;
        protected virtual void OnKlipperGcodeMoveStateChanged(KlipperGcodeMoveStateChangedEventArgs e)
        {
            KlipperGcodeMoveStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperVirtualSdCardStateChangedEventArgs> KlipperVirtualSdCardStateChanged;
        protected virtual void OnKlipperVirtualSdCardStateChanged(KlipperVirtualSdCardStateChangedEventArgs e)
        {
            KlipperVirtualSdCardStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperExtruderStatesChangedEventArgs> KlipperExtruderStatesChanged;
        protected virtual void OnKlipperExtruderStatesChanged(KlipperExtruderStatesChangedEventArgs e)
        {
            KlipperExtruderStatesChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperTemperatureSensorStatesChangedEventArgs> KlipperTemperatureSensorStatesChanged;
        protected virtual void OnKlipperTemperatureSensorStatesChanged(KlipperTemperatureSensorStatesChangedEventArgs e)
        {
            KlipperTemperatureSensorStatesChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperHeaterBedStateChangedEventArgs> KlipperHeaterBedStateChanged;
        protected virtual void OnKlipperHeaterBedStateChanged(KlipperHeaterBedStateChangedEventArgs e)
        {
            KlipperHeaterBedStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperPrintStateChangedEventArgs> KlipperPrintStateChanged;
        protected virtual void OnKlipperPrintStateChanged(KlipperPrintStateChangedEventArgs e)
        {
            KlipperPrintStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperIsPrintingStateChangedEventArgs> KlipperIsPrintingStateChanged;
        protected virtual void OnKlipperIsPrintingStateChanged(KlipperIsPrintingStateChangedEventArgs e)
        {
            KlipperIsPrintingStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperFanStateChangedEventArgs> KlipperFanStateChanged;
        protected virtual void OnKlipperFanStateChanged(KlipperFanStateChangedEventArgs e)
        {
            KlipperFanStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperMotionReportChangedEventArgs> KlipperMotionReportChanged;
        protected virtual void OnKlipperMotionReportChanged(KlipperMotionReportChangedEventArgs e)
        {
            KlipperMotionReportChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperIdleStateChangedEventArgs> KlipperIdleStateChanged;
        protected virtual void OnKlipperIdleStateChanged(KlipperIdleStateChangedEventArgs e)
        {
            KlipperIdleStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperToolHeadStateChangedEventArgs> KlipperToolHeadStateChanged;
        protected virtual void OnKlipperToolHeadStateChanged(KlipperToolHeadStateChangedEventArgs e)
        {
            KlipperToolHeadStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperActiveJobStateChangedEventArgs> KlipperActiveJobStateChanged;
        protected virtual void OnKlipperActiveJobStateChanged(KlipperActiveJobStateChangedEventArgs e)
        {
            KlipperActiveJobStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperDisplayStatusChangedEventArgs> KlipperDisplayStatusChanged;
        protected virtual void OnKlipperDisplayStatusChanged(KlipperDisplayStatusChangedEventArgs e)
        {
            KlipperDisplayStatusChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperFSensorStateChangedEventArgs> KlipperFSensorChanged;
        protected virtual void OnKlipperFSensorChanged(KlipperFSensorStateChangedEventArgs e)
        {
            KlipperFSensorChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperWebCamConfigChangedEventArgs> KlipperWebCamConfigChanged;
        protected virtual void OnKlipperWebCamConfigChanged(KlipperWebCamConfigChangedEventArgs e)
        {
            KlipperWebCamConfigChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperPresetsChangedEventArgs> KlipperPresetsChanged;
        protected virtual void OnKlipperPresetsChanged(KlipperPresetsChangedEventArgs e)
        {
            KlipperPresetsChanged?.Invoke(this, e);
        }
        #endregion

        #region Remote Printers
        public event EventHandler<KlipperRemotePrintersChangedEventArgs> KlipperRemotePrinterChanged;
        protected virtual void OnKlipperRemotePrinterChanged(KlipperRemotePrintersChangedEventArgs e)
        {
            KlipperRemotePrinterChanged?.Invoke(this, e);
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
        public event EventHandler<KlipperJobListStateChangedEventArgs> KlipperJobListStateChanged;
        protected virtual void OnKlipperJobListStateChanged(KlipperJobListStateChangedEventArgs e)
        {
            KlipperJobListStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperJobStatusChangedEventArgs> KlipperJobStatusChanged;
        protected virtual void OnKlipperJobStatusChanged(KlipperJobStatusChangedEventArgs e)
        {
            KlipperJobStatusChanged?.Invoke(this, e);
        }

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

        #region Login
        public event EventHandler<KlipperLoginEventArgs> LoginChanged;
        protected virtual void OnLoginChanged(KlipperLoginEventArgs e)
        {
            LoginChanged?.Invoke(this, e);
        }
        #endregion

        #endregion

        #region WebSocket
        [Obsolete("Use ConnectWebSocketAsync instead")]
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
                //string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(API) ? $"?token={(LoginRequired ? UserToken : API)}" : "")}";
                //var token = GetOneshotTokenAsync();

                // If logged in, even if passing an api key, the websocket returns 401?!
                // The OneShotToken seems to work in both cases
                string target = LoginRequired ?
                        $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket?token={OneShotToken}" :
                        //$"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(API) ? $"?token={API}" : "")}";
                        $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(OneShotToken) ? $"?token={OneShotToken}" : $"?token={API}")}";
                WebSocket = new WebSocket(target)
                {
                    EnableAutoSendPing = false,
                    
                };
                if(LoginRequired)
                {
                    //WebSocket.Security.Credential = new NetworkCredential(Username, Password);
                }

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
        [Obsolete("Use DisconnectWebSocketAsync instead")]
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
        
        public async Task ConnectWebSocketAsync()
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
                //string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket{(!string.IsNullOrEmpty(API) ? $"?token={(LoginRequired ? UserToken : API)}" : "")}";

                KlipperAccessTokenResult oneshotToken = await GetOneshotTokenAsync();
                OneShotToken = oneshotToken?.Result;

                string target = $"{(IsSecure ? "wss" : "ws")}://{ServerAddress}:{Port}/websocket?token={OneShotToken}";
                WebSocket = new WebSocket(target)
                {
                    EnableAutoSendPing = false,
                    
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

                await WebSocket.OpenAsync();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public async Task DisconnectWebSocketAsync()
        {
            try
            {
                if (WebSocket != null)
                {
                    if (WebSocket.State == WebSocketState.Open)
                    await WebSocket.CloseAsync();
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
            WebSocketConnectionId = -1;
            OnWebSocketError(e);
            OnError(e);
        }

        private void WebSocket_Closed(object sender, EventArgs e)
        {
            IsListeningToWebsocket = false;
            WebSocketConnectionId = -1;
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
                {
                    return;
                }
                if (e.Message.ToLower().Contains("method"))
                {
                    string name = string.Empty;
                    string jsonBody = string.Empty;
                    try
                    {
                        Dictionary<int, KlipperStatusExtruder> extruderStats = new();
                        KlipperWebSocketMessage method = JsonConvert.DeserializeObject<KlipperWebSocketMessage>(e.Message);
                        for (int i = 0; i < method?.Params?.Count; i++)
                        {
                            if (method.Params[i] is not JObject jsonObject)
                            {
                                continue;
                            }
                            // Parse each property individually
                            foreach (JProperty property in jsonObject.Children<JProperty>())
                            {
                                name = property.Name;
                                jsonBody = property.Value.ToString();
                                switch (name)
                                {
                                    case "klippy_state":
                                        KlipperState = jsonBody;
                                        break;
                                    case "probe":
                                        KlipperStatusProbe probe =
                                            JsonConvert.DeserializeObject<KlipperStatusProbe>(jsonBody);
                                        break;
                                    case "virtual_sdcard":
                                        if (!jsonBody.Contains("progress"))
                                        {
                                            //break;
                                        }
                                        KlipperStatusVirtualSdcard virtualSdcardState =
                                            JsonConvert.DeserializeObject<KlipperStatusVirtualSdcard>(jsonBody);
                                        VirtualSdCard = virtualSdcardState;
                                        break;
                                    case "display_status":
                                        if (!jsonBody.Contains("progress"))
                                        {
                                            break;
                                        }
                                        KlipperStatusDisplay displayState =
                                            JsonConvert.DeserializeObject<KlipperStatusDisplay>(jsonBody);
                                        DisplayStatus = displayState;
                                        break;
                                    case "moonraker_stats":
                                        MoonrakerStatInfo notifyProcState =
                                            JsonConvert.DeserializeObject<MoonrakerStatInfo>(jsonBody);
                                        break;
                                    case "mcu":
                                        KlipperStatusMcu mcuState =
                                            JsonConvert.DeserializeObject<KlipperStatusMcu>(jsonBody);
                                        break;
                                    case "system_stats":
                                        KlipperStatusSystemStats systemState =
                                            JsonConvert.DeserializeObject<KlipperStatusSystemStats>(jsonBody);
                                        break;
                                    case "registered_directories":
                                        RegisteredDirectories =
                                            JsonConvert.DeserializeObject<List<string>>(jsonBody);
                                        break;
                                    case "cpu_temp":
                                        CpuTemp =
                                            JsonConvert.DeserializeObject<double>(jsonBody.Replace(",", "."));
                                        break;
                                    case "system_cpu_usage":
                                        var tempUsageObject = JsonConvert.DeserializeObject<Dictionary<string, double?>>(jsonBody);
                                        if (tempUsageObject != null)
                                        {
                                            foreach (var cpuUsageItem in tempUsageObject)
                                            {
                                                string cpuUsageIdentifier = cpuUsageItem.Key;
                                                if (CpuUsage.ContainsKey(cpuUsageIdentifier))
                                                {
                                                    CpuUsage[cpuUsageIdentifier] = cpuUsageItem.Value;
                                                }
                                                else
                                                {
                                                    CpuUsage.Add(cpuUsageIdentifier, cpuUsageItem.Value);
                                                }
                                            }
                                        }
                                        break;
                                    case "system_memory":
                                        var tempMemoryObject = JsonConvert.DeserializeObject<Dictionary<string, long?>>(jsonBody);
                                        if (tempMemoryObject != null)
                                        {
                                            foreach (var memoryUsage in tempMemoryObject)
                                            {
                                                string memoryIdentifier = memoryUsage.Key;
                                                if (SystemMemory.ContainsKey(memoryIdentifier))
                                                {
                                                    SystemMemory[memoryIdentifier] = memoryUsage.Value;
                                                }
                                                else
                                                {
                                                    SystemMemory.Add(memoryIdentifier, memoryUsage.Value);
                                                }
                                            }
                                        }
                                        break;
                                    case "moonraker_version":
                                        MoonrakerVersion = jsonBody;
                                        break;
                                    case "websocket_connections":
                                        int wsConnections =
                                            JsonConvert.DeserializeObject<int>(jsonBody);
                                        break;
                                    case "network":
                                        Dictionary<string, KlipperNetworkInterface> network =
                                            JsonConvert.DeserializeObject<Dictionary<string, KlipperNetworkInterface>>(jsonBody);
                                        break;
                                    case "gcode_move":
                                        KlipperStatusGcodeMove gcodeMoveState =
                                            JsonConvert.DeserializeObject<KlipperStatusGcodeMove>(jsonBody);
                                        GcodeMove = gcodeMoveState;
                                        break;
                                    case "print_stats":
                                        KlipperStatusPrintStats printStats =
                                            JsonConvert.DeserializeObject<KlipperStatusPrintStats>(jsonBody);
                                        printStats.ValidPrintState = jsonBody.Contains("state");
                                        if (PrintStats != null)
                                        {
                                            // This property is only sent once if changed, so store it
                                            if (!jsonBody.Contains("filename"))
                                            {
                                                printStats.Filename = PrintStats.Filename;
                                            }
                                            else
                                            {

                                            }
                                        }
                                        PrintStats = printStats;
                                        break;
                                    case "fan":
                                        KlipperStatusFan fanState =
                                            JsonConvert.DeserializeObject<KlipperStatusFan>(jsonBody);
                                        Fan = fanState;
                                        break;
                                    case "toolhead":
                                        KlipperStatusToolhead toolhead =
                                            JsonConvert.DeserializeObject<KlipperStatusToolhead>(jsonBody);
                                        ToolHead = toolhead;
                                        break;
                                    case "heater_bed":
                                        // In the status report the temp is missing, so do not parse the heater then.
                                        if (!jsonBody.Contains("temperature") || RefreshHeatersDirectly) break;
                                        KlipperStatusHeaterBed heaterBed =
                                            JsonConvert.DeserializeObject<KlipperStatusHeaterBed>(jsonBody);
                                        if (HeaterBed != null)
                                        {
                                            // This property is only sent once if changed, so store it
                                            if (!jsonBody.Contains("target"))
                                            {
                                                heaterBed.Target = HeaterBed.Target;
                                            }
                                            else
                                            {

                                            }
                                        }
                                        HeaterBed = heaterBed;
                                        break;
                                    case "extruder":
                                    case "extruder1":
                                    case "extruder2":
                                    case "extruder3":
                                        // In the status report the temp is missing, so do not parse the heater then.
                                        if (!jsonBody.Contains("temperature") || RefreshHeatersDirectly) break;
                                        int index = 0;
                                        string extruderIndex = name.Replace("extruder", string.Empty);
                                        if(extruderIndex.Length > 0)
                                        {
                                            int.TryParse(extruderIndex, out index);
                                        }
                                        KlipperStatusExtruder extruder =
                                            JsonConvert.DeserializeObject<KlipperStatusExtruder>(jsonBody);
                                        if (Extruders.ContainsKey(index))
                                        {
                                            // This property is only sent once if changed, so store it
                                            KlipperStatusExtruder previousExtruderState = Extruders[index];
                                            if (!jsonBody.Contains("target"))
                                            {
                                                extruder.Target = previousExtruderState.Target;
                                            }
                                        }
                                        extruderStats.Add(index, extruder);
                                        //OnPropertyChanged(nameof(Extruders));
                                        break;
                                    case "motion_report":
                                        KlipperStatusMotionReport motionReport =
                                            JsonConvert.DeserializeObject<KlipperStatusMotionReport>(jsonBody);
                                        MotionReport = motionReport;
                                        break;
                                    case "idle_timeout":
                                        KlipperStatusIdleTimeout idleTimeout =
                                            JsonConvert.DeserializeObject<KlipperStatusIdleTimeout>(jsonBody);
                                        idleTimeout.ValidState = jsonBody.Contains("state");
                                        IdleState = idleTimeout;
                                        break;
                                    case "filament_switch_sensor fsensor":
                                        KlipperStatusFilamentSensor fSensor =
                                            JsonConvert.DeserializeObject<KlipperStatusFilamentSensor>(jsonBody);
                                        FilamentSensor = fSensor;
                                        break;
                                    case "pause_resume":
                                        KlipperStatusPauseResume pauseResume =
                                            JsonConvert.DeserializeObject<KlipperStatusPauseResume>(jsonBody);
                                        IsPaused = pauseResume.IsPaused;
                                        break;
                                    case "action":
                                        string action = jsonBody;
                                        break;
                                    case "bed_mesh":
                                        KlipperStatusMesh mesh =
                                            JsonConvert.DeserializeObject<KlipperStatusMesh>(jsonBody);
                                        break;
                                    case "job":
                                        if (!jsonBody.Contains("filename")) break;
                                        KlipperStatusJob job =
                                            JsonConvert.DeserializeObject<KlipperStatusJob>(jsonBody);
                                        //ActiveJobName = job?.Filename;
                                        JobStatus = job;
                                        if (JobStatus?.Status == KlipperJobStates.Completed)
                                        {
                                            OnJobFinished(new()
                                            {
                                                Job = job,
                                            });
                                        }
                                        break;
                                    case "updated_queue":
                                        if (string.IsNullOrEmpty(jsonBody))
                                        {
                                            JobList = new();
                                            break;
                                        }
                                        List<KlipperJobQueueItem> queueUpdate =
                                            JsonConvert.DeserializeObject<List<KlipperJobQueueItem>>(jsonBody);
                                        JobList = new(queueUpdate);
                                        break;
                                    case "queue_state":
                                        string state = jsonBody;
                                        JobListState = state;
                                        break;
                                    case "remote_printers":
                                        string remotePrinter = jsonBody;
                                        //JobListState = state;
                                        break;

                                    // Not relevant so far
                                    //case "temperature_host raspberry_pi":
                                    case "item":
#if DEBUG
                                        Console.WriteLine($"Ignored Json object: '{name}' => '{jsonBody}");
#endif
                                        break;
                                    default:
                                        bool nameFound = false;
                                        try
                                        {
                                            if (name.StartsWith("heater_fan"))
                                            {
                                                string[] fan = name.Split(' ');
                                                if (fan.Length >= 2)
                                                {
#if NETSTANDARD
                                                    string fanName = fan[^1];
#else
                                                string fanName = fan[fan.Length -1];
#endif
                                                    KlipperStatusFan fanObject = JsonConvert.DeserializeObject<KlipperStatusFan>(jsonBody);
                                                    if (fanObject != null)
                                                    {
                                                        if (Fans.ContainsKey(fanName))
                                                        {
                                                            Fans[fanName] = fanObject;
                                                        }
                                                        else
                                                        {
                                                            Fans.Add(fanName, fanObject);
                                                        }
                                                    }
                                                    nameFound = true;
                                                }
                                            }
                                            else if (name.StartsWith("temperature_sensor") || name.StartsWith("temperature_host"))
                                            {
                                                string[] sensor = name.Split(' ');
                                                if (sensor.Length >= 2)
                                                {
#if NETSTANDARD
                                                    string sensorName = sensor[^1];
#else
                                                    string sensorName = sensor[sensor.Length - 1];
#endif
                                                    KlipperStatusTemperatureSensor tempObject = JsonConvert.DeserializeObject<KlipperStatusTemperatureSensor>(jsonBody);
                                                    if (tempObject != null)
                                                    {
                                                        if (TemperatureSensors.ContainsKey(sensorName))
                                                        {
                                                            TemperatureSensors[sensorName] = tempObject;
                                                        }
                                                        else
                                                        {
                                                            TemperatureSensors.Add(sensorName, tempObject);
                                                        }
                                                        nameFound = true;
                                                    }
                                                }
                                            }
                                            else if (name.StartsWith("tmc2130"))
                                            {
                                                string[] driver = name.Split(' ');
                                                if (driver.Length >= 2)
                                                {
#if NETSTANDARD
                                                    string driverName = driver[^1];
#else
                                                string driverName = driver[driver.Length - 1];
#endif
                                                    KlipperStatusDriverRespone drvObject = JsonConvert.DeserializeObject<KlipperStatusDriverRespone>(jsonBody);
                                                    if (drvObject != null)
                                                    {
                                                        if (Drivers.ContainsKey(driverName))
                                                        {
                                                            Drivers[driverName] = drvObject.DrvStatus;
                                                        }
                                                        else
                                                        {
                                                            Drivers.Add(driverName, drvObject.DrvStatus);
                                                        }
                                                        nameFound = true;
                                                    }
                                                }
                                            }
                                        }
                                        catch (JsonException jecx)
                                        {
                                            OnError(new KlipprtJsonConvertEventArgs()
                                            {
                                                Exception = jecx,
                                                OriginalString = jsonBody,
                                                Message = jecx.Message,
                                            });
                                        }
                                        catch (Exception exc)
                                        {
                                            OnError(new UnhandledExceptionEventArgs(exc, false));
                                        }
                                        if(nameFound)
                                        {
                                            break;
                                        }
#if DEBUG
                                        Console.WriteLine($"No Json object found for '{name}' => '{jsonBody}");
#endif
                                        Dictionary<string, string> loggedResults = new(IgnoredJsonResults);
                                        if (!loggedResults.ContainsKey(name))
                                        {
                                            // Log unused json results for further releases
                                            loggedResults.Add(name, jsonBody);
                                            IgnoredJsonResults = loggedResults;
                                        }
                                        break;
                                }
                            }
                        }

                        // Update extruder states if changed
                        if (extruderStats.Count > 0)
                        {
                            Extruders = extruderStats;
                        }
                    }
                    catch (JsonException jecx)
                    {
                        OnError(new KlipprtJsonConvertEventArgs()
                        {
                            Exception = jecx,
                            OriginalString = jsonBody,
                            TargetType = name,
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
                        if (result?.Result is JObject jsonObject)
                        {
                            foreach (JProperty property in jsonObject.Children<JProperty>())
                            {
                                string name = property.Name;
                                string jsonBody = property.Value.ToString();
                                switch (name)
                                {
                                    case "websocket_id":
                                        long wsId =
                                            JsonConvert.DeserializeObject<long>(jsonBody);
                                        WebSocketConnectionId = wsId;
                                        break;
                                    case "klippy_connected":
                                        bool klippyConnected =
                                            JsonConvert.DeserializeObject<bool>(jsonBody.ToLower());
                                        break;
                                    case "registered_directories":
                                        RegisteredDirectories =
                                            JsonConvert.DeserializeObject<List<string>>(jsonBody);
                                        break;
                                    case "cpu_temp":
                                        CpuTemp =
                                            JsonConvert.DeserializeObject<double>(jsonBody.Replace(",", "."));
                                        break;
                                    case "moonraker_version":
                                        MoonrakerVersion = jsonBody;
                                        break;
                                    case "klippy_state":
                                        KlipperState = jsonBody;
                                        break;
                                    // Not relevant so far
                                    case "components":
                                    case "failed_components":
                                    //case "registered_directories":
                                    case "warnings":
                                    case "websocket_count":
                                    //case "moonraker_version":
#if DEBUG
                                        Console.WriteLine($"Ignored Json object: '{name}' => '{jsonBody}");
#endif
                                        break;
                                    default:
#if DEBUG
                                        Console.WriteLine($"No Json object found for '{name}' => '{jsonBody}");
#endif
                                        Dictionary<string, string> loggedResults = new(IgnoredJsonResults);
                                        if (!loggedResults.ContainsKey(name))
                                        {
                                            // Log unused json results for further releases
                                            loggedResults.Add(name, jsonBody);
                                            IgnoredJsonResults = loggedResults;
                                        }
                                        break;
                                }
                            }
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
                {
                    return true;
                }
                KlipperActionResult actionResult = JsonConvert.DeserializeObject<KlipperActionResult>(result);
                return actionResult != null ? actionResult.Result.ToLower() == "ok" : false;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result,
                    TargetType = nameof(KlipperActionResult),
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
        
        KlipperApiRequestRespone ValidateRespone(RestResponse respone, Uri targetUri)
        {
            KlipperApiRequestRespone apiRsponeResult = new() { IsOnline = IsOnline };
            try
            {
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
                        Uri = targetUri,
                    };
                }
                else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation
                    || respone.StatusCode == HttpStatusCode.Forbidden
                    || respone.StatusCode == HttpStatusCode.Unauthorized
                    )
                {
                    apiRsponeResult.IsOnline = true;
                    apiRsponeResult.HasAuthenticationError = true;
                    apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    };
                }
                else if (respone.StatusCode == HttpStatusCode.Conflict)
                {
                    apiRsponeResult.IsOnline = true;
                    apiRsponeResult.HasAuthenticationError = false;
                    apiRsponeResult.EventArgs = new KlipperRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    };
                }
                else
                {
                    OnRestApiError(new KlipperRestEventArgs()
                    {
                        Status = respone.ResponseStatus.ToString(),
                        Exception = respone.ErrorException,
                        Message = respone.ErrorMessage,
                        Uri = targetUri,
                    });
                }
            }
            catch(Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }
        #endregion

        #region RestApi
        async Task<KlipperApiRequestRespone> SendRestApiRequestAsync(
            MoonrakerCommandBase commandBase,
            Method method,
            string command,
            object jsonObject = null,
            CancellationTokenSource cts = default,           
            Dictionary<string, string> urlSegments = null,
            string requestTargetUri = ""
            )
        {
            KlipperApiRequestRespone apiRsponeResult = new() { IsOnline = IsOnline };
            if (!IsOnline) return apiRsponeResult;

            try
            {
                if(cts == default)
                {
                    cts = new(DefaultTimeout);
                }
                // https://github.com/Arksine/moonraker/blob/master/docs/web_api.md
                if(restClient == null)
                {
                    UpdateRestClientInstance();
                }
                RestRequest request = new(
                    $"{(string.IsNullOrEmpty(requestTargetUri) ? commandBase.ToString() : requestTargetUri)}/{command}")
                {
                    RequestFormat = DataFormat.Json,
                    Method = method
                };

                bool validHeader = false;
                // Prefer usertoken over api key
                if (!string.IsNullOrEmpty(UserToken))
                {
                    request.AddHeader("Authorization", $"Bearer {UserToken}");
                    validHeader = true;
                }
                else if (!string.IsNullOrEmpty(API))
                {
                    request.AddHeader("X-Api-Key", $"{API}");
                    validHeader = true;
                }

                if (urlSegments != null)
                {
                    foreach (KeyValuePair<string, string> pair in urlSegments)
                    {
                        request.AddParameter(pair.Key, pair.Value, ParameterType.QueryString);
                    }
                }

                if (jsonObject != null)
                {
                    request.AddJsonBody(jsonObject, "application/json");
                }
                // https://moonraker.readthedocs.io/en/latest/web_api/#authorization
                if (!validHeader)
                {
                    // Prefer usertoken over api key
                    if (!string.IsNullOrEmpty(RefreshToken))
                    {
                        request.AddParameter("access_token", RefreshToken, ParameterType.QueryString);
                    }
                    else if (!string.IsNullOrEmpty(API))
                    {
                        request.AddParameter("token", API, ParameterType.QueryString);
                    }
                }

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
                    apiRsponeResult = ValidateRespone(respone, fullUri);     
                }
                catch (TaskCanceledException texp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    }
                }
                catch (HttpRequestException hexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(hexp, false));
                    }
                }
                catch (TimeoutException toexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(toexp, false));
                    }
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<KlipperApiRequestRespone> SendOnlineCheckRestApiRequestAsync(
            MoonrakerCommandBase commandBase,
            string command,
            CancellationTokenSource cts,
            string requestTargetUri = ""
            )
        {
            KlipperApiRequestRespone apiRsponeResult = new() { IsOnline = false };
            try
            {
                if (cts == default)
                {
                    cts = new(DefaultTimeout);
                }
                // https://github.com/Arksine/moonraker/blob/master/docs/web_api.md
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                RestRequest request = new(
                    $"{(string.IsNullOrEmpty(requestTargetUri) ? commandBase.ToString() : requestTargetUri)}/{command}")
                {
                    RequestFormat = DataFormat.Json,
                    Method = Method.Get
                };

                bool validHeader = false;
                // Prefer usertoken over api key
                if (!string.IsNullOrEmpty(UserToken))
                {
                    //request.AddHeader("Authorization", $"Bearer {UserToken}", false);
                    request.AddHeader("Authorization", $"Bearer {UserToken}");
                    validHeader = true;
                }
                else if (!string.IsNullOrEmpty(API))
                {
                    request.AddHeader("X-Api-Key", $"{API}");
                    validHeader = true;
                }

                // https://moonraker.readthedocs.io/en/latest/web_api/#authorization
                if (!validHeader)
                {
                    // Prefer usertoken over api key
                    if (!string.IsNullOrEmpty(RefreshToken))
                    {
                        request.AddParameter("access_token", RefreshToken, ParameterType.QueryString);
                    }
                    else if (!string.IsNullOrEmpty(API))
                    {
                        request.AddParameter("token", API, ParameterType.QueryString);
                    }
                }

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                }
                catch (TaskCanceledException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }
                catch (HttpRequestException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }
                catch (TimeoutException)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                }

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            return apiRsponeResult;
        }

        async Task<KlipperApiRequestRespone> SendMultipartFormDataFileRestApiRequestAsync(
            string filePath,
            string root = "gcodes",
            string path = "",
            int timeout = 100000
            )
        {
            KlipperApiRequestRespone apiRsponeResult = new();
            if (!IsOnline) return apiRsponeResult;

            try
            {
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                RestRequest request = new("/server/files/upload");
                
                bool validHeader = false;
                // Prefer usertoken over api key
                if (!string.IsNullOrEmpty(UserToken))
                {
                    request.AddHeader("Authorization", $"Bearer {UserToken}");
                    validHeader = true;
                }
                else if (!string.IsNullOrEmpty(API))
                {
                    request.AddHeader("X-Api-Key", $"{API}");
                    validHeader = true;
                }
                // https://moonraker.readthedocs.io/en/latest/web_api/#authorization
                if (!validHeader)
                {
                    // Prefer usertoken over api key
                    if (!string.IsNullOrEmpty(RefreshToken))
                    {
                        request.AddParameter("access_token", RefreshToken, ParameterType.QueryString);
                    }
                    else if (!string.IsNullOrEmpty(API))
                    {
                        request.AddParameter("token", API, ParameterType.QueryString);
                    }
                }

                request.RequestFormat = DataFormat.Json;
                request.Method = Method.Post;
                request.AlwaysMultipartFormData = true;

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFile("file", filePath, "application/octet-stream");
                request.AddParameter("root", root, ParameterType.GetOrPost);
                request.AddParameter("path", path, ParameterType.GetOrPost);
                /*
                request.AddParameter("root", root, "multipart/form-data", ParameterType.GetOrPost);
                request.AddParameter("path", path, "multipart/form-data", ParameterType.GetOrPost);
                */
                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                    /*
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
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation
                        || respone.StatusCode == HttpStatusCode.Forbidden
                        || respone.StatusCode == HttpStatusCode.Unauthorized
                        )
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
                    */
                }
                catch (TaskCanceledException texp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    }
                }
                catch (HttpRequestException hexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(hexp, false));
                    }
                }
                catch (TimeoutException toexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(toexp, false));
                    }
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
            int timeout = 100000
            )
        {
            KlipperApiRequestRespone apiRsponeResult = new();
            if (!IsOnline) return apiRsponeResult;

            try
            {
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                RestRequest request = new("/server/files/upload");
                
                bool validHeader = false;
                // Prefer usertoken over api key
                if (!string.IsNullOrEmpty(UserToken))
                {
                    request.AddHeader("Authorization", $"Bearer {UserToken}");
                    validHeader = true;
                }
                else if (!string.IsNullOrEmpty(API))
                {
                    request.AddHeader("X-Api-Key", $"{API}");
                    validHeader = true;
                }
                // https://moonraker.readthedocs.io/en/latest/web_api/#authorization
                if (!validHeader)
                {
                    // Prefer usertoken over api key
                    if (!string.IsNullOrEmpty(RefreshToken))
                    {
                        request.AddParameter("access_token", RefreshToken, ParameterType.QueryString);
                    }
                    else if (!string.IsNullOrEmpty(API))
                    {
                        request.AddParameter("token", API, ParameterType.QueryString);
                    }
                }

                request.RequestFormat = DataFormat.Json;
                request.Method = Method.Post;
                request.AlwaysMultipartFormData = true;

                //Multiform
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFile("file", file, fileName, "application/octet-stream");
                request.AddParameter("root", root, ParameterType.GetOrPost);
                request.AddParameter("path", path, ParameterType.GetOrPost);
                /*
                request.AddParameter("root", root, "multipart/form-data", ParameterType.GetOrPost);
                request.AddParameter("path", path, "multipart/form-data", ParameterType.GetOrPost);
                */

                Uri fullUri = restClient.BuildUri(request);
                try
                {
                    RestResponse respone = await restClient.ExecuteAsync(request, cts.Token);
                    apiRsponeResult = ValidateRespone(respone, fullUri);
                    /*
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
                    else if (respone.StatusCode == HttpStatusCode.NonAuthoritativeInformation
                        || respone.StatusCode == HttpStatusCode.Forbidden
                        || respone.StatusCode == HttpStatusCode.Unauthorized
                        )
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
                    */
                }
                catch (TaskCanceledException texp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(texp, false));
                    }
                }
                catch (HttpRequestException hexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(hexp, false));
                    }
                }
                catch (TimeoutException toexp)
                {
                    // Throws exception on timeout, not actually an error but indicates if the server is not reachable.
                    if (!IsOnline)
                    {
                        OnError(new UnhandledExceptionEventArgs(toexp, false));
                    }
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
        public async Task<byte[]> DownloadFileFromUriAsync(string path, int timeout = 10000)
        {
            try
            {
                if (restClient == null)
                {
                    UpdateRestClientInstance();
                }
                RestRequest request = new(path);
                
                bool validHeader = false;
                // Prefer usertoken over api key
                if (!string.IsNullOrEmpty(UserToken))
                {
                    request.AddHeader("Authorization", $"Bearer {UserToken}");
                    validHeader = true;
                }
                else if (!string.IsNullOrEmpty(API))
                {
                    request.AddHeader("X-Api-Key", $"{API}");
                    validHeader = true;
                }
                // https://moonraker.readthedocs.io/en/latest/web_api/#authorization
                if (!validHeader)
                {
                    // Prefer usertoken over api key
                    if (!string.IsNullOrEmpty(RefreshToken))
                    {
                        request.AddParameter("access_token", RefreshToken, ParameterType.QueryString);
                    }
                    else if (!string.IsNullOrEmpty(API))
                    {
                        request.AddParameter("token", API, ParameterType.QueryString);
                    }
                }

                request.RequestFormat = DataFormat.Json;
                request.Method = Method.Get;
                request.Timeout = timeout;

                Uri fullUrl = restClient.BuildUri(request);
                CancellationTokenSource cts = new(timeout);
                byte[] respone = await restClient.DownloadDataAsync(request, cts.Token)
                    .ConfigureAwait(false)
                    ;

                return respone;
                /*
                // Workaround, because the RestClient returns bad requests
                using WebClient client = new();
                byte[] bytes = await client.DownloadDataTaskAsync(fullUrl);
                return bytes;
                */
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
            WebProxy proxy = new()
            {
                Address = GetProxyUri(),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = ProxyUseDefaultCredentials,
            };
            if (ProxyUseDefaultCredentials && !string.IsNullOrEmpty(ProxyUser))
            {
                proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
            }
            else
            {
                proxy.UseDefaultCredentials = ProxyUseDefaultCredentials;
            }

            return proxy;
        }

        void UpdateRestClientInstance()
        {
            if (string.IsNullOrEmpty(ServerAddress))
            {
                return;
            }
            if (EnableProxy && !string.IsNullOrEmpty(ProxyAddress))
            {
                RestClientOptions options = new(FullWebAddress)
                {
                    ThrowOnAnyError = true,
                    MaxTimeout = 10000,
                };
                HttpClientHandler httpHandler = new()
                {
                    UseProxy = true,
                    Proxy = GetCurrentProxy(),
                    AllowAutoRedirect = true,
                };
                
                httpClient = new(handler: httpHandler, disposeHandler: true);
                restClient = new(httpClient: httpClient, options: options);
            }
            else
            {
                httpClient = null;
                restClient = new(baseUrl: FullWebAddress);
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
                KlipperState = newPrinterInfo.State;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void UpdateCurrentPrintDependencies(bool newIsPrintingState)
        {
            try
            {
                if(newIsPrintingState)
                {
                    // Refresh current job and gcode meta
                    Task.Run(async () =>
                    {
                        //ActiveJob = await GetActiveJobStatusAsync();
                        await RefreshGcodeMetadataAsync(ActiveJobName);
                    });
                }
                else
                {
                    ActiveJobName = null;
                    GcodeMeta = null;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void UpdateActivePrintGcodeMeta(KlipperStatusJob job)
        {
            try
            {
                if(job == null || string.IsNullOrEmpty(job.Filename))
                {
                    GcodeMeta = null;
                }
                else
                {
                    Task.Run(async () =>
                    {
                        await RefreshGcodeMetadataAsync(job.Filename).ConfigureAwait(false);
                    });
                }
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
            //UpdateWebClientInstance();
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
            //UpdateWebClientInstance();
        }
        #endregion

        #region Refresh
        [Obsolete("Use StartListeningAsync instead")]
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
                    if(RefreshCounter%2 == 0)
                    {
                        await RefreshServerCachedTemperatureDataAsync().ConfigureAwait(false);
                    }
                    if (RefreshHeatersDirectly)
                    {
                        List<Task> tasks = new()
                        {
                            RefreshExtruderStatusAsync(),
                            RefreshHeaterBedStatusAsync(),
                        };
                        await Task.WhenAll(tasks).ConfigureAwait(false);
                    }
                }
                else if (IsListening)
                {
                    StopListening();
                }
            }, null, 0, RefreshInterval * 1000);
            IsListening = true;
        }
        [Obsolete("Use StopListeningAsync instead")]
        public void StopListening()
        {
            CancelCurrentRequests();
            StopPingTimer();
            StopTimer();

            if (IsListeningToWebsocket)
                DisconnectWebSocket();
            IsListening = false;
        }

        public async Task StartListeningAsync(bool StopActiveListening = false)
        {
            if (IsListening)// avoid multiple sessions
            {
                if (StopActiveListening)
                {
                    await StopListeningAsync();
                }
                else
                {
                    return; // StopListening();
                }
            }
            await ConnectWebSocketAsync().ConfigureAwait(false);
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
                    if(RefreshCounter%2 == 0)
                    {
                        await RefreshServerCachedTemperatureDataAsync().ConfigureAwait(false);
                    }
                    if (RefreshHeatersDirectly)
                    {
                        List<Task> tasks = new()
                        {
                            RefreshExtruderStatusAsync(),
                            RefreshHeaterBedStatusAsync(),
                        };
                        await Task.WhenAll(tasks).ConfigureAwait(false);
                    }
                }
                else if (IsListening)
                {
                    StopListening();
                }
            }, null, 0, RefreshInterval * 1000);
            IsListening = true;
        }
        public async Task StopListeningAsync()
        {
            CancelCurrentRequests();
            StopPingTimer();
            StopTimer();

            if (IsListeningToWebsocket)
            {
                await DisconnectWebSocketAsync().ConfigureAwait(false);
            }
            IsListening = false;
        }
        public async Task RefreshAllAsync()
        {
            try
            {
                // Avoid multiple calls
                if (IsRefreshing) return;
                IsRefreshing = true;
                // Detects current operating system, must be called before each other Database method
                await RefreshDatabaseNamespacesAsync();
                //await RefreshPrinterListAsync();
                List<Task> task = new()
                {
                    //Task.Delay(10),
                    RefreshServerConfigAsync(),
                    RefreshPrinterInfoAsync(),
                    RefreshDashboardPresetsAsync(),
                    RefreshAvailableFilesAsync(),
                    RefreshJobQueueStatusAsync(),
                    RefreshDirectoryInformationAsync(),
                    RefreshAvailableDirectorienAsync(),
                    RefreshServerCachedTemperatureDataAsync(),

                    RefreshToolHeadStatusAsync(),
                    RefreshVirtualSdCardStatusAsync(),
                    RefreshPrintStatusAsync(),
                    RefreshGcodeMoveStatusAsync(),
                    RefreshExtruderStatusAsync(),
                    RefreshHeaterBedStatusAsync(),
                    RefreshIdleStatusAsync(),
                    RefreshFanStatusAsync(),
                    RefreshDisplayStatusAsync(),
                    RefreshMotionReportAsync(),

                    RefreshGeneralSettingsAsync(),
                    RefreshWebCamConfigAsync(),
                    RefreshRemotePrintersAsync(),
                };
                await Task.WhenAll(task).ConfigureAwait(false);
                if (!InitialDataFetched)
                {
                    InitialDataFetched = true;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsRefreshing = false;
        }
        #endregion

        #region Login
        [Obsolete("Not needed anymore, will be removed")]
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
                /* */
                if (httpClient != null)
                {
                    httpClient.CancelPendingRequests();
                    UpdateRestClientInstance();
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        #endregion

        #region CheckOnline

        public async Task CheckOnlineAsync(int Timeout = 10000)
        {
            CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, Timeout));
            await CheckOnlineAsync(cts).ConfigureAwait(false);
            cts?.Dispose();
        }
        public async Task CheckOnlineAsync(CancellationTokenSource cts)
        {
            if (IsConnecting) return; // Avoid multiple calls
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                string uriString = FullWebAddress;
                try
                {
                    // Send a blank api request in order to check if the server is reachable
                    KlipperApiRequestRespone respone = 
                        await SendOnlineCheckRestApiRequestAsync(MoonrakerCommandBase.api, "version", cts)
                        .ConfigureAwait(false);
                    isReachable = respone?.IsOnline == true;
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
            if (!IsOnline || isReachable || _retries > RetriesWhenOffline)
            {
                // Do not check if the previous state was already offline
                _retries = 0;
                IsOnline = isReachable;
            }
            else
            {
                // Retry with shorter timeout to see if the connection loss is real
                _retries++;
                await CheckOnlineAsync(3500).ConfigureAwait(false);
            }
        }
        public async Task<bool> CheckIfApiIsValidAsync(int Timeout = 10000)
        {
            try
            {
                if (IsOnline)
                {
                    CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, Timeout));
                    KlipperApiRequestRespone respone = await SendOnlineCheckRestApiRequestAsync(MoonrakerCommandBase.api, "version", cts).ConfigureAwait(false);
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
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task CheckServerIfApiIsValidAsync(int Timeout = 10000)
        {
            _ = await CheckIfApiIsValidAsync(Timeout).ConfigureAwait(false);
        }
        #endregion

        #region WebCam
        public string GetDefaultWebCamUri()
        {
            try
            {
                string token = !string.IsNullOrEmpty(API) ? API : UserToken;
                return $"{FullWebAddress}/webcam/?action=stream{(!string.IsNullOrEmpty(token) ? $"?t={token}" : "")}";
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return "";
            }
        }
        public async Task<string> GetWebCamUriAsync(int index = 0, bool refreshWebCamConfig = false)
        {
            try
            {
                if (WebCamConfigs?.Count <= 0)
                {
                    await RefreshWebCamConfigAsync().ConfigureAwait(false);
                }
                KlipperDatabaseWebcamConfig config = null;
                if (WebCamConfigs?.Count > index)
                {
                    config = WebCamConfigs[index];
                }
                else if (WebCamConfigs.Count > 0)
                {
                    config = WebCamConfigs.FirstOrDefault();
                }
                // If nothing is found, try the default setup
                else
                {
                    config = new KlipperDatabaseWebcamConfig()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Default",
                        Enabled = true,
                        FlipX = false,
                        FlipY = false,
                        TargetFps = 15,
                        Url = "/webcam?action=stream"
                    };
                }

                string token = !string.IsNullOrEmpty(API) ? API : UserToken;
                return config == null ? GetDefaultWebCamUri() : $"{FullWebAddress}{config.Url}{(!string.IsNullOrEmpty(token) ? $"?t={token}" : "")}";
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
        public bool CheckIfConfigurationHasChanged(object temp)
        {
            try
            {
                if (temp is not MoonrakerClient tempKlipper) return false;
                else return
                    !(ServerAddress == tempKlipper.ServerAddress &&
                        Port == tempKlipper.Port &&
                        API == tempKlipper.API &&
                        IsSecure == tempKlipper.IsSecure &&
                        LoginRequired == tempKlipper.LoginRequired
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Get, "oneshot_token").ConfigureAwait(false);
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
                    TargetType = nameof(KlipperAccessTokenResult),
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Get, "api_key").ConfigureAwait(false);
                KlipperAccessTokenResult accessToken = JsonConvert.DeserializeObject<KlipperAccessTokenResult>(result.Result);
                //API = accessToken?.Result;
                return accessToken;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperAccessTokenResult),
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

        public async Task RefreshApiKeyAsync()
        {
            try
            {
                KlipperAccessTokenResult result = await GetApiKeyAsync().ConfigureAwait(false);
                API = result?.Result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "info")
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
                    TargetType = nameof(KlipperPrinterStateMessageRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "emergency_stop")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "restart")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "firmware_restart")
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
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
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
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetSpeedFactorAsync(int target)
        {
            try
            {
                string cmd = $"M220 S{target}";
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SetExtrusionFactorAsync(int target)
        {
            try
            {
                string cmd = $"M221 S{target}";
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
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
                    switch (target)
                    {
                        case > 255:
                            setSpeed = 255;
                            break;
                        case < 0:
                            setSpeed = 0;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    setSpeed = Convert.ToInt32(target * 255f / 100f);
                }

                string cmd = $"M106 S{setSpeed}";
                bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
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
        public async Task<List<string>> GetPrinterObjectListAsync(string startsWith = "", bool removeStartTag = false)
        {
            KlipperApiRequestRespone result = new();
            List<string> resultObject = new();
            try
            {
                //object cmd = new { name = ScriptName };
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "objects/list")
                    .ConfigureAwait(false);

                KlipperActionListRespone state = JsonConvert.DeserializeObject<KlipperActionListRespone>(result.Result);
                if(!string.IsNullOrEmpty(startsWith))
                {
                    resultObject = state?.Result?.Objects.Where(obj => obj.StartsWith(startsWith)).ToList();
                    if (removeStartTag)
                    {
                        resultObject = resultObject.Select(item => item.Replace(startsWith, string.Empty).Trim()).ToList();
                    }
                    return resultObject;
                }
                return state?.Result?.Objects;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperActionListRespone),
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

        public async Task<Dictionary<string, object>> QueryPrinterObjectStatusAsync(Dictionary<string, string> objects)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, object> resultObject = new();
            try
            {
                Dictionary<string, string> urlSegments = new();
                foreach (KeyValuePair<string, string> obj in objects)
                {
                    // Do not query macros here, there is an extra method for this.
                    if (obj.Key.StartsWith("gcode_macro")) continue;
                    urlSegments.Add(obj.Key, obj.Value);
                }
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "objects/query", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);

                KlipperPrinterStatusRespone queryResult = JsonConvert.DeserializeObject<KlipperPrinterStatusRespone>(result.Result);
                if (queryResult?.Result?.Status is JObject jsonObject)
                {
                    foreach (JProperty property in jsonObject.Children<JProperty>())
                    {
                        Stack<JToken> avilableProperties = new(jsonObject.Children<JToken>());
                        do
                        {
                            JToken token = avilableProperties.Pop();
                            if(token is JProperty propTest)
                            {
                                // Get the childs for this tags
                                if(propTest.Name.StartsWith("configfile") || propTest.Name.StartsWith("settings"))
                                {
                                    // Add all child properties back to the stack
                                    List<JToken> children = token.Children().ToList();
                                    foreach (JToken child in children)
                                    {
                                        avilableProperties.Push(child);
                                    }
                                    continue;
                                }
                            }
                            else if (token is JToken childToken)
                            {
                                if (childToken?.First is not JProperty jp)
                                    continue;
                                /**/
                                // Get the childs for this tags
                                if (jp.Name.StartsWith("configfile") || jp.Name.StartsWith("settings"))
                                {
                                    // Add all child properties back to the stack
                                    List<JToken> children = token.Children().ToList();
                                    foreach (JToken child in children)
                                    {
                                        avilableProperties.Push(child);
                                    }
                                    continue;
                                }
                                
                            }

                            if (token is not JProperty parent)
                            {
                                // Add all child properties back to the stack
                                List<JToken> chilTokens = token.Children().ToList();
                                foreach (JToken child in chilTokens)
                                {
                                    avilableProperties.Push(child);
                                }
                                continue;
                            }

                            string name = parent.Name;
                            string path = parent.Path;
                            string jsonBody = parent.Value.ToString();
                            switch (name)
                            {
                                case "probe":
                                    KlipperStatusProbe probe =
                                        JsonConvert.DeserializeObject<KlipperStatusProbe>(jsonBody);
                                    resultObject.Add(name, probe);
                                    break;
                                case "configfile":
                                    KlipperStatusConfigfile configFile =
                                        JsonConvert.DeserializeObject<KlipperStatusConfigfile>(jsonBody);
                                    resultObject.Add(name, configFile);
                                    break;
                                case "query_endstops":
                                    KlipperStatusQueryEndstops endstops =
                                        JsonConvert.DeserializeObject<KlipperStatusQueryEndstops>(jsonBody);
                                    resultObject.Add(name, endstops);
                                    break;
                                case "virtual_sdcard":
                                    KlipperStatusVirtualSdcard virtualSdcardState =
                                        JsonConvert.DeserializeObject<KlipperStatusVirtualSdcard>(jsonBody);
                                    resultObject.Add(name, virtualSdcardState);
                                    break;
                                case "display_status":
                                    KlipperStatusDisplay displayState =
                                        JsonConvert.DeserializeObject<KlipperStatusDisplay>(jsonBody);
                                    resultObject.Add(name, displayState);
                                    break;
                                case "moonraker_stats":
                                    MoonrakerStatInfo notifyProcState =
                                        JsonConvert.DeserializeObject<MoonrakerStatInfo>(jsonBody);
                                    resultObject.Add(name, notifyProcState);
                                    break;
                                case "mcu":
                                    KlipperStatusMcu mcuState =
                                        JsonConvert.DeserializeObject<KlipperStatusMcu>(jsonBody);
                                    resultObject.Add(name, mcuState);
                                    break;
                                case "system_stats":
                                    KlipperStatusSystemStats systemState =
                                        JsonConvert.DeserializeObject<KlipperStatusSystemStats>(jsonBody);
                                    resultObject.Add(name, systemState);
                                    break;
                                case "cpu_temp":
                                    double cpuTemp =
                                        JsonConvert.DeserializeObject<double>(jsonBody.Replace(",", "."));
                                    resultObject.Add(name, cpuTemp);
                                    break;
                                case "websocket_connections":
                                    int wsConnections =
                                        JsonConvert.DeserializeObject<int>(jsonBody);
                                    resultObject.Add(name, wsConnections);
                                    break;
                                case "network":
                                    Dictionary<string, KlipperNetworkInterface> network =
                                        JsonConvert.DeserializeObject<Dictionary<string, KlipperNetworkInterface>>(jsonBody);
                                    resultObject.Add(name, network);
                                    break;
                                case "gcode_move":
                                    KlipperStatusGcodeMove gcodeMoveState =
                                        JsonConvert.DeserializeObject<KlipperStatusGcodeMove>(jsonBody);
                                    resultObject.Add(name, gcodeMoveState);
                                    break;
                                case "print_stats":
                                    KlipperStatusPrintStats printStats =
                                        JsonConvert.DeserializeObject<KlipperStatusPrintStats>(jsonBody);
                                    printStats.ValidPrintState = jsonBody.Contains("state");
                                    resultObject.Add(name, printStats);
                                    break;
                                case "fan":
                                    KlipperStatusFan fanState =
                                        JsonConvert.DeserializeObject<KlipperStatusFan>(jsonBody);
                                    resultObject.Add(name, fanState);
                                    break;
                                case "toolhead":
                                    KlipperStatusToolhead toolhead =
                                        JsonConvert.DeserializeObject<KlipperStatusToolhead>(jsonBody);
                                    resultObject.Add(name, toolhead);
                                    break;
                                case "heater_bed":
                                    // In the status report the temp is missing, so do not parse the heater then.
                                    //if (!jsonBody.Contains("temperature")) break;
                                    if (path.EndsWith("settings.heater_bed"))
                                    {
                                        KlipperConfigHeaterBed settingsHeaterBed =
                                            JsonConvert.DeserializeObject<KlipperConfigHeaterBed>(jsonBody);
                                        resultObject.Add(name, settingsHeaterBed);
                                    }
                                    else
                                    {
                                        KlipperStatusHeaterBed heaterBed =
                                            JsonConvert.DeserializeObject<KlipperStatusHeaterBed>(jsonBody);
                                        resultObject.Add(name, heaterBed);
                                    }
                                    break;
                                case "extruder":
                                case "extruder1":
                                case "extruder2":
                                case "extruder3":
                                    // In the status report the temp is missing, so do not parse the heater then.
                                    //if (!jsonBody.Contains("temperature")) break;
                                    if (path.EndsWith("settings.extruder"))
                                    {
                                        KlipperConfigExtruder settingsExtruder =
                                            JsonConvert.DeserializeObject<KlipperConfigExtruder>(jsonBody);
                                        resultObject.Add(name, settingsExtruder);
                                    }
                                    else
                                    {
                                        KlipperStatusExtruder extruder =
                                            JsonConvert.DeserializeObject<KlipperStatusExtruder>(jsonBody);
                                        resultObject.Add(name, extruder);
                                    }
                                    break;
                                case "motion_report":
                                    KlipperStatusMotionReport motionReport =
                                        JsonConvert.DeserializeObject<KlipperStatusMotionReport>(jsonBody);
                                    resultObject.Add(name, motionReport);
                                    break;
                                case "idle_timeout":
                                    KlipperStatusIdleTimeout idleTimeout =
                                        JsonConvert.DeserializeObject<KlipperStatusIdleTimeout>(jsonBody);
                                    idleTimeout.ValidState = jsonBody.Contains("state");
                                    resultObject.Add(name, idleTimeout);
                                    break;
                                case "filament_switch_sensor fsensor":
                                    KlipperStatusFilamentSensor fSensor =
                                        JsonConvert.DeserializeObject<KlipperStatusFilamentSensor>(jsonBody);
                                    resultObject.Add(name, fSensor);
                                    break;
                                case "pause_resume":
                                    KlipperStatusPauseResume pauseResume =
                                        JsonConvert.DeserializeObject<KlipperStatusPauseResume>(jsonBody);
                                    resultObject.Add(name, pauseResume);
                                    break;
                                case "action":
                                    string action = jsonBody;
                                    resultObject.Add(name, action);
                                    break;
                                case "bed_mesh":
                                    KlipperStatusMesh mesh =
                                        JsonConvert.DeserializeObject<KlipperStatusMesh>(jsonBody);
                                    resultObject.Add(name, mesh);
                                    break;
                                case "job":
                                    KlipperStatusJob job =
                                        JsonConvert.DeserializeObject<KlipperStatusJob>(jsonBody);
                                    resultObject.Add(name, job);
                                    break;
                                default:
#if DEBUG
                                    Console.WriteLine($"No Json object found for '{name}' => '{jsonBody}");
#endif
                                    if (name.StartsWith("gcode_macro"))
                                    {
                                        KlipperGcodeMacro gcMacro =
                                            JsonConvert.DeserializeObject<KlipperGcodeMacro>(jsonBody);
                                        if (string.IsNullOrEmpty(gcMacro?.Name))
                                        {
                                            gcMacro.Name = name.Replace("gcode_macro", string.Empty).Trim();
                                        }
                                        resultObject.Add(name, gcMacro);
                                    }
                                    else
                                    {
                                        // If no parser found, pass the json object instead
                                        resultObject.Add(name, parent.Value);
                                    }
                                    Dictionary<string, string> loggedResults = new(IgnoredJsonResults);
                                    if (!loggedResults.ContainsKey(name))
                                    {
                                        // Log unused json results for further releases
                                        loggedResults.Add(name, jsonBody);
                                        IgnoredJsonResults = loggedResults;
                                    }
                                    break;
                            }

                        }
                        while (avilableProperties.Count > 0);
                    }
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

        public async Task<Dictionary<string, KlipperGcodeMacro>> GetGcodeMacrosAsync()
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, KlipperGcodeMacro> resultObject = null;
            try
            {
                Dictionary<string, string> objects = new();
                objects.Add("configfile", "settings");

                Dictionary<string, object> settings = await QueryPrinterObjectStatusAsync(objects).ConfigureAwait(false);
#if NETSTANDARD
                IEnumerable<KeyValuePair<string, KlipperGcodeMacro>> macros =
                    settings.Where(keypair => keypair.Key.StartsWith("gcode_macro"))
                    .Select(pair => new KeyValuePair<string, KlipperGcodeMacro>(pair.Key, pair.Value as KlipperGcodeMacro));
                return new(macros);
#else
                List<KeyValuePair<string, KlipperGcodeMacro>> macros =
                    settings.Where(keypair => keypair.Key.StartsWith("gcode_macro"))
                    .Select(pair => new KeyValuePair<string, KlipperGcodeMacro>(pair.Key, pair.Value as KlipperGcodeMacro))
                .ToList()
                ;

                resultObject = new();
                for (int i = 0; i < macros?.Count; i++)
                {
                    resultObject.Add(macros[i].Key, macros[i].Value);
                }
                return resultObject;
#endif
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
        public async Task<Dictionary<string, KlipperStatusFilamentSensor>> GetFilamentSensorsAsync(Dictionary<string, string> macros = null)
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, KlipperStatusFilamentSensor> resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                if (macros != null)
                {
                    foreach (KeyValuePair<string, string> obj in macros)
                    {
                        urlSegments.Add(obj.Key, obj.Value);
                    }
                }
                else
                {
                    urlSegments.Add("filament_switch_sensor", string.Empty);
                }
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "objects/query", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);

                KlipperFilamentSensorsRespone queryResult = JsonConvert.DeserializeObject<KlipperFilamentSensorsRespone>(result.Result);
                return queryResult?.Result?.Status;

            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperFilamentSensorsRespone),
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
                // Doc: https://moonraker.readthedocs.io/en/latest/printer_objects/#print_stats
                string key = "print_stats";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, "");

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                if (result.ContainsKey(key) && result?[key] is KlipperStatusPrintStats stateObj)
                {
                    stateObj.ValidPrintState = true;
                    //IsPrinting = stateObj.State == KlipperPrintStates.Printing;
                    resultObject = stateObj;
                }
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshPrintStatusAsync()
        {
            try
            {
                KlipperStatusPrintStats result = await GetPrintStatusAsync().ConfigureAwait(false);
                PrintStats = result;
                if (PrintStats != null)
                {
                    await RefreshGcodeMetadataAsync(PrintStats.Filename).ConfigureAwait(false);
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                PrintStats = null;
                GcodeMeta = null;
            }
        }

        public async Task<KlipperStatusExtruder> GetExtruderStatusAsync(int index = 0)
        {
            KlipperStatusExtruder resultObject = null;
            try
            {
                string key = $"extruder{(index > 0 ? index : "")}";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, "");
                //queryObjects.Add(key, "temperature,target,power,pressure_advance,smooth_time");

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                if (result.ContainsKey(key) && result?[key] is KlipperStatusExtruder stateObj)
                    resultObject = stateObj;
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshExtruderStatusAsync(int index = 0)
        {
            try
            {
                KlipperStatusExtruder result = await GetExtruderStatusAsync(index).ConfigureAwait(false);
                if(result != null)
                {
                    Dictionary<int, KlipperStatusExtruder> states = new();
                    states.Add(index, result);
                    Extruders = states;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Extruders = null;
            }
        }

        public async Task<KlipperStatusFan> GetFanStatusAsync()
        {
            KlipperStatusFan resultObject = null;
            try
            {
                string key = "fan";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, "");

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);
                if (result.ContainsKey(key) && result?[key] is KlipperStatusFan stateObj)
                    resultObject = stateObj;
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshFanStatusAsync()
        {
            try
            {
                KlipperStatusFan result = await GetFanStatusAsync().ConfigureAwait(false);
                Fan = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Fan = null;
            }
        }

        public async Task<KlipperStatusIdleTimeout> GetIdleStatusAsync()
        {
            KlipperStatusIdleTimeout resultObject = null;
            try
            {
                string key = "idle_timeout";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, string.Empty);

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusIdleTimeout stateObj)
                {
                    stateObj.ValidState = true;
                    resultObject = stateObj;
                }
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshIdleStatusAsync()
        {
            try
            {
                KlipperStatusIdleTimeout result = await GetIdleStatusAsync().ConfigureAwait(false);
                IdleState = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                IdleState = null;
            }
        }

        public async Task<KlipperStatusDisplay> GetDisplayStatusAsync()
        {
            KlipperStatusDisplay resultObject = null;
            try
            {
                string key = "display_status";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, string.Empty);

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusDisplay stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshDisplayStatusAsync()
        {
            try
            {
                KlipperStatusDisplay result = await GetDisplayStatusAsync().ConfigureAwait(false);
                DisplayStatus = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                DisplayStatus = null;
            }
        }

        public async Task<KlipperStatusToolhead> GetToolHeadStatusAsync()
        {
            KlipperStatusToolhead resultObject = null;
            try
            {
                string key = "toolhead";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, string.Empty);

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusToolhead stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshToolHeadStatusAsync()
        {
            try
            {
                KlipperStatusToolhead result = await GetToolHeadStatusAsync().ConfigureAwait(false);
                ToolHead = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                ToolHead = null;
            }
        }

        public async Task<KlipperStatusGcodeMove> GetGcodeMoveStatusAsync()
        {
            KlipperStatusGcodeMove resultObject = null;
            try
            {
                string key = "gcode_move";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, string.Empty);

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusGcodeMove stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshGcodeMoveStatusAsync()
        {
            try
            {
                KlipperStatusGcodeMove result = await GetGcodeMoveStatusAsync().ConfigureAwait(false);
                GcodeMove = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                GcodeMove = null;
            }
        }

        public async Task<KlipperStatusMotionReport> GetMotionReportAsync()
        {
            KlipperStatusMotionReport resultObject = null;
            try
            {
                string key = "motion_report";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, string.Empty);

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusMotionReport stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshMotionReportAsync()
        {
            try
            {
                KlipperStatusMotionReport result = await GetMotionReportAsync().ConfigureAwait(false);
                MotionReport = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                MotionReport = null;
            }
        }

        public async Task<KlipperStatusVirtualSdcard> GetVirtualSdCardStatusAsync()
        {
            KlipperStatusVirtualSdcard resultObject = null;
            try
            {
                string key = "virtual_sdcard";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, string.Empty);

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusVirtualSdcard stateObj)
                    resultObject = stateObj;
                return resultObject;

            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshVirtualSdCardStatusAsync()
        {
            try
            {
                KlipperStatusVirtualSdcard result = await GetVirtualSdCardStatusAsync().ConfigureAwait(false);
                VirtualSdCard = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                VirtualSdCard = null;
            }
        }

        public async Task<KlipperStatusHeaterBed> GetHeaterBedStatusAsync()
        {
            KlipperStatusHeaterBed resultObject = null;
            try
            {
                string key = "heater_bed";
                Dictionary<string, string> queryObjects = new();
                queryObjects.Add(key, string.Empty);

                Dictionary<string, object> result = await QueryPrinterObjectStatusAsync(queryObjects)
                    .ConfigureAwait(false);

                if (result.ContainsKey(key) && result?[key] is KlipperStatusHeaterBed stateObj)
                {
                    resultObject = stateObj;
                }
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshHeaterBedStatusAsync()
        {
            try
            {
                KlipperStatusHeaterBed result = await GetHeaterBedStatusAsync().ConfigureAwait(false);
                HeaterBed = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                HeaterBed = null;
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

                for (int i = 0; i < objects.Count; i++)
                {
                    string key = objects[i];
                    string value = string.Empty;
                    urlSegments.Add(key, value);
                }

                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "objects/subscribe", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                return result?.Result;
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "query_endstops/status").ConfigureAwait(false);
                KlipperEndstopQueryRespone queryResult = JsonConvert.DeserializeObject<KlipperEndstopQueryRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperEndstopQueryRespone),
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

        #region Server Config
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "config").ConfigureAwait(false);
                KlipperServerConfigRespone config = JsonConvert.DeserializeObject<KlipperServerConfigRespone>(result.Result);
                return config?.Result?.Config;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperServerConfigRespone),
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

        public async Task RefreshServerCachedTemperatureDataAsync()
        {
            try
            {
                Dictionary<string, KlipperTemperatureSensorHistory> result = await GetServerCachedTemperatureDataAsync().ConfigureAwait(false);
                TemperatureCache = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                TemperatureCache = new();
            }
        }
        public async Task<Dictionary<string, KlipperTemperatureSensorHistory>> GetServerCachedTemperatureDataAsync()
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, KlipperTemperatureSensorHistory> resultObject = new();
            try
            {
                //object cmd = new { name = ScriptName };
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "temperature_store").ConfigureAwait(false);
                KlipperServerTempDataRespone tempData = JsonConvert.DeserializeObject<KlipperServerTempDataRespone>(result.Result);
                return tempData?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperServerTempDataRespone),
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"gcode_store?count={count}").ConfigureAwait(false);
                KlipperGcodesRespone tempData = JsonConvert.DeserializeObject<KlipperGcodesRespone>(result.Result);
                return tempData?.Result?.Gcodes;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperGcodesRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, "restart")
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
                result = await SendRestApiRequestAsync(MoonRakerCommandBase.server, Method.Get, "websocket_id").ConfigureAwait(false);
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "gcode/script", default, null, urlSegements)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> RunGcodeMacroAsync(KlipperGcodeMacro marco)
        {
            try
            {
                if (string.IsNullOrEmpty(marco?.Name))
                {
                    return false;
                }
                string cmd = $"{marco.Name}";
                bool result = await SendOctoPrintApiGcodeCommandAsync(cmd).ConfigureAwait(false);
                return result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> RunPresetAsync(KlipperDatabaseTemperaturePreset presetProfile)
        {
            try
            {
                if (presetProfile == null)
                {
                    return false;
                }

                List<string> cmds = new();
                if (!string.IsNullOrEmpty(presetProfile?.Gcode))
                {
                    cmds.Add(presetProfile.Gcode);
                }
                if (presetProfile?.Values?.ContainsKey("extruder") != null && presetProfile?.Values?["extruder"]?.Active == true)
                {
                    cmds.Add($"SET_HEATER_TEMPERATURE HEATER=extruder TARGET={presetProfile.Values["extruder"].Value}");
                }
                if (presetProfile?.Values?.ContainsKey("extruder1") != null && presetProfile?.Values?["extruder1"]?.Active == true)
                {
                    cmds.Add($"SET_HEATER_TEMPERATURE HEATER=extruder1 TARGET={presetProfile.Values["extruder1"].Value}");
                }
                if (presetProfile?.Values?.ContainsKey("heater_bed") != null && presetProfile?.Values?["heater_bed"]?.Active == true)
                {
                    cmds.Add($"SET_HEATER_TEMPERATURE HEATER=heater_bed TARGET={presetProfile.Values["heater_bed"].Value}");
                }
                //bool result = await SendGcodeCommandAsync(cmds.ToArray()).ConfigureAwait(false);
                //return result;
                List<bool> results = new();
                for (int i = 0; i < cmds.Count; i++)
                {
                    string cmd = cmds[i];
                    bool result = await RunGcodeScriptAsync(cmd).ConfigureAwait(false);
                    results.Add(result);
                }
                return results.All(res => res);
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "gcode/help").ConfigureAwait(false);
                KlipperGcodeHelpRespone config = JsonConvert.DeserializeObject<KlipperGcodeHelpRespone>(result.Result);
                return config?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperGcodeHelpRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "print/start", default, null, urlSegements)
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "print/pause")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "print/resume")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "print/cancel")
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
                var axes = new[] {x, y, z};
                StringBuilder gcodeCommand = new();
                var anyAxesMoves = axes.Any(a => a != double.PositiveInfinity);
                if (anyAxesMoves) gcodeCommand.Append($"G1 F{speed}");
                if (x != double.PositiveInfinity) gcodeCommand.Append($" X{(relative ? "" : "+")}{x}");
                if (y != double.PositiveInfinity) gcodeCommand.Append($" Y{(relative ? "" : "+")}{y}");
                if (z != double.PositiveInfinity) gcodeCommand.Append($" Z{(relative ? "" : "+")}{z}");
                if (anyAxesMoves) gcodeCommand.Append("\n"); // add new line to end command string
                if (e != double.PositiveInfinity) gcodeCommand.Append($"M83\nG1 E{e} F{speed};");
                

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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, "system_info").ConfigureAwait(false);
                KlipperMachineInfoRespone config = JsonConvert.DeserializeObject<KlipperMachineInfoRespone>(result.Result);
                return config?.Result?.SystemInfo;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperMachineInfoRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "shutdown")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "reboot")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "services/restart", default, null, urlSegements)
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "services/stop", default, null, urlSegements)
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "services/start", default, null, urlSegements)
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, "proc_stats").ConfigureAwait(false);
                KlipperMoonrakerProcessStatsRespone config = JsonConvert.DeserializeObject<KlipperMoonrakerProcessStatsRespone>(result.Result);
                return config?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperMoonrakerProcessStatsRespone),
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

                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "files/list", default, null, urlSegements).ConfigureAwait(false);
                KlipperFileListRespone files = JsonConvert.DeserializeObject<KlipperFileListRespone>(result.Result);
                if (includeGcodeMeta)
                {
                    for (int i = 0; i < files?.Result?.Count; i++)
                    {
                        KlipperFile current = files?.Result[i];
                        current.GcodeMeta = await GetGcodeMetadataAsync(current.Path).ConfigureAwait(false);
                        if (current.GcodeMeta?.Thumbnails?.Count > 0)
                        {
                            current.Image = await GetGcodeThumbnailImageAsync(current.GcodeMeta, 1)
                                .ConfigureAwait(false)
                                ;
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
                    TargetType = nameof(KlipperFileListRespone),
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
                if (string.IsNullOrEmpty(fileName)) return resultObject;

                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("filename", fileName);

                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "files/metadata", default, null, urlSegements).ConfigureAwait(false);
                KlipperGcodeMetaRespone queryResult = JsonConvert.DeserializeObject<KlipperGcodeMetaRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperGcodeMetaRespone),
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

        public async Task RefreshGcodeMetadataAsync(string fileName)
        {
            try
            {
                KlipperGcodeMetaResult meta = await GetGcodeMetadataAsync(fileName).ConfigureAwait(false);
                GcodeMeta = meta;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                GcodeMeta = null;
            }
        }

        public async Task<byte[]> GetGcodeThumbnailImageAsync(string relativePath, int timeout = 10000)
        {
            try
            {
                //Uri target = new($"{FullWebAddress}/server/files/gcodes/{relativePath}");
                string target = $"{FullWebAddress}/server/files/gcodes/{relativePath}";
                byte[] thumb = await DownloadFileFromUriAsync(target, timeout)
                    .ConfigureAwait(false)
                    ;

                return thumb;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new byte[0];
            }
        }
        public async Task<byte[]> GetGcodeThumbnailImageAsync(KlipperGcodeMetaResult gcodeMeta, int index = 0, int timeout = 10000)
        {
            string path = gcodeMeta.Thumbnails.Count > index ?
                gcodeMeta.Thumbnails[index].RelativePath : gcodeMeta.Thumbnails.FirstOrDefault().RelativePath;

            string subfolder = string.Empty;
            if (gcodeMeta.Filename.Contains("/"))
            {
                subfolder = gcodeMeta.Filename.Substring(0, gcodeMeta.Filename.LastIndexOf("/"));
                subfolder += "/";
            }

            return string.IsNullOrEmpty(path) ? null : await GetGcodeThumbnailImageAsync(subfolder + path, timeout)
                .ConfigureAwait(false)
                ;
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

                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "files/directory", default, null, urlSegements).ConfigureAwait(false);

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
                    TargetType = nameof(KlipperDirectoryInfoRespone),
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

        public async Task<List<KlipperDirectory>> GetAvailableDirectoriesAsync(string path = "")
        {
            List<KlipperDirectory> resultObject = new();
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = "gcodes";
                }
                KlipperDirectoryInfoResult result = await GetDirectoryInformationAsync(path, false).ConfigureAwait(false);
                resultObject = result?.Dirs ?? new();
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task RefreshAvailableDirectorienAsync(string path = "")
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = "gcodes";
                }
                List<KlipperDirectory> result = await GetAvailableDirectoriesAsync(path).ConfigureAwait(false);
                AvailableDirectories = new(result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"files/directory", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperDirectoryActionRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"files/directory", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperDirectoryActionRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"files/move", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperDirectoryActionRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"files/copy", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperDirectoryActionRespone),
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

        public async Task<byte[]> DownloadFileAsync(string relativeFilePath)
        {
            try
            {
                //Uri uri = new($"{FullWebAddress}/server/files/{relativeFilePath}");
                string uri = $"{FullWebAddress}/server/files/{relativeFilePath}";
                byte[] file = await DownloadFileFromUriAsync(uri)
                    .ConfigureAwait(false)
                    ;
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
                    TargetType = nameof(KlipperFileActionResult),
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
                    TargetType = nameof(KlipperFileActionResult),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"files/{root}/{filePath}")
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
                    TargetType = nameof(KlipperDirectoryActionResult),
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
            if (string.IsNullOrEmpty(filePath))
            {
                return resultObject;
            }
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"files/{filePath}")
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
                    TargetType = nameof(KlipperDirectoryActionRespone),
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

        public async Task<byte[]> DownloadLogFileAsync(KlipperLogFileTypes logType)
        {
            try
            {
                //Uri uri = new($"{FullWebAddress}/server/files/{logType.ToString().ToLower()}.log");
                string uri = $"{FullWebAddress}/server/files/{logType.ToString().ToLower()}.log";
                byte[] file = await DownloadFileFromUriAsync(uri)
                    .ConfigureAwait(false);
                    ;
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

        // Doc: https://moonraker.readthedocs.io/en/latest/web_api/#login-user
        public async Task<KlipperUserActionResult> LoginUserAsync(string username, string password)
        {
            KlipperApiRequestRespone result = new();
            KlipperUserActionResult resultObject = null;
            try
            {
                Username = username;
                Password = SecureStringHelper.ConvertToSecureString(password);

                object cmd = new
                {
                    username = username,
                    password = password,
                    source = "moonraker",
                };
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "login", cmd, default).ConfigureAwait(false);
                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);
                
                IsLoggedIn = queryResult != null;
                if (IsLoggedIn)
                {
                    // Needed for websocket connection
                    KlipperAccessTokenResult apiToken = await GetApiKeyAsync();
                    //KlipperAccessTokenResult oneshot = await GetOneshotTokenAsync();
                    API = apiToken?.Result;
                }
                UserToken = queryResult?.Result?.Token;
                RefreshToken = queryResult?.Result?.RefreshToken;

                OnLoginChanged(new()
                {
                    UserName = username,
                    Action = "login",
                    UserToken = UserToken,
                    RefreshToken = RefreshToken,
                    Succeeded = queryResult != null,
                });

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperUserActionRespone),
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

        public async Task<string> LoginUserForApiKeyAsync(string username, string password)
        {
            try
            {
                KlipperUserActionResult result = await LoginUserAsync(username, password).ConfigureAwait(false);

                if (IsLoggedIn)
                {
                    KlipperAccessTokenResult apiToken = await GetApiKeyAsync();
                    return apiToken?.Result;
                }
                return string.Empty;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return string.Empty;
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "refresh_jwt", cmd).ConfigureAwait(false);
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
                    TargetType = nameof(KlipperUserActionRespone),
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "user/password", cmd).ConfigureAwait(false);
                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperUserActionRespone),
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "logout").ConfigureAwait(false);
                UserToken = string.Empty;
                RefreshToken = string.Empty;

                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);
                IsLoggedIn = !(queryResult != null);
                OnLoginChanged(new()
                {
                    UserName = queryResult?.Result?.Username,
                    Action = "logout",
                    UserToken = UserToken,
                    RefreshToken = RefreshToken,
                    Succeeded = queryResult != null,
                });
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperUserActionRespone),
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Get, "user").ConfigureAwait(false);
                KlipperUserRespone queryResult = JsonConvert.DeserializeObject<KlipperUserRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperUserRespone),
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

                // This operation needs a valid token / api key
                if (string.IsNullOrEmpty(API))
                {
                    KlipperAccessTokenResult token = await GetOneshotTokenAsync().ConfigureAwait(false);
                    API = token?.Result;
                }
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "user", cmd).ConfigureAwait(false);
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
                    TargetType = nameof(KlipperUserActionRespone),
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Delete, "user", cmd).ConfigureAwait(false);

                KlipperUserActionRespone queryResult = JsonConvert.DeserializeObject<KlipperUserActionRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperUserActionRespone),
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Get, "users/list").ConfigureAwait(false);
                KlipperUserListRespone queryResult = JsonConvert.DeserializeObject<KlipperUserListRespone>(result.Result);
                return queryResult?.Result?.Users;

            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperUserListRespone),
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
            List<string> resultObject = new();
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"database/list")
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
                    TargetType = nameof(KlipperDatabaseNamespaceListRespone),
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
        public async Task RefreshDatabaseNamespacesAsync()
        {
            try
            {
                List<string> result = await ListDatabaseNamespacesAsync().ConfigureAwait(false);
                AvailableNamespaces = result ?? new();
                if(AvailableNamespaces?.Count > 0)
                {
                    // Try to detect the current operating system
                    OperatingSystem = AvailableNamespaces.Contains("mainsail") ? 
                        MoonrakerOperatingSystems.MainsailOS :
                        MoonrakerOperatingSystems.FluiddPi
                        ;
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                AvailableNamespaces = new();
            }
        }

        public async Task<Dictionary<string, object>> GetDatabaseItemAsync(string namespaceName, string key = "")
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, object> resultObject = null;
            try
            {
                if(AvailableNamespaces?.Count == 0 || AvailableNamespaces == null)
                {
                    AvailableNamespaces = await ListDatabaseNamespacesAsync();
                }
                if (!AvailableNamespaces.Contains(namespaceName))
                {
                    throw new ArgumentOutOfRangeException(namespaceName, "The requested namespace name was not found in the database!");
                }
                
                Dictionary<string, string> urlSegements = new();
                urlSegements.Add("namespace", namespaceName);
                if (!string.IsNullOrEmpty(key)) urlSegements.Add("key", key);

                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"database/item", default, null, urlSegements)
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
                    TargetType = nameof(KlipperDatabaseItemRespone),
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

        public async Task<List<KlipperDatabaseWebcamConfig>> GetWebCamSettingsAsync()
        {
            string resultString = string.Empty;
            List<KlipperDatabaseWebcamConfig> resultObject = new();
            try
            {
                // Both operating systems handles their datababase namespaces and keys differently....
                // @fluidd
                // It seems that the webcams setting are also stored in the namespace=webcams
                //string currentNameSpace = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "webcams";
                //string currentKey = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "webcam" : "";
                
                // Seems to be the write way for both, MainsailOS and Fluidd
                string currentNameSpace = "webcams";
                string currentKey = "";

                Dictionary<string, object> result = await GetDatabaseItemAsync(currentNameSpace, currentKey).ConfigureAwait(false);
                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (pair == null) return resultObject;
                
                //resultString = pair.Value.ToString();
                resultString = pair.Value.Value.ToString();

                switch (OperatingSystem)
                {
                    /*
                    case MoonrakerOperatingSystems.MainsailOS:
                        KlipperDatabaseMainsailValueWebcam mainsailObject = JsonConvert.DeserializeObject<KlipperDatabaseMainsailValueWebcam>(resultString);
                        if(mainsailObject?.Configs != null)
                        {
                            IEnumerable<KlipperDatabaseWebcamConfig> temp = mainsailObject.Configs.Select(item => new KlipperDatabaseWebcamConfig()
                            {
                                Enabled = true,
                                Name = item.Name,
                                Icon = item.Icon,
                                FlipX = item.FlipX,
                                FlipY = item.FlipY,
                                Service = item.Service,
                                TargetFps = item.TargetFps,
                                Url = item.Url,
                            });
                            resultObject = new(temp);
                        }
                        break;
                    */
                    case MoonrakerOperatingSystems.MainsailOS:
                    case MoonrakerOperatingSystems.FluiddPi:
                        Dictionary<Guid, KlipperDatabaseFluiddValueWebcamConfig> fluiddObject = JsonConvert.DeserializeObject<Dictionary<Guid, KlipperDatabaseFluiddValueWebcamConfig>>(resultString);
                        if(fluiddObject?.Count > 0)
                        {
                            IEnumerable<KlipperDatabaseWebcamConfig> temp = fluiddObject.Select(item => new KlipperDatabaseWebcamConfig()
                            {
                                Id = item.Key,
                                Enabled = item.Value.Enabled,
                                Name = item.Value.Name,
                                FlipX = item.Value.FlipX,
                                FlipY = item.Value.FlipY,
                                Service = item.Value.Service,
                                TargetFps = item.Value.Fpstarget,
                                Url = item.Value.UrlStream,
                                UrlSnapshot = item.Value.UrlSnapshot,
                                Rotation = item.Value.Rotation,
                            });
                            resultObject = new(temp);
                        }
                        break;
                    default:
                        break;
                }

                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValueWebcam),
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

        public async Task RefreshWebCamConfigAsync()
        {
            try
            {
                List<KlipperDatabaseWebcamConfig> result = await GetWebCamSettingsAsync().ConfigureAwait(false);
                WebCamConfigs = result ?? new();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                WebCamConfigs = new();
            }
        }

        public async Task<KlipperDatabaseSettingsGeneral> GetGeneralSettingsAsync()
        {
            string resultString = string.Empty;
            KlipperDatabaseSettingsGeneral resultObject = null;
            try
            {
                string currentNamespace = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                string currentKey = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "general" : "uiSettings";

                Dictionary<string, object> result = await GetDatabaseItemAsync(currentNamespace, currentKey).ConfigureAwait(false);

                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (pair == null) return resultObject;

                //resultString = pair.Value.ToString();
                resultString = pair.Value.Value.ToString();

                switch (OperatingSystem)
                {
                    case MoonrakerOperatingSystems.MainsailOS:
                        KlipperDatabaseMainsailValueGeneral mainsailObject = JsonConvert.DeserializeObject<KlipperDatabaseMainsailValueGeneral>(resultString);
                        if (mainsailObject != null)
                        {
                            resultObject = new()
                            {
                                DisplayCancelPrint = mainsailObject.DisplayCancelPrint,
                                Printername = mainsailObject.Printername,
                            };
                        }
                        break;
                    case MoonrakerOperatingSystems.FluiddPi:
                        KlipperDatabaseFluiddValueUiSettings fluiddObject = JsonConvert.DeserializeObject<KlipperDatabaseFluiddValueUiSettings>(resultString);
                        if (fluiddObject?.General != null)
                        {
                            resultObject = new()
                            {
                                Locale = fluiddObject.General.Locale,
                                Printername = fluiddObject.General.InstanceName,
                            };
                        }
                        break;
                    default:
                        break;
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValueGeneral),
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

        public async Task RefreshGeneralSettingsAsync()
        {
            try
            {
                KlipperDatabaseSettingsGeneral result = await GetGeneralSettingsAsync().ConfigureAwait(false);
                HostName = result?.Printername;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                HostName = string.Empty;
            }
        }

        public async Task<List<KlipperDatabaseRemotePrinter>> GetRemotePrintersAsync()
        {
            string resultString = string.Empty;
            List<KlipperDatabaseRemotePrinter> resultObject = new();
            try
            {
                /*
                if (OperatingSystem != MoonrakerOperatingSystems.MainsailOS)
                {
                    throw new NotSupportedException($"The method '{nameof(GetRemotePrintersAsync)}() is only support on '{MoonrakerOperatingSystems.MainsailOS}!");
                }*/
                string currentNamespace = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                string currentKey = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "remote_printers" : "uiSettings";

                Dictionary<string, object> result = await GetDatabaseItemAsync(currentNamespace, currentKey).ConfigureAwait(false);
                
                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (pair == null) return resultObject;

                //resultString = pair.Value.ToString();
                resultString = pair.Value.Value.ToString();

                switch (OperatingSystem)
                {
                    case MoonrakerOperatingSystems.MainsailOS:
                        List<KlipperDatabaseMainsailValueRemotePrinter> mainsailObject = JsonConvert.DeserializeObject<List<KlipperDatabaseMainsailValueRemotePrinter>>(resultString);
                        if (mainsailObject != null)
                        {
                            resultObject = new(mainsailObject.Select(item => new KlipperDatabaseRemotePrinter()
                            {
                                Hostname = item.Hostname,
                                Port = item.Port,
                                Settings = item.Settings,
                                WebPort = item.WebPort,
                            }));
                        }
                        break;
                    case MoonrakerOperatingSystems.FluiddPi:
#if DEBUG
                        throw new NotSupportedException($"The method '{nameof(GetRemotePrintersAsync)}() is only support on '{MoonrakerOperatingSystems.MainsailOS}!");
#endif
                    /*
                    KlipperDatabaseFluiddValueUiSettings fluiddObject = JsonConvert.DeserializeObject<KlipperDatabaseFluiddValueUiSettings>(resultString);
                    if (fluiddObject?.General != null)
                    {
                        resultObject = new()
                        {
                            Locale = fluiddObject.General.Locale,
                            Printername = fluiddObject.General.InstanceName,
                        };
                    }
                    break;
                    */
                    default:
                        break;
                }

                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValueRemotePrinter),
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

        public async Task RefreshRemotePrintersAsync()
        {
            try
            {
                List<KlipperDatabaseRemotePrinter> result = await GetRemotePrintersAsync().ConfigureAwait(false);
                Printers = new(result ?? new());
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Printers = new();
            }
        }

        public async Task<List<KlipperDatabaseTemperaturePreset>> GetDashboardPresetsAsync()
        {
            string resultString = string.Empty;
            List<KlipperDatabaseTemperaturePreset> resultObject = new();
            try
            {
                string currentNamespace = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "mainsail" : "fluidd";
                string currentKey = OperatingSystem == MoonrakerOperatingSystems.MainsailOS ? "presets" : "uiSettings";

                Dictionary<string, object> result = await GetDatabaseItemAsync(currentNamespace, currentKey).ConfigureAwait(false);
                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (pair == null) return resultObject;

                resultString = pair.Value.Value.ToString();

                switch (OperatingSystem)
                {
                    case MoonrakerOperatingSystems.MainsailOS:
                        // New since latest update
                        KlipperDatabaseMainsailValuePresets mainsailObject = JsonConvert.DeserializeObject<KlipperDatabaseMainsailValuePresets> (resultString);
                        //List<KlipperDatabaseMainsailValuePreset> mainsailObject = JsonConvert.DeserializeObject<List<KlipperDatabaseMainsailValuePreset>>(resultString);
                        if (mainsailObject != null)
                        {
                            IEnumerable<KlipperDatabaseTemperaturePreset> temp = mainsailObject.Presets.Select((item, index) => new KlipperDatabaseTemperaturePreset()
                            {        
                                //Id = Guid.NewGuid(),
                                Id = item.Key,
                                Name = item.Value.Name,
                                Gcode = item.Value.Gcode,
                                Values = new(item.Value.Values.Select(valuePair => new KeyValuePair<string, KlipperDatabaseTemperaturePresetHeater>(
                                    valuePair.Key, new KlipperDatabaseTemperaturePresetHeater()
                                {
                                    Name = valuePair.Key,
                                    Active = valuePair.Value.Bool,
                                    Type = valuePair.Value.Type,
                                    Value = valuePair.Value.Value,
                                }))),
                            });                          
                            resultObject = new(temp);
                        }
                        break;
                    case MoonrakerOperatingSystems.FluiddPi:
                        //resultString = pair.Value.ToString();
                        KlipperDatabaseFluiddValueUiSettings fluiddObject = JsonConvert.DeserializeObject<KlipperDatabaseFluiddValueUiSettings>(resultString);
                        if (fluiddObject?.Dashboard?.TempPresets != null)
                        {
                            IEnumerable<KlipperDatabaseTemperaturePreset> temp = fluiddObject.Dashboard.TempPresets.Select(item => new KlipperDatabaseTemperaturePreset()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Gcode = item.Gcode,
                                Values = new(item.Values.Select(valuePair => new KeyValuePair<string, KlipperDatabaseTemperaturePresetHeater>(
                                    valuePair.Key, new KlipperDatabaseTemperaturePresetHeater()
                                    {
                                        Name = valuePair.Key,
                                        Active = valuePair.Value.Active,
                                        Type = valuePair.Value.Type,
                                        Value = valuePair.Value.Value,
                                    }))),
                            });
                            resultObject = new(temp);
                        }
                        break;
                    default:
                        break;
                }
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValuePreset),
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
        public async Task RefreshDashboardPresetsAsync()
        {
            try
            {
                // Could be null if no presets have been defined yet
                List<KlipperDatabaseTemperaturePreset> result = await GetDashboardPresetsAsync().ConfigureAwait(false);
                Presets = result ?? new();
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Presets = new();
            }
        }

        public async Task<KlipperDatabaseMainsailValueHeightmapSettings> GetMeshHeightMapSettingsAsync()
        {
            string resultString = string.Empty;
            KlipperDatabaseMainsailValueHeightmapSettings resultObject = null;
            try
            {
                if (OperatingSystem != MoonrakerOperatingSystems.MainsailOS)
                {
                    throw new NotSupportedException($"The method '{nameof(GetMeshHeightMapSettingsAsync)}() is only support on '{MoonrakerOperatingSystems.MainsailOS}!");
                }

                Dictionary<string, object> result = await GetDatabaseItemAsync("mainsail", "heightmap").ConfigureAwait(false);
                KeyValuePair<string, object>? pair = result?.FirstOrDefault();
                if (pair == null) return resultObject;

                //resultString = pair.Value.ToString();
                resultString = pair.Value.Value.ToString();

                resultObject = JsonConvert.DeserializeObject<KlipperDatabaseMainsailValueHeightmapSettings>(resultString);
                return resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = resultString,
                    TargetType = nameof(KlipperDatabaseMainsailValueHeightmapSettings),
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, "database/item", cmd).ConfigureAwait(false);
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
                    TargetType = nameof(KlipperDatabaseItemRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"database/item", default, null, urlSegements)
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
                    TargetType = nameof(KlipperDatabaseItemRespone),
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

        public async Task<KlipperJobQueueResult> GetJobQueueStatusAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "job_queue/status").ConfigureAwait(false);

                KlipperJobQueueRespone queryResult = JsonConvert.DeserializeObject<KlipperJobQueueRespone>(result.Result);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperJobQueueRespone),
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
        public async Task<List<KlipperJobQueueItem>> GetJobQueueListAsync()
        {
            List<KlipperJobQueueItem> resultObject = new();
            try
            {
                KlipperJobQueueResult result = await GetJobQueueStatusAsync().ConfigureAwait(false);
                JobListState = result?.QueueState ?? "";
                return result?.QueuedJobs ?? resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                JobListState = "";
                return resultObject;
            }
        }

        public async Task RefreshJobQueueStatusAsync()
        {
            try
            {
                ObservableCollection<KlipperJobQueueItem> jobList = new();
                List<KlipperJobQueueItem> result = await GetJobQueueListAsync().ConfigureAwait(false);
                JobList = result != null ? new(result) : jobList;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                //JobListState = "";
                JobList = new();
            }
        }

        public async Task<KlipperJobQueueResult> EnqueueJobAsync(string job)
        {
            return await EnqueueJobsAsync(new string[] { job }).ConfigureAwait(false);
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
                result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, "job_queue/job", cmd).ConfigureAwait(false);
                KlipperJobQueueRespone queryResult = JsonConvert.DeserializeObject<KlipperJobQueueRespone>(result.Result);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperJobQueueRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"job_queue/job", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperJobQueueRespone),
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

        public async Task<KlipperJobQueueResult> RemoveJobsAsync(string[] jobIds)
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("job_ids", string.Join(",", jobIds));

                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"job_queue/job", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperJobQueueRespone),
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
        public async Task<KlipperJobQueueResult> RemoveJobAsync(string jobId)
        {
            return await RemoveJobsAsync(new string[] { jobId }).ConfigureAwait(false);
        }

        public async Task<KlipperJobQueueResult> PauseJobQueueAsync()
        {
            KlipperApiRequestRespone result = new();
            KlipperJobQueueResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"job_queue/pause")
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
                    TargetType = nameof(KlipperJobQueueRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"job_queue/start")
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
                    TargetType = nameof(KlipperJobQueueRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, $"update/status", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                KlipperUpdateStatusRespone queryResult = JsonConvert.DeserializeObject<KlipperUpdateStatusRespone>(result.Result);
                if(queryResult?.Result?.VersionInfo != null)
                {
                    foreach(KeyValuePair<string, KlipperUpdateVersionInfo> keypair in queryResult?.Result?.VersionInfo)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(keypair.Value.Name))
                            {
                                keypair.Value.Name = keypair.Key;
                            }
                        }
                        catch(Exception) { continue; }
                    }
                }
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new KlipprtJsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result.Result,
                    TargetType = nameof(KlipperUpdateStatusRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/full")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/moonraker")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/klipper")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/client", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/system")
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/recover", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, $"device_power/devices")
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
                    TargetType = nameof(KlipperDeviceListRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, $"device_power/device", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperDeviceStatusRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"device_power/device", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperDeviceStatusRespone),
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
                    {
                        deviceList.Append("&");
                    }
                }

                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, $"device_power/status?{deviceList}")
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
                    TargetType = nameof(KlipperDeviceStatusRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"device_power/on?{deviceList}")
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
                    TargetType = nameof(KlipperDeviceStatusRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"device_power/off?{deviceList}")
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
                    TargetType = nameof(KlipperDeviceStatusRespone),
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
        public async Task<OctoprintApiVersionResult> GetOctoPrintApiVersionInfoAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiVersionResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"version")
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
                    TargetType = nameof(OctoprintApiVersionResult),
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

        public async Task<OctoprintApiServerStatusResult> GetOctoPrintApiServerStatusAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiServerStatusResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"server")
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
                    TargetType = nameof(OctoprintApiServerStatusResult),
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

        public async Task<OctoprintApiServerStatusResult> GetOctoPrintApiUserInformationAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiServerStatusResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"login")
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
                    TargetType = nameof(OctoprintApiServerStatusResult),
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

        public async Task<OctoprintApiSettingsResult> GetOctoPrintApiSettingsAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiSettingsResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"settings")
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
                    TargetType = nameof(OctoprintApiSettingsResult),
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

        public async Task<OctoprintApiJobResult> GetOctoPrintApiJobStatusAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiJobResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"job")
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
                    TargetType = nameof(OctoprintApiJobResult),
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

        public async Task<OctoprintApiPrinterStatusResult> GetOctoPrintApiPrinterStatusAsync()
        {
            KlipperApiRequestRespone result = new();
            OctoprintApiPrinterStatusResult resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"printer")
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
                    TargetType = nameof(OctoprintApiPrinterStatusResult),
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

        public async Task<bool> SendOctoPrintApiGcodeCommandAsync(string[] commands)
        {
            try
            {
                object cmd = new
                {
                    commands = commands
                };
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Post, "printer/command", cmd)
                    .ConfigureAwait(false);
                return GetQueryResult(result.Result, true);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> SendOctoPrintApiGcodeCommandAsync(string command)
        {
            return await SendOctoPrintApiGcodeCommandAsync(new string[] { command }).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, OctoprintApiPrinter>> GetOctoPrintApiPrinterProfilesAsync()
        {
            KlipperApiRequestRespone result = new();
            Dictionary<string, OctoprintApiPrinter> resultObject = null;
            try
            {
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"printerprofiles")
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
                    TargetType = nameof(OctoprintApiPrinterProfilesResult),
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
        public async Task<List<KlipperJobItem>> GetHistoryJobListAsync(int limit = 100, int start = 0, double since = -1, double before = -1, string order = "asc")
        {
            List<KlipperJobItem> resultObject = null;
            try
            {
                KlipperHistoryResult result = await GetHistoryJobListResultAsync(limit, start, since, before, order).ConfigureAwait(false);
                return result?.Jobs;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<KlipperHistoryResult> GetHistoryJobListResultAsync(int limit = 100, int start = 0, double since = -1, double before = -1, string order = "asc")
        {
            KlipperApiRequestRespone result = new();
            KlipperHistoryResult resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "limit", $"{limit}" },
                    { "start", $"{start}" }
                };
                if (since >= 0) urlSegments.Add("since", $"{since}");
                if (before >= 0) urlSegments.Add("before", $"{before}");
                urlSegments.Add("order", order);

                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"history/list", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperHistoryRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"history/totals")
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
                    TargetType = nameof(KlipperHistoryTotalRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"history/reset_totals")
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
                    TargetType = nameof(KlipperHistoryTotalRespone),
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
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"history/job", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperHistorySingleJobRespone),
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

        public async Task<List<string>> DeleteHistoryJobAsync(KlipperJobItem item)
        {
            return await DeleteHistoryJobAsync(item?.JobId?.ToString()).ConfigureAwait(false);
        }
        public async Task<List<string>> DeleteHistoryJobAsync(string uid)
        {
            KlipperApiRequestRespone result = new();
            List<string> resultObject = null;
            try
            {
                if (string.IsNullOrEmpty(uid)) return resultObject;

                Dictionary<string, string> urlSegments = new();
                urlSegments.Add("uid", uid);

                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"history/job", jsonObject: null, cts: default, urlSegments: urlSegments)
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
                    TargetType = nameof(KlipperHistoryJobDeletedRespone),
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
            if (obj is not MoonrakerClient item)
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
                StopListeningAsync();
                DisconnectWebSocketAsync();
            }
        }
        #endregion
    }
}
