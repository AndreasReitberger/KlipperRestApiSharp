namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperProfile : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperProfile);
        #endregion
    }
}
