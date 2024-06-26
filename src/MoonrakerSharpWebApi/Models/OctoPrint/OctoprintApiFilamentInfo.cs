﻿using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiFilamentInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("length", NullValueHandling = NullValueHandling.Ignore)]
        double length;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        double volume;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
