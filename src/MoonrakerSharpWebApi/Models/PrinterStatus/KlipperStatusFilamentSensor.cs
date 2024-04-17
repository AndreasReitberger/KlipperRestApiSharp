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
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        Guid id;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        bool triggered = false;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(Triggered))]
        [property: JsonProperty("filament_detected")]
        bool filamentDetected;
        partial void OnFilamentDetectedChanged(bool value)
        {
            Triggered = value;
        }

        [ObservableProperty]
        bool enabled;

        [ObservableProperty]
        Printer3dSensorType type = Printer3dSensorType.Filament;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
