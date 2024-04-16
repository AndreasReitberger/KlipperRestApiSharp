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
        bool on; 
        
        [ObservableProperty]
        long? voltage;

        [ObservableProperty]
        [JsonProperty("speed")]
        [property: JsonIgnore]
        public int? speed = 0;
        partial void OnSpeedChanged(int? value)
        {
            if (value is not null)
                Percent = Convert.ToInt32(value * 100);
            else
                Percent = 0;
        }

        [ObservableProperty]
        [JsonProperty("rpm")]
        [property: JsonIgnore]
        long? rpm = 0;

        [ObservableProperty]
        [JsonIgnore]
        int? percent = 0;
        //public int Percent => GetPercentageSpeed();
        #endregion

        #region Methods
        public Task<bool> SetFanSpeedAsync(IPrint3dServerClient client, string command, object data) => client?.SetFanSpeedAsync(command, data);
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
