using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace AndreasReitberger.API.Moonraker.Extensions
{
    [Table(nameof(MoonrakerClient) + "s")]
    public partial class MoonrakerSqliteClient : MoonrakerClient
    {
        [ObservableProperty]
        [property: PrimaryKey]
        Guid id = Guid.Empty;
    }
}