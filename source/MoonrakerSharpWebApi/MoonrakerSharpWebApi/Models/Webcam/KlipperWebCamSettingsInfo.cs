using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperWebCamSettingsInfo : BaseModel
    {
        #region Properties
        [JsonProperty(nameof(Id))]
        Guid _id = Guid.Empty;
        [JsonIgnore]
        public Guid Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsDefault))]
        bool _isDefault = false;
        [JsonIgnore]
        public bool IsDefault
        {
            get => _isDefault;
            set
            {
                if (_isDefault == value) return;
                _isDefault = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Autostart))]
        bool _autostart = false;
        [JsonIgnore]
        public bool Autostart
        {
            get => _autostart;
            set
            {
                if (_autostart == value) return;
                _autostart = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Name))]
        string _name = string.Empty;
        [JsonIgnore]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ServerId))]
        Guid _serverId = Guid.Empty;
        [JsonIgnore]
        public Guid ServerId
        {
            get => _serverId;
            set
            {
                if (_serverId == value) return;
                _serverId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(CamIndex))]
        int _camIndex = -1;
        [JsonIgnore]
        public int CamIndex
        {
            get => _camIndex;
            set
            {
                if (_camIndex == value) return;
                _camIndex = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(RotationAngle))]
        int _rotationAngle = 0;
        [JsonIgnore]
        public int RotationAngle
        {
            get => _rotationAngle;
            set
            {
                if (_rotationAngle == value) return;
                _rotationAngle = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(NetworkBufferTime))]
        int _networkBufferTime = 150;
        [JsonIgnore]
        public int NetworkBufferTime
        {
            get => _networkBufferTime;
            set
            {
                if (_networkBufferTime == value) return;
                _networkBufferTime = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(FileCachingTime))]
        int _fileCachingTime = 1000;
        [JsonIgnore]
        public int FileCachingTime
        {
            get => _fileCachingTime;
            set
            {
                if (_fileCachingTime == value) return;
                _fileCachingTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
