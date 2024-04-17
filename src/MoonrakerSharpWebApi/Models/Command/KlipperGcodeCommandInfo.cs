using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperGcodeCommandInfo : BaseModel
    {
        #region Id
        [JsonProperty(nameof(Id))]
        Guid _id;
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
        #endregion

        #region Properties
        [JsonProperty(nameof(Command))]
        string _command = string.Empty;
        [JsonIgnore]
        public string Command
        {
            get => _command;
            set
            {
                if (_command == value) return;
                _command = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Sent))]
        bool _sent = false;
        [JsonIgnore]
        public bool Sent
        {
            get => _sent;
            set
            {
                if (_sent == value) return;
                _sent = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Succeeded))]
        bool _succeeded = false;
        [JsonIgnore]
        public bool Succeeded
        {
            get => _succeeded;
            set
            {
                if (_succeeded == value) return;
                _succeeded = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(TimeStamp))]
        DateTime _timeStamp = DateTime.Now;
        [JsonIgnore]
        public DateTime TimeStamp
        {
            get => _timeStamp;
            set
            {
                if (_timeStamp == value) return;
                _timeStamp = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Overrides

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
