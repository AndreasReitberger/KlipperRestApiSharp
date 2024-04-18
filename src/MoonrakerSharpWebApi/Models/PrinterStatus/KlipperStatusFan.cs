using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusFan : ObservableObject, IPrint3dFan
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        bool on; 
        
        [ObservableProperty, JsonIgnore]
        long? voltage;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(Speed))]
        [property: JsonProperty("speed")]
        double? fanSpeed = 0;
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

        [ObservableProperty, JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        [property: JsonProperty("rpm")]
        long? rpm = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        int? percent = 0;
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
