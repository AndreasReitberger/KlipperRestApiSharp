﻿using AndreasReitberger.API.Moonraker.Models;
using System;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {

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

        public event EventHandler<KlipperEventArgs> WebSocketMessageReceived;
        protected virtual void OnWebSocketMessageReceived(KlipperEventArgs e)
        {
            WebSocketMessageReceived?.Invoke(this, e);
        }

        public event EventHandler<KlipperWebSocketDataEventArgs> WebSocketDataReceived;
        protected virtual void OnWebSocketDataReceived(KlipperWebSocketDataEventArgs e)
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
        protected virtual void OnKlipperStateChangedEvent(KlipperStateChangedEventArgs e)
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

        public event EventHandler<KlipperGcodeCacheChangedEventArgs> KlipperServerGcodeCacheChanged;
        protected virtual void OnKlipperServerGcodeCacheChanged(KlipperGcodeCacheChangedEventArgs e)
        {
            KlipperServerGcodeCacheChanged?.Invoke(this, e);
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

        public event EventHandler<KlipperIsPrintingProgressChangedEventArgs> KlipperIsPrintingProgressChanged;
        protected virtual void OnKlipperIsPrintingProgressChanged(KlipperIsPrintingProgressChangedEventArgs e)
        {
            KlipperIsPrintingProgressChanged?.Invoke(this, e);
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

        public event EventHandler<KlipperCurrentPrintImageChangedEventArgs> KlipperCurrentPrintImageChanged;
        protected virtual void OnKlipperCurrentPrintImageChanged(KlipperCurrentPrintImageChangedEventArgs e)
        {
            KlipperCurrentPrintImageChanged?.Invoke(this, e);
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

    }
}