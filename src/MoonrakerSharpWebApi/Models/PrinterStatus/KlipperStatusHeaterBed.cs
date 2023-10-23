using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusHeaterBed : ObservableObject, IHeaterComponent
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [JsonProperty("temperature")]
        public double? TempRead { get; set; }

        [JsonProperty("target")]
        public double? TempSet { get; set; }

        [JsonProperty("power")]
        public double? Power { get; set; }

        [JsonIgnore]
        public bool CanUpdateTarget { get; set; } = false;

        [JsonIgnore]
        public KlipperToolState State { get => GetCurrentState(); }

        #region JsonIgnore
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string name;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long error;

        [ObservableProperty]
        Printer3dHeaterType type = Printer3dHeaterType.HeatedBed;
        #endregion

        #endregion

        #region Methods
        KlipperToolState GetCurrentState()
        {
            try
            {
                if (TempSet == null || TempRead == null)
                    return KlipperToolState.Idle;

                return TempSet <= 0
                    ? KlipperToolState.Idle
                    : TempSet > TempRead && Math.Abs(Convert.ToDouble(TempSet - TempRead)) > 2 ? KlipperToolState.Heating : KlipperToolState.Ready;
            }
            catch (Exception)
            {
                return KlipperToolState.Error;
            }

        }
        public Task<bool> SetTemperatureAsync(IPrint3dServerClient client, string command, object data) => client?.SetBedTemperatureAsync(command, data);
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
