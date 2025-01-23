using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusFan : ObservableObject, IPrint3dFan
    {
        #region Properties
        [ObservableProperty]

        public partial bool On { get; set; }

        [ObservableProperty]

        public partial long? Voltage { get; set; }

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(Speed))]
        [JsonProperty("speed")]
        public partial double? FanSpeed { get; set; } = 0;

        partial void OnFanSpeedChanged(double? value)
        {
            if (value is not null)
                Percent = Convert.ToInt32(value * 100);
            else
                Percent = 0;
        }

        public int? Speed => Convert.ToInt32(Percent * 2.55f);
        /*
        [ObservableProperty, JsonIgnore]
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public int? speed = 0;
        partial void OnSpeedChanged(int? value)
        {
            if (value is not null)
                Percent = Convert.ToInt32(value * 100);
            else
                Percent = 0;
        }
        */

        [ObservableProperty]
        [field: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        [JsonProperty("rpm")]
        public partial long? Rpm { get; set; } = 0;

        [ObservableProperty]

        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        public partial int? Percent { get; set; } = 0;

        partial void OnPercentChanged(int? value)
        {
            On = value > 0;
        }
        #endregion

        #region Methods
        public Task<bool> SetFanSpeedAsync(IPrint3dServerClient client, string command, object? data) => client.SetFanSpeedAsync(command, data);
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion
    }
}
