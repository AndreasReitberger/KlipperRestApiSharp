using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace AndreasReitberger.API.Moonraker.Extensions
{
    /// <summary>
    /// This is a prepartion to make the client workable with SQlite, however not yet ready.
    /// </summary>
    [Table(nameof(MoonrakerClient) + "s")]
    public partial class MoonrakerSqliteClient : MoonrakerClient
    {
        [ObservableProperty]
        [property: PrimaryKey]
        Guid id = Guid.Empty;
    }
}