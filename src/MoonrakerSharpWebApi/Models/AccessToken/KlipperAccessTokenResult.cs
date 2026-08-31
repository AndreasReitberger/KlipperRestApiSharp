namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperAccessTokenResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Result { get; set; } = string.Empty;
        #endregion

        #region Overrides

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
