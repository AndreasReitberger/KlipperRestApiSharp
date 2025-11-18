using AndreasReitberger.API.Moonraker.Models;
using System;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {

        #region EventHandlers

        #region WebSocket

        public event EventHandler<KlipperWebSocketConnectionChangedEventArgs>? WebSocketConnectionIdChanged;
        protected virtual void OnWebSocketConnectionIdChanged(KlipperWebSocketConnectionChangedEventArgs e)
        {
            WebSocketConnectionIdChanged?.Invoke(this, e);
        }
        #endregion

        #region State & Config
        public event EventHandler<KlipperStateChangedEventArgs>? KlipperStateChanged;
        protected virtual void OnKlipperStateChangedEvent(KlipperStateChangedEventArgs e)
        {
            KlipperStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperCpuTemperatureChangedEventArgs>? KlipperCpuTemperatureChanged;
        protected virtual void OnKlipperCpuTemperatureChanged(KlipperCpuTemperatureChangedEventArgs e)
        {
            KlipperCpuTemperatureChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperServerConfigChangedEventArgs>? KlipperServerConfigChanged;
        protected virtual void OnKlipperServerConfigChanged(KlipperServerConfigChangedEventArgs e)
        {
            KlipperServerConfigChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperTemperatureCacheChangedEventArgs>? KlipperServerTemperatureCacheChanged;
        protected virtual void OnKlipperServerTemperatureCacheChanged(KlipperTemperatureCacheChangedEventArgs e)
        {
            KlipperServerTemperatureCacheChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperGcodeCacheChangedEventArgs>? KlipperServerGcodeCacheChanged;
        protected virtual void OnKlipperServerGcodeCacheChanged(KlipperGcodeCacheChangedEventArgs e)
        {
            KlipperServerGcodeCacheChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperCpuUsageChangedEventArgs>? KlipperServerCpuUsageChanged;
        protected virtual void OnKlipperServerCpuUsageChanged(KlipperCpuUsageChangedEventArgs e)
        {
            KlipperServerCpuUsageChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperSystemMemoryChangedEventArgs>? KlipperServerSystemMemoryChanged;
        protected virtual void OnKlipperServerSystemMemoryChanged(KlipperSystemMemoryChangedEventArgs e)
        {
            KlipperServerSystemMemoryChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperPrinterInfoChangedEventArgs>? KlipperPrinterInfoChanged;
        protected virtual void OnKlipperPrinterInfoChanged(KlipperPrinterInfoChangedEventArgs e)
        {
            KlipperPrinterInfoChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperGcodeMetaResultChangedEventArgs>? KlipperGcodeMetaResultChanged;
        protected virtual void OnKlipperGcodeMetaResultChanged(KlipperGcodeMetaResultChangedEventArgs e)
        {
            KlipperGcodeMetaResultChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperGcodeMoveStateChangedEventArgs>? KlipperGcodeMoveStateChanged;
        protected virtual void OnKlipperGcodeMoveStateChanged(KlipperGcodeMoveStateChangedEventArgs e)
        {
            KlipperGcodeMoveStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperVirtualSdCardStateChangedEventArgs>? KlipperVirtualSdCardStateChanged;
        protected virtual void OnKlipperVirtualSdCardStateChanged(KlipperVirtualSdCardStateChangedEventArgs e)
        {
            KlipperVirtualSdCardStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperTemperatureSensorStatesChangedEventArgs>? KlipperTemperatureSensorStatesChanged;
        protected virtual void OnKlipperTemperatureSensorStatesChanged(KlipperTemperatureSensorStatesChangedEventArgs e)
        {
            KlipperTemperatureSensorStatesChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperPrintStateChangedEventArgs>? KlipperPrintStateChanged;
        protected virtual void OnKlipperPrintStateChanged(KlipperPrintStateChangedEventArgs e)
        {
            KlipperPrintStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperIsPrintingProgressChangedEventArgs>? KlipperIsPrintingProgressChanged;
        protected virtual void OnKlipperIsPrintingProgressChanged(KlipperIsPrintingProgressChangedEventArgs e)
        {
            KlipperIsPrintingProgressChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperFanStateChangedEventArgs>? KlipperFanStateChanged;
        protected virtual void OnKlipperFanStateChanged(KlipperFanStateChangedEventArgs e)
        {
            KlipperFanStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperMotionReportChangedEventArgs>? KlipperMotionReportChanged;
        protected virtual void OnKlipperMotionReportChanged(KlipperMotionReportChangedEventArgs e)
        {
            KlipperMotionReportChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperIdleStateChangedEventArgs>? KlipperIdleStateChanged;
        protected virtual void OnKlipperIdleStateChanged(KlipperIdleStateChangedEventArgs e)
        {
            KlipperIdleStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperToolHeadStateChangedEventArgs>? KlipperToolHeadStateChanged;
        protected virtual void OnKlipperToolHeadStateChanged(KlipperToolHeadStateChangedEventArgs e)
        {
            KlipperToolHeadStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperActiveJobStateChangedEventArgs>? KlipperActiveJobStateChanged;
        protected virtual void OnKlipperActiveJobStateChanged(KlipperActiveJobStateChangedEventArgs e)
        {
            KlipperActiveJobStateChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperDisplayStatusChangedEventArgs>? KlipperDisplayStatusChanged;
        protected virtual void OnKlipperDisplayStatusChanged(KlipperDisplayStatusChangedEventArgs e)
        {
            KlipperDisplayStatusChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperFSensorStateChangedEventArgs>? KlipperFSensorChanged;
        protected virtual void OnKlipperFSensorChanged(KlipperFSensorStateChangedEventArgs e)
        {
            KlipperFSensorChanged?.Invoke(this, e);
        }

        public event EventHandler<KlipperPresetsChangedEventArgs>? KlipperPresetsChanged;
        protected virtual void OnKlipperPresetsChanged(KlipperPresetsChangedEventArgs e)
        {
            KlipperPresetsChanged?.Invoke(this, e);
        }
        #endregion

        #region Jobs & Queue
        public event EventHandler<KlipperJobListStateChangedEventArgs>? KlipperJobListStateChanged;
        protected virtual void OnKlipperJobListStateChanged(KlipperJobListStateChangedEventArgs e)
        {
            KlipperJobListStateChanged?.Invoke(this, e);
        }

        #endregion

        #region Login
        public event EventHandler<KlipperLoginEventArgs>? LoginChanged;
        protected virtual void OnLoginChanged(KlipperLoginEventArgs e)
        {
            LoginChanged?.Invoke(this, e);
        }
        #endregion

        #endregion

    }
}
