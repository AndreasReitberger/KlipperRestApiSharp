using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusFilamentSensor : ObservableObject, ISensorComponent
    {
        #region Properties
        [ObservableProperty]
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public partial bool Triggered { get; set; } = false;

        [ObservableProperty]
        
        [NotifyPropertyChangedFor(nameof(Triggered))]
        [JsonProperty("filament_detected")]
        public partial bool FilamentDetected { get; set; }

        partial void OnFilamentDetectedChanged(bool value)
        {
            Triggered = value;
        }

        [ObservableProperty]
        public partial bool Enabled { get; set; }

        [ObservableProperty]
        public partial Printer3dSensorType Type { get; set; } = Printer3dSensorType.Filament;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
