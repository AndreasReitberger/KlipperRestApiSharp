using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Moonraker.Extensions;
using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Moonraker.Structs;
using AndreasReitberger.API.Print3dServer.Core;
using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Events;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker
{
    // Needs: https://github.com/Arksine/moonraker/blob/master/docs/web_api.md
    // Docs: https://moonraker.readthedocs.io/en/latest/configuration/
    public partial class MoonrakerClient : Print3dServerClient, IPrint3dServerClient
    {
        #region Variables
        readonly bool _enableCooldown = true;
        readonly int _cooldownFallback = 4;

        int _cooldownExtruder = 4;
        int _cooldownTemperatureSensor = 4;
        int _cooldownCpuUsage = 4;
        int _cooldownSystemMemory = 4;
        //int _cooldownHeaterBed = 4;

        #endregion

        #region Instance
        static MoonrakerClient? _instance = null;
        static readonly object Lock = new();
        public new static MoonrakerClient Instance
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

        #endregion

        #region Properties

        #region Connection

        [ObservableProperty]
        MoonrakerOperatingSystems operatingSystem = MoonrakerOperatingSystems.MainsailOS;

        [ObservableProperty]
        string hostName = string.Empty;

        #endregion

        #region General

        [ObservableProperty]
        bool refreshHeatersDirectly = true;

        #endregion

        #region Api & Version

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string moonrakerVersion = string.Empty;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        List<string> registeredDirectories = new();

        #endregion

        #region Jobs

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string jobListState = string.Empty;
        partial void OnJobListStateChanging(string value)
        {
            OnKlipperJobListStateChanged(new KlipperJobListStateChangedEventArgs()
            {
                NewJobListStatus = value,
                PreviousJobListStatus = JobListState,
                SessionId = SessionId,
                CallbackId = -1,
                AuthToken = !string.IsNullOrEmpty(UserToken) ? UserToken : ApiKey,
            });
        }

        #endregion

        #region State & Config
        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string klipperState = string.Empty;
        partial void OnKlipperStateChanging(string value)
        {
            OnKlipperStateChangedEvent(new KlipperStateChangedEventArgs()
            {
                NewState = value,
                PreviousState = KlipperState,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        double cpuTemp = 0;
        partial void OnCpuTempChanged(double value)
        {
            OnKlipperCpuTemperatureChanged(new KlipperCpuTemperatureChangedEventArgs()
            {
                NewTemperature = value,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperServerConfig? config;
        partial void OnConfigChanged(KlipperServerConfig? value)
        {
            OnKlipperServerConfigChanged(new KlipperServerConfigChangedEventArgs()
            {
                NewConfiguration = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
            UpdateServerConfig(value);
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
#if ConcurrentDictionary
        ConcurrentDictionary<string, KlipperTemperatureSensorHistory> temperatureCache = new();
        partial void OnTemperatureCacheChanged(ConcurrentDictionary<string, KlipperTemperatureSensorHistory> value)
        {
            OnKlipperServerTemperatureCacheChanged(new KlipperTemperatureCacheChangedEventArgs()
            {
                CachedTemperatures = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }
#else
        Dictionary<string, KlipperTemperatureSensorHistory> temperatureCache = new();
        partial void OnTemperatureCacheChanged(Dictionary<string, KlipperTemperatureSensorHistory> value)
        {
            OnKlipperServerTemperatureCacheChanged(new KlipperTemperatureCacheChangedEventArgs()
            {
                CachedTemperatures = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }
#endif

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        List<KlipperGcode> gcodeCache = [];
        partial void OnGcodeCacheChanged(List<KlipperGcode> value)
        {
            OnKlipperServerGcodeCacheChanged(new KlipperGcodeCacheChangedEventArgs()
            {
                CachedGcodes = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperGcodeMetaResult? gcodeMeta;
        partial void OnGcodeMetaChanged(KlipperGcodeMetaResult? value)
        {
            OnKlipperGcodeMetaResultChanged(new KlipperGcodeMetaResultChangedEventArgs()
            {
                NewResult = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
            UpdateGcodeMetaDependencies();
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperStatusGcodeMove? gcodeMove;
        partial void OnGcodeMoveChanged(KlipperStatusGcodeMove? value)
        {
            OnKlipperGcodeMoveStateChanged(new KlipperGcodeMoveStateChangedEventArgs()
            {
                NewState = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
            SpeedFactor = value?.SpeedFactor * 100 ?? 100;
            FlowFactor = value?.ExtrudeFactor * 100 ?? 100;
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperStatusVirtualSdcard? virtualSdCard;
        partial void OnVirtualSdCardChanged(KlipperStatusVirtualSdcard? value)
        {
            OnKlipperVirtualSdCardStateChanged(new KlipperVirtualSdCardStateChangedEventArgs()
            {
                NewState = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        ConcurrentDictionary<string, KlipperStatusTemperatureSensor> temperatureSensors = new();
        partial void OnTemperatureSensorsChanged(ConcurrentDictionary<string, KlipperStatusTemperatureSensor> value)
        {
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
                        SessionId = SessionId,
                        CallbackId = -1,
                    });
                }
            }
            else
            {
                OnKlipperTemperatureSensorStatesChanged(new KlipperTemperatureSensorStatesChangedEventArgs()
                {
                    TemperatureStates = value,
                    SessionId = SessionId,
                    CallbackId = -1,
                });
            }
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        ConcurrentDictionary<string, double?> cpuUsage = new();
        partial void OnCpuUsageChanged(ConcurrentDictionary<string, double?> value)
        {
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
                        SessionId = SessionId,
                        CallbackId = -1,
                    });
                }
            }
            else
            {
                OnKlipperServerCpuUsageChanged(new KlipperCpuUsageChangedEventArgs()
                {
                    CpuUsage = value,
                    SessionId = SessionId,
                    CallbackId = -1,
                });
            }
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        ConcurrentDictionary<string, long?> systemMemory = new();
        partial void OnSystemMemoryChanged(ConcurrentDictionary<string, long?> value)
        {
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
                        SessionId = SessionId,
                        CallbackId = -1,
                    });
                }
            }
            else
            {
                OnKlipperServerSystemMemoryChanged(new KlipperSystemMemoryChangedEventArgs()
                {
                    SystemMemory = value,
                    SessionId = SessionId,
                    CallbackId = -1,
                });
            }
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        ConcurrentDictionary<string, KlipperStatusDriver> drivers = new();
        partial void OnDriversChanged(ConcurrentDictionary<string, KlipperStatusDriver> value)
        {
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
                        SessionId = SessionId,
                        CallbackId = -1,
                    });
                }
            }
            else
            {
                OnKlipperExtruderStatesChanged(new KlipperExtruderStatesChangedEventArgs()
                {
                    ExtruderStates = value,
                    SessionId = SessionId,
                    CallbackId = -1,
                });
            }
            */
        }

        /*
        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        bool isPrinting = false;
        partial void OnIsPrintingChanged(bool value)
        {
            UpdateCurrentPrintDependencies(value);
            OnKlipperIsPrintingStateChanged(new KlipperIsPrintingStateChangedEventArgs()
            {
                IsPrinting = value,
                IsPaused = IsPaused,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        bool isPaused = false;
        partial void OnIsPausedChanged(bool value)
        {
            OnKlipperIsPrintingStateChanged(new KlipperIsPrintingStateChangedEventArgs()
            {
                IsPrinting = IsPrinting,
                IsPaused = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }
        */

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        double progress = 0;
        partial void OnProgressChanging(double value)
        {
            OnKlipperIsPrintingProgressChanged(new KlipperIsPrintingProgressChangedEventArgs()
            {
                PreviousPrintProgress = Progress,
                NewPrintProgress = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        double printTime = 0;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        double totalPrintTime = 0;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        double remainingPrintTime = 0;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperStatusPrintStats? printStats;
        partial void OnPrintStatsChanging(KlipperStatusPrintStats? value)
        {
            OnKlipperPrintStateChanged(new KlipperPrintStateChangedEventArgs()
            {
                NewPrintState = value,
                PreviousPrintState = PrintStats,
                SessionId = SessionId,
                CallbackId = -1,
            });
            if (value?.ValidPrintState == true)
            {
                IsPrinting = value?.State == KlipperPrintStates.Printing;
                ActiveJobName = value?.Filename ?? string.Empty;
                ActiveJob = new KlipperStatusJob()
                {
                    FileName = ActiveJobName,
                    PrintDuration = value?.PrintDuration,
                    TotalPrintDuration = GcodeMeta?.EstimatedTime,
                    RemainingPrintTime = Convert.ToDouble(TotalPrintTime - value?.PrintDuration ?? 0),
                    Done = Math.Round(MathHelper.Clamp((Convert.ToDouble(value?.PrintDuration ?? 0)) / (TotalPrintTime / 100), 0, 100), 2),
                };

                // Update progress
                PrintTime = value?.PrintDuration ?? 0;
                TotalPrintTime = GcodeMeta?.EstimatedTime ?? 0;
                RemainingPrintTime = Convert.ToDouble(TotalPrintTime - value?.PrintDuration ?? 0);
                Progress = Math.Round(MathHelper.Clamp((Convert.ToDouble(value?.PrintDuration ?? 0)) / (TotalPrintTime / 100), 0, 100), 2);
            }
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperStatusMotionReport? motionReport;
        partial void OnMotionReportChanging(KlipperStatusMotionReport? value)
        {
            OnKlipperMotionReportChanged(new KlipperMotionReportChangedEventArgs()
            {
                NewState = value,
                PreviousState = MotionReport,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }
        partial void OnMotionReportChanged(KlipperStatusMotionReport? value)
        {
            UpdateMotionReportDependencies();
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperStatusIdleTimeout? idleState;
        partial void OnIdleStateChanging(KlipperStatusIdleTimeout? value)
        {
            OnKlipperIdleStateChanged(new KlipperIdleStateChangedEventArgs()
            {
                NewState = value,
                PreviousState = IdleState,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperStatusToolhead? toolHeadStatus;
        partial void OnToolHeadStatusChanged(KlipperStatusToolhead? value)
        {
            OnKlipperToolHeadStateChanged(new KlipperToolHeadStateChangedEventArgs()
            {
                NewToolheadState = value,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string activeJobName = string.Empty;
        partial void OnActiveJobNameChanging(string value)
        {
            OnKlipperActiveJobStateChanged(new KlipperActiveJobStateChangedEventArgs()
            {
                NewJobState = value,
                PreviousJobState = ActiveJobName,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperStatusDisplay? displayStatus;
        partial void OnDisplayStatusChanging(KlipperStatusDisplay? value)
        {
            OnKlipperDisplayStatusChanged(new KlipperDisplayStatusChangedEventArgs()
            {
                NewDisplayStatus = value,
                PreviousDisplayStatus = DisplayStatus,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        KlipperStatusFilamentSensor? filamentSensor = new() { FilamentDetected = false };
        partial void OnFilamentSensorChanging(KlipperStatusFilamentSensor? value)
        {
            OnKlipperFSensorChanged(new KlipperFSensorStateChangedEventArgs()
            {
                NewFSensorState = value,
                PreviousFSensorState = FilamentSensor,
                SessionId = SessionId,
                CallbackId = -1,
            });
        }


        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        List<KlipperDatabaseTemperaturePreset> presets = [];
        partial void OnPresetsChanged(List<KlipperDatabaseTemperaturePreset> value)
        {
            OnKlipperPresetsChanged(new KlipperPresetsChangedEventArgs()
            {
                NewPresets = value,
            });
        }
        #endregion

        #region ReadOnly

        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public new bool IsReady
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

        #region Constructor
        public MoonrakerClient()
        {
            Id = Guid.NewGuid();
            Target = Print3dServerTarget.Moonraker;
            ApiKeyRegexPattern = "";
            WebSocketTarget = "websocket";
            WebCamTarget = "/webcam/?action=stream";
            WebSocketMessageReceived += Client_WebSocketMessageReceived;
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
            Target = Print3dServerTarget.Moonraker;
            ApiKeyRegexPattern = "";
            WebSocketTarget = "websocket";
            WebCamTarget = "/webcam/?action=stream";
            WebSocketMessageReceived += Client_WebSocketMessageReceived;
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
            Target = Print3dServerTarget.Moonraker;
            ApiKeyRegexPattern = "";
            WebSocketTarget = "websocket";
            WebCamTarget = "/webcam/?action=stream";
            WebSocketMessageReceived += Client_WebSocketMessageReceived;
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
            if (WebSocket is not null)
            {
                /* SharpWebSocket
                if (WebSocket.ReadyState == WebSocketState.Open)
                    WebSocket.Close();
                WebSocket = null;
                */
            }
            WebSocketMessageReceived -= Client_WebSocketMessageReceived;
        }
        #endregion

        #region Init
        public new void InitInstance()
        {
            try
            {
                Instance = this;
                if (Instance is not null)
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
        public new void InitInstance(string serverAddress, int port = 80, string api = "", bool isSecure = false)
        {
            try
            {
                ServerAddress = serverAddress;
                ApiKey = api;
                Port = port;
                IsSecure = isSecure;

                Instance = this;

                if (Instance is not null)
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

        #region Methods

        #region Private

        #region Download
        public Task<byte[]?> DownloadFileFromUriAsync(string path, int timeout = 10000) => DownloadFileFromUriAsync(path, AuthHeaders, null, timeout);

        #endregion

        #region State & Config
        void UpdateServerConfig(KlipperServerConfig? newConfig)
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
        void UpdatePrinterInfo(KlipperPrinterStateMessageResult? newPrinterInfo)
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
                if (newIsPrintingState)
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
                    ActiveJobName = string.Empty;
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
                if (job == null || string.IsNullOrEmpty(job.FileName))
                {
                    GcodeMeta = null;
                }
                else
                {
                    Task.Run(async () =>
                    {
                        await RefreshGcodeMetadataAsync(job.FileName).ConfigureAwait(false);
                    });
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void UpdateMotionReportDependencies()
        {
            try
            {
                if (MotionReport?.LivePosition is not null && MotionReport.LivePosition.Count > 0)
                {
                    // [X, Y, Z, E] 
                    X = MotionReport.LivePosition[0];
                    Y = MotionReport.LivePosition[1];
                    Z = MotionReport.LivePosition[2];
                }
                if (MotionReport?.LiveVelocity is not null)
                {
                    LiveVelocity = Convert.ToDouble(MotionReport.LiveVelocity);
                }
                if (MotionReport?.LiveExtruderVelocity is not null)
                {
                    LiveExtruderVelocity = Convert.ToDouble(MotionReport.LiveExtruderVelocity);
                }
                if (GcodeMeta is not null)
                {
                    Layer = MathHelper.Clamp(Convert.ToInt64(Z / GcodeMeta.LayerHeight), 0, Layers);
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        void UpdateGcodeMetaDependencies()
        {
            try
            {
                Layers = GcodeMeta?.Layers ?? 0;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }


        #endregion

        #endregion

        #region Public

        #region Refresh

        public new Task StartListeningAsync(bool stopActiveListening = false, string[]? commandsOnConnect = null) => StartListeningAsync(WebSocketTargetUri, stopActiveListening, () => Task.Run(async () =>
        {
            List<Task> tasks =
            [
                RefreshExtruderStatusAsync(),
                RefreshHeaterBedStatusAsync(),
                RefreshPrintStatusAsync(),
                RefreshGcodeMoveStatusAsync(),
                RefreshMotionReportAsync(),
                RefreshToolHeadStatusAsync(),
            ];
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }), commandsOnConnect: commandsOnConnect
        );

        public new async Task RefreshAllAsync()
        {
            try
            {
                await base.RefreshAllAsync().ConfigureAwait(false);
                // Avoid multiple calls
                if (IsRefreshing) return;

                IsRefreshing = true;
                // Detects current operating system, must be called before each other Database method
                await RefreshDatabaseNamespacesAsync().ConfigureAwait(false);
                // Get a token for the WebSocket connection
                KlipperAccessTokenResult? oneshotToken = await GetOneshotTokenAsync().ConfigureAwait(false);
                SessionId = OneShotToken = oneshotToken?.Result ?? string.Empty;
                //await RefreshPrinterListAsync();
                List<Task> task =
                [
                    //Task.Delay(10),
                    RefreshServerConfigAsync(),
                    RefreshPrinterInfoAsync(),
                    RefreshDashboardPresetsAsync(),
                    RefreshAvailableFilesAsync(),
                    RefreshJobQueueStatusAsync(),
                    RefreshDirectoryInformationAsync(),
                    RefreshAvailableDirectorienAsync(),
                    RefreshServerCachedTemperatureDataAsync(),
                    RefreshServerCachedGcodesAsync(),

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
                ];
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

        #region CheckOnline

        public override async Task CheckOnlineAsync(int timeout = 10000)
        {
            CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
            await CheckOnlineAsync(cts).ConfigureAwait(false);
            cts?.Dispose();
        }
        public Task CheckOnlineAsync(CancellationTokenSource cts) => CheckOnlineAsync($"{MoonrakerCommands.Base}", AuthHeaders, "version", cts);

        public Task<bool> CheckIfApiIsValidAsync(int timeout = 10000) => CheckIfApiIsValidAsync($"{MoonrakerCommands.Base}", AuthHeaders, "version", timeout);

        public Task CheckServerIfApiIsValidAsync(int timeout = 10000) => CheckIfApiIsValidAsync(timeout);
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
                        ApiKey == tempKlipper.ApiKey &&
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
        public async Task<KlipperAccessTokenResult?> GetOneshotTokenAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperAccessTokenResult? resultObject = null;
            try
            {
                //object cmd = new { name = ScriptName };
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "oneshot_token",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Get, "oneshot_token").ConfigureAwait(false);
                KlipperAccessTokenResult? accessToken = GetObjectFromJson<KlipperAccessTokenResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                SessionId = accessToken?.Result ?? string.Empty;
                return accessToken;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperAccessTokenResult?> GetApiKeyAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperAccessTokenResult? resultObject = null;
            try
            {
                //object cmd = new { name = ScriptName };
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "api_key",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Get, "api_key").ConfigureAwait(false);
                KlipperAccessTokenResult? accessToken = GetObjectFromJson<KlipperAccessTokenResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                //API = accessToken?.Result;
                return accessToken;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
                KlipperAccessTokenResult? result = await GetApiKeyAsync().ConfigureAwait(false);
                ApiKey = result?.Result ?? string.Empty;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        #endregion

        #region Server Config
        public async Task RefreshServerConfigAsync()
        {
            try
            {
                KlipperServerConfig? result = await GetServerConfigAsync().ConfigureAwait(false);
                Config = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Config = null;
            }
        }

        public async Task<KlipperServerConfig?> GetServerConfigAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperServerConfig? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "config",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "config").ConfigureAwait(false);
                KlipperServerConfigRespone? config = GetObjectFromJson<KlipperServerConfigRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return config?.Result?.Config;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
#if ConcurrentDictionary
                TemperatureCache = result.ToConcurrent();
#else
                TemperatureCache = result;
#endif
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                TemperatureCache = new();
            }
        }
        public async Task<Dictionary<string, KlipperTemperatureSensorHistory>> GetServerCachedTemperatureDataAsync()
        {
            IRestApiRequestRespone? result = null;
            Dictionary<string, KlipperTemperatureSensorHistory> resultObject = [];
            try
            {
                //object cmd = new { name = ScriptName };
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "temperature_store",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "temperature_store").ConfigureAwait(false);
                KlipperServerTempDataRespone? tempData = GetObjectFromJson<KlipperServerTempDataRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return tempData?.Result ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public async Task RefreshServerCachedGcodesAsync()
        {
            try
            {
                List<KlipperGcode> result = await GetServerCachedGcodesAsync().ConfigureAwait(false);
                GcodeCache = result;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                GcodeCache = [];
            }
        }
        public async Task<List<KlipperGcode>> GetServerCachedGcodesAsync(long count = 100)
        {
            IRestApiRequestRespone? result = null;
            List<KlipperGcode> resultObject = [];
            try
            {
                //object cmd = new { name = ScriptName };
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"gcode_store?count={count}",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"gcode_store?count={count}").ConfigureAwait(false);
                KlipperGcodesRespone? tempData = GetObjectFromJson<KlipperGcodesRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return tempData?.Result?.Gcodes ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
                string targetUri = $"{MoonrakerCommands.Server}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                        method: Method.Post,
                       command: $"restart",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, "restart")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        #endregion

        #region Gcode API
        public async Task<bool> RunGcodeScriptAsync(string script)
        {
            try
            {
                Dictionary<string, string> urlSegements = new()
                {
                    { "script", script }
                };
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "gcode/script",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "gcode/script", default, null, urlSegements)
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                if (presetProfile?.Values?.ContainsKey("extruder") is not null && presetProfile?.Values?["extruder"]?.Active == true)
                {
                    cmds.Add($"SET_HEATER_TEMPERATURE HEATER=extruder TARGET={presetProfile.Values["extruder"].Value}");
                }
                if (presetProfile?.Values?.ContainsKey("extruder1") is not null && presetProfile?.Values?["extruder1"]?.Active == true)
                {
                    cmds.Add($"SET_HEATER_TEMPERATURE HEATER=extruder1 TARGET={presetProfile.Values["extruder1"].Value}");
                }
                if (presetProfile?.Values?.ContainsKey("heater_bed") is not null && presetProfile?.Values?["heater_bed"]?.Active == true)
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
            IRestApiRequestRespone? result = null;
            Dictionary<string, string> resultObject = [];
            try
            {
                string targetUri = $"{MoonrakerCommands.Printer}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "gcode/help",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Get, "gcode/help").ConfigureAwait(false);
                KlipperGcodeHelpRespone? config = GetObjectFromJson<KlipperGcodeHelpRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return config?.Result ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
                Dictionary<string, string> urlSegments = new()
                {
                    { "filename", fileName }
                };
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "print/start",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "print/start", default, null, urlSegements)
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "print/pause",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegements: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "print/pause")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "print/resume",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegements: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "print/resume")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                string targetUri = $"{MoonrakerCommands.Printer}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "print/cancel",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegements: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.printer, Method.Post, "print/cancel")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                var axes = new[] { x, y, z };
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
        public async Task<KlipperMachineInfo?> GetMachineSystemInfoAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperMachineInfo? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "system_info",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegements: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, "system_info").ConfigureAwait(false);
                KlipperMachineInfoRespone? config = GetObjectFromJson<KlipperMachineInfoRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return config?.Result?.SystemInfo;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "shutdown",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegements: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "shutdown")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "reboot",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegements: urlSegements,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "reboot")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                Dictionary<string, string> urlSegments = new()
                {
                    { "service", service }
                };

                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "services/restart",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "services/restart", default, null, urlSegements)
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public Task<bool> RestartSystemServiceAsync(KlipperServices service) => RestartSystemServiceAsync(service.ToString());

        public async Task<bool> StopSystemServiceAsync(string service)
        {
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "service", service }
                };

                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "services/stop",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "services/stop", default, null, urlSegments)
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public Task<bool> StopSystemServiceAsync(KlipperServices service) => StopSystemServiceAsync(service.ToString());

        public async Task<bool> StartSystemServiceAsync(string service)
        {
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "service", service }
                };

                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "services/start",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, "services/start", default, null, urlSegments)
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public Task<bool> StartSystemServiceAsync(KlipperServices service) => StartSystemServiceAsync(service.ToString());

        public async Task<KlipperMoonrakerProcessStatsResult?> GetMoonrakerProcessStatsAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperMoonrakerProcessStatsResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "proc_stats",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, "proc_stats").ConfigureAwait(false);
                KlipperMoonrakerProcessStatsRespone? config = GetObjectFromJson<KlipperMoonrakerProcessStatsRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return config?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        #region Authorization

        // Doc: https://moonraker.readthedocs.io/en/latest/web_api/#login-user
        public async Task<KlipperUserActionResult?> LoginUserAsync(string username, string password)
        {
            IRestApiRequestRespone? result = null;
            KlipperUserActionResult? resultObject = null;
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

                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "login",
                       jsonObject: cmd,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);

                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "login", cmd, default).ConfigureAwait(false);
                KlipperUserActionRespone? queryResult = GetObjectFromJson<KlipperUserActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);

                IsLoggedIn = queryResult is not null;
                UserToken = queryResult?.Result?.Token ?? string.Empty;
                RefreshToken = queryResult?.Result?.RefreshToken ?? string.Empty;
                // Must come after setting the `UserToken`, otherwise the request fails
                if (IsLoggedIn)
                {
                    // Needed for websocket connection
                    KlipperAccessTokenResult? apiToken = await GetApiKeyAsync();
                    ApiKey = apiToken?.Result ?? string.Empty;
                }

                OnLoginChanged(new()
                {
                    UserName = username,
                    Action = "login",
                    UserToken = UserToken,
                    RefreshToken = RefreshToken,
                    Succeeded = queryResult is not null,
                });

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
                KlipperUserActionResult? result = await LoginUserAsync(username, password).ConfigureAwait(false);

                if (IsLoggedIn)
                {
                    KlipperAccessTokenResult? apiToken = await GetApiKeyAsync();
                    return apiToken?.Result ?? string.Empty;
                }
                return string.Empty;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return string.Empty;
            }
        }

        public async Task<KlipperUserActionResult?> RefreshJSONWebTokenAsync(string refreshToken = "")
        {
            IRestApiRequestRespone? result = null;
            KlipperUserActionResult? resultObject = null;
            try
            {
                string token = !string.IsNullOrEmpty(refreshToken) ? refreshToken : RefreshToken;
                object cmd = new
                {
                    refresh_token = token,
                };
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "refresh_jwt",
                       jsonObject: cmd,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "refresh_jwt", cmd).ConfigureAwait(false);
                KlipperUserActionRespone? queryResult = GetObjectFromJson<KlipperUserActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);

                UserToken = queryResult?.Result?.Token ?? string.Empty;
                if (queryResult is not null && queryResult.Result is not null)
                    queryResult.Result.RefreshToken = token;

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperUserActionResult?> ResetUserPasswordAsync(string password, string newPassword)
        {
            IRestApiRequestRespone? result = null;
            KlipperUserActionResult? resultObject = null;
            try
            {
                object cmd = new
                {
                    password = password,
                    new_password = newPassword,
                };
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "user/password",
                       jsonObject: cmd,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "user/password", cmd).ConfigureAwait(false);
                KlipperUserActionRespone? queryResult = GetObjectFromJson<KlipperUserActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperUserActionResult?> LogoutCurrentUserAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperUserActionResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "logout",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "logout").ConfigureAwait(false);
                UserToken = string.Empty;
                RefreshToken = string.Empty;

                KlipperUserActionRespone? queryResult = GetObjectFromJson<KlipperUserActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                IsLoggedIn = !(queryResult is not null);
                OnLoginChanged(new()
                {
                    UserName = queryResult?.Result?.Username ?? string.Empty,
                    Action = "logout",
                    UserToken = UserToken,
                    RefreshToken = RefreshToken,
                    Succeeded = queryResult is not null,
                });
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperUser?> GetCurrentUserAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperUser? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "user",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Get, "user").ConfigureAwait(false);
                KlipperUserRespone? queryResult = GetObjectFromJson<KlipperUserRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperUserActionResult?> CreateUserAsync(string username, string password)
        {
            IRestApiRequestRespone? result = null;
            KlipperUserActionResult? resultObject = null;
            try
            {
                /*
                Dictionary<string, string> urlSegments = new()
                {
                    { "username", username },
                    { "password", password }
                };
                */
                object cmd = new
                {
                    username = username,
                    password = password
                };

                // This operation needs a valid token / api key
                if (string.IsNullOrEmpty(ApiKey))
                {
                    KlipperAccessTokenResult? token = await GetOneshotTokenAsync().ConfigureAwait(false);
                    ApiKey = token?.Result ?? string.Empty;
                }
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "user",
                       jsonObject: cmd,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Post, "user", cmd).ConfigureAwait(false);
                KlipperUserActionRespone? queryResult = GetObjectFromJson<KlipperUserActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;

            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperUserActionResult?> DeleteUserAsync(string username)
        {
            IRestApiRequestRespone? result = null;
            KlipperUserActionResult? resultObject = null;
            try
            {
                /*
                Dictionary<string, string> urlSegments = new()
                {
                    { "username", username }
                };
                */
                object cmd = new
                {
                    username = username,
                };
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: "user",
                       jsonObject: cmd,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Delete, "user", cmd).ConfigureAwait(false);
                KlipperUserActionRespone? queryResult = GetObjectFromJson<KlipperUserActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
            IRestApiRequestRespone? result = null;
            List<KlipperUser> resultObject = [];
            try
            {
                string targetUri = $"{MoonrakerCommands.Access}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "users/list",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.access, Method.Get, "users/list").ConfigureAwait(false);
                KlipperUserListRespone? queryResult = GetObjectFromJson<KlipperUserListRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result?.Users ?? [];
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        #region Job Queue APIs

        public async Task<KlipperJobQueueResult?> GetJobQueueStatusAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperJobQueueResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "job_queue/status",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "job_queue/status").ConfigureAwait(false);
                KlipperJobQueueRespone? queryResult = GetObjectFromJson<KlipperJobQueueRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public async Task<List<IPrint3dJob>> GetJobQueueListAsync()
        {
            List<IPrint3dJob> resultObject = [];
            try
            {
                KlipperJobQueueResult? result = await GetJobQueueStatusAsync().ConfigureAwait(false);
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
                ObservableCollection<IPrint3dJob> jobList = [];
                List<IPrint3dJob> result = await GetJobQueueListAsync().ConfigureAwait(false);
                Jobs = result is not null ? new(result) : jobList;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Jobs = [];
            }
        }

        public Task<KlipperJobQueueResult?> EnqueueJobAsync(string job) => EnqueueJobsAsync([job]);

        public async Task<KlipperJobQueueResult?> EnqueueJobsAsync(string[] jobs)
        {
            IRestApiRequestRespone? result = null;
            KlipperJobQueueResult? resultObject = null;
            try
            {
                object cmd = new
                {
                    filenames = jobs,
                };
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "job_queue/job",
                       jsonObject: cmd,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, "job_queue/job", cmd).ConfigureAwait(false);
                KlipperJobQueueRespone? queryResult = GetObjectFromJson<KlipperJobQueueRespone>(result?.Result, NewtonsoftJsonSerializerSettings);

                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperJobQueueResult?> RemoveAllJobAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperJobQueueResult? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "all", "true" }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: "job_queue/job",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"job_queue/job", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperJobQueueRespone? queryResult = GetObjectFromJson<KlipperJobQueueRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperJobQueueResult?> RemoveJobsAsync(string[] jobIds)
        {
            IRestApiRequestRespone? result = null;
            KlipperJobQueueResult? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "job_ids", string.Join(",", jobIds) }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: "job_queue/job",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"job_queue/job", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperJobQueueRespone? queryResult = GetObjectFromJson<KlipperJobQueueRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public Task<KlipperJobQueueResult?> RemoveJobAsync(string jobId) => RemoveJobsAsync(new string[] { jobId });

        public async Task<KlipperJobQueueResult?> PauseJobQueueAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperJobQueueResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "job_queue/pause",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"job_queue/pause")
                    .ConfigureAwait(false);
                */
                KlipperJobQueueRespone? queryResult = GetObjectFromJson<KlipperJobQueueRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<KlipperJobQueueResult?> StartJobQueueAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperJobQueueResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "job_queue/start",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"job_queue/start")
                    .ConfigureAwait(false);
                */
                KlipperJobQueueRespone? queryResult = GetObjectFromJson<KlipperJobQueueRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public async Task<KlipperUpdateStatusResult?> GetUpdateStatusAsync(bool refresh = false)
        {
            IRestApiRequestRespone? result = null;
            KlipperUpdateStatusResult? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "refresh", refresh ? "true" : "false" }
                };

                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "update/status",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, $"update/status", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperUpdateStatusRespone? queryResult = GetObjectFromJson<KlipperUpdateStatusRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                if (queryResult?.Result?.VersionInfo is not null)
                {
                    foreach (KeyValuePair<string, KlipperUpdateVersionInfo> keypair in queryResult.Result.VersionInfo)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(keypair.Value.Name))
                            {
                                keypair.Value.Name = keypair.Key;
                            }
                        }
                        catch (Exception) { continue; }
                    }
                }
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "update/full",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/full")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "update/moonraker",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/moonraker")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "update/klipper",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/klipper")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                Dictionary<string, string> urlSegments = new()
                {
                    { "name", clientName }
                };

                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "update/client",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/client", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "update/system",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/system")
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
                Dictionary<string, string> urlSegments = new()
                {
                    { "name", repoName },
                    { "hard", hard ? "true" : "false" }
                };

                string targetUri = $"{MoonrakerCommands.Machine}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "update/recover",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"update/recover", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result);
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
            IRestApiRequestRespone? result = null;
            List<KlipperDevice> resultObject = [];
            try
            {
                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "device_power/devices",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, $"device_power/devices")
                    .ConfigureAwait(false);
                */
                KlipperDeviceListRespone? queryResult = GetObjectFromJson<KlipperDeviceListRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result?.Devices ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
            IRestApiRequestRespone? result = null;
            Dictionary<string, string> resultObject = new();
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "device", device }
                };

                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "device_power/device",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, $"device_power/device", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperDeviceStatusRespone? queryResult = GetObjectFromJson<KlipperDeviceStatusRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.DeviceStates ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
            IRestApiRequestRespone? result = null;
            Dictionary<string, string> resultObject = new();
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "device", device },
                    { "action", action.ToString().ToLower() }
                };

                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "device_power/device",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"device_power/device", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperDeviceStatusRespone? queryResult = GetObjectFromJson<KlipperDeviceStatusRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.DeviceStates ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
            IRestApiRequestRespone? result = null;
            Dictionary<string, string> resultObject = new();
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

                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"device_power/status?{deviceList}",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Get, $"device_power/status?{deviceList}")
                    .ConfigureAwait(false);
                */
                KlipperDeviceStatusRespone? queryResult = GetObjectFromJson<KlipperDeviceStatusRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.DeviceStates ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
            IRestApiRequestRespone? result = null;
            Dictionary<string, string> resultObject = new();
            try
            {
                StringBuilder deviceList = new();
                for (int i = 0; i < devices.Length; i++)
                {
                    deviceList.Append(devices[i]);
                    if (i < devices.Length - 1)
                        deviceList.Append("&");
                }

                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: $"device_power/on?{deviceList}",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"device_power/on?{deviceList}")
                    .ConfigureAwait(false);
                */
                KlipperDeviceStatusRespone? queryResult = GetObjectFromJson<KlipperDeviceStatusRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.DeviceStates ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
            IRestApiRequestRespone? result = null;
            Dictionary<string, string> resultObject = [];
            try
            {
                StringBuilder deviceList = new();
                for (int i = 0; i < devices.Length; i++)
                {
                    deviceList.Append(devices[i]);
                    if (i < devices.Length - 1)
                        deviceList.Append("&");
                }

                string targetUri = $"{MoonrakerCommands.Machine}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: $"device_power/off?{deviceList}",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.machine, Method.Post, $"device_power/off?{deviceList}")
                    .ConfigureAwait(false);
                */
                KlipperDeviceStatusRespone? queryResult = GetObjectFromJson<KlipperDeviceStatusRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.DeviceStates ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public async Task<OctoprintApiVersionResult?> GetOctoPrintApiVersionInfoAsync()
        {
            IRestApiRequestRespone? result = null;
            OctoprintApiVersionResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Api}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"version",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"version")
                    .ConfigureAwait(false);
                */
                OctoprintApiVersionResult? queryResult = GetObjectFromJson<OctoprintApiVersionResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<OctoprintApiServerStatusResult?> GetOctoPrintApiServerStatusAsync()
        {
            IRestApiRequestRespone? result = null;
            OctoprintApiServerStatusResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Api}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"server",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"server")
                    .ConfigureAwait(false);
                */
                OctoprintApiServerStatusResult? queryResult = GetObjectFromJson<OctoprintApiServerStatusResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<OctoprintApiServerStatusResult?> GetOctoPrintApiUserInformationAsync()
        {
            IRestApiRequestRespone? result = null;
            OctoprintApiServerStatusResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Api}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"login",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"login")
                    .ConfigureAwait(false);
                */
                OctoprintApiServerStatusResult? queryResult = GetObjectFromJson<OctoprintApiServerStatusResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<OctoprintApiSettingsResult?> GetOctoPrintApiSettingsAsync()
        {
            IRestApiRequestRespone? result = null;
            OctoprintApiSettingsResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Api}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"settings",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"settings")
                    .ConfigureAwait(false);
                */
                OctoprintApiSettingsResult? queryResult = GetObjectFromJson<OctoprintApiSettingsResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<OctoprintApiJobResult?> GetOctoPrintApiJobStatusAsync()
        {
            IRestApiRequestRespone? result = null;
            OctoprintApiJobResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Api}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"job",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"job")
                    .ConfigureAwait(false);
                */
                OctoprintApiJobResult? queryResult = GetObjectFromJson<OctoprintApiJobResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public async Task<OctoprintApiPrinterStatusResult?> GetOctoPrintApiPrinterStatusAsync()
        {
            IRestApiRequestRespone? result = null;
            OctoprintApiPrinterStatusResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Api}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"printer",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"printer")
                    .ConfigureAwait(false);
                */
                OctoprintApiPrinterStatusResult? queryResult = GetObjectFromJson<OctoprintApiPrinterStatusResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
                string targetUri = $"{MoonrakerCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"printer/command",
                       jsonObject: cmd,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                KlipperApiRequestRespone result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Post, "printer/command", cmd)
                    .ConfigureAwait(false);
                */
                return GetQueryResult(result?.Result, true);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public Task<bool> SendOctoPrintApiGcodeCommandAsync(string command) => SendOctoPrintApiGcodeCommandAsync(new string[] { command });

        public async Task<Dictionary<string, OctoprintApiPrinter>> GetOctoPrintApiPrinterProfilesAsync()
        {
            IRestApiRequestRespone? result = null;
            Dictionary<string, OctoprintApiPrinter> resultObject = [];
            try
            {
                string targetUri = $"{MoonrakerCommands.Api}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"printerprofiles",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.api, Method.Get, $"printerprofiles")
                    .ConfigureAwait(false);
                */
                OctoprintApiPrinterProfilesResult? queryResult = GetObjectFromJson<OctoprintApiPrinterProfilesResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Profiles ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
            List<KlipperJobItem> resultObject = [];
            try
            {
                KlipperHistoryResult? result = await GetHistoryJobListResultAsync(limit, start, since, before, order).ConfigureAwait(false);
                return result?.Jobs ?? resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<KlipperHistoryResult?> GetHistoryJobListResultAsync(int limit = 100, int start = 0, double since = -1, double before = -1, string order = "asc")
        {
            IRestApiRequestRespone? result = null;
            KlipperHistoryResult? resultObject = null;
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

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"history/list",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"history/list", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperHistoryRespone? queryResult = GetObjectFromJson<KlipperHistoryRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public async Task<KlipperHistoryJobTotalsResult?> GetHistoryTotalJobsAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperHistoryJobTotalsResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"history/totals",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"history/totals")
                    .ConfigureAwait(false);
                */
                KlipperHistoryTotalRespone? queryResult = GetObjectFromJson<KlipperHistoryTotalRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result?.JobTotals;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public async Task<KlipperHistoryJobTotalsResult?> ResetHistoryTotalJobsAsync()
        {
            IRestApiRequestRespone? result = null;
            KlipperHistoryJobTotalsResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: $"history/reset_totals",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"history/reset_totals")
                    .ConfigureAwait(false);
                */
                KlipperHistoryTotalRespone? queryResult = GetObjectFromJson<KlipperHistoryTotalRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result?.JobTotals;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public async Task<KlipperJobItem?> GetHistoryJobAsync(string uid)
        {
            IRestApiRequestRespone? result = null;
            KlipperJobItem? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "uid", uid }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: $"history/job",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, $"history/job", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperHistorySingleJobRespone? queryResult = GetObjectFromJson<KlipperHistorySingleJobRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result?.Job;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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

        public Task<List<string>> DeleteHistoryJobAsync(KlipperJobItem item) => DeleteHistoryJobAsync(item?.JobId?.ToString());

        public async Task<List<string>> DeleteHistoryJobAsync(string? uid)
        {
            IRestApiRequestRespone? result = null;
            List<string> resultObject = [];
            try
            {
                if (string.IsNullOrEmpty(uid)) return resultObject;

                Dictionary<string, string> urlSegments = new()
                {
                    { "uid", uid }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: $"history/job",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"history/job", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperHistoryJobDeletedRespone? queryResult = GetObjectFromJson<KlipperHistoryJobDeletedRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result?.DeletedJobs ?? resultObject;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
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
        public override bool Equals(object? obj)
        {
            if (obj is not MoonrakerClient item)
                return false;
            return Id.Equals(item.Id);
        }
        public override int GetHashCode() => Id.GetHashCode();

        #endregion

    }
}
