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
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperature")]
        public double? tempRead;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("target")]
        public double? tempSet;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("power")]
        public double? power;

        [JsonIgnore]
        public bool CanUpdateTarget = false;

        [JsonIgnore]
        public Printer3dToolHeadState State { get => GetCurrentState(); }

        #region JsonIgnore
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long error;

        [ObservableProperty, JsonIgnore]
        Printer3dHeaterType type = Printer3dHeaterType.HeatedBed;
        #endregion

        #endregion

        #region Methods
        public Printer3dToolHeadState GetCurrentState()
        {
            try
            {
                if (TempSet == null || TempRead == null)
                    return Printer3dToolHeadState.Idle;

                return TempSet <= 0
                    ? Printer3dToolHeadState.Idle
                    : TempSet > TempRead && Math.Abs(Convert.ToDouble(TempSet - TempRead)) > 2 ? Printer3dToolHeadState.Heating : Printer3dToolHeadState.Ready;
            }
            catch (Exception)
            {
                return Printer3dToolHeadState.Error;
            }

        }
        public Task<bool> SetTemperatureAsync(IPrint3dServerClient client, string command, object? data) => client.SetBedTemperatureAsync(command, data);
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
