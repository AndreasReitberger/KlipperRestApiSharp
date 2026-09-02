namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperCpuInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("cpu_count")]
        public partial long CpuCount { get; set; }

        [ObservableProperty]

        [JsonPropertyName("bits")]
        public partial string Bits { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("processor")]
        public partial string Processor { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("cpu_desc")]
        public partial string CpuDesc { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("serial_number")]
        public partial string SerialNumber { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("hardware_desc")]
        public partial string HardwareDesc { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("model")]
        public partial string Model { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("total_memory")]
        public partial long TotalMemory { get; set; }

        [ObservableProperty]

        [JsonPropertyName("memory_units")]
        public partial string MemoryUnits { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperCpuInfo);
        #endregion
    }
}
