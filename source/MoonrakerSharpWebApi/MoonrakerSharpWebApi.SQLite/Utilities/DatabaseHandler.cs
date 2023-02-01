using AndreasReitberger.API.Moonraker.Models.Events;
using AndreasReitberger.Core.Utilities;
using SQLite;
using System.Diagnostics;

namespace AndreasReitberger.API.Moonraker.Utilities
{
    public partial class DatabaseHandler : BaseModel//, IDatabaseHandler
    {
        #region Instance
        static DatabaseHandler? _instance;
        static readonly object Lock = new();
        public static DatabaseHandler? Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseHandler();
                    }
                }
                return _instance;
            }

            set
            {
                if (_instance == value) return;
                lock (Lock)
                {
                    _instance = value;
                }
            }

        }
        #endregion

        #region Properties
        bool _isInitialized = false;
        public bool IsInitialized
        {
            get => _isInitialized;
            private set
            {
                if (_isInitialized == value) return;
                _isInitialized = value;
                OnPropertyChanged();
            }
        }

        string _databasePath = "";
        public string DatabasePath
        {
            get => _databasePath;
            private set
            {
                if (_databasePath == value) return;
                _databasePath = value;
                OnPropertyChanged();
            }
        }

        SQLiteConnection _database;
        public SQLiteConnection Database
        {
            get => _database;
            private set
            {
                if (_database == value) return;
                _database = value;
                OnPropertyChanged();
            }
        }

        SQLiteAsyncConnection _databaseAsync;
        public SQLiteAsyncConnection DatabaseAsync
        {
            get => _databaseAsync;
            private set
            {
                if (_databaseAsync == value) return;
                _databaseAsync = value;
                OnPropertyChanged();
            }
        }

        List<Action> _delegates = new();
        public List<Action> Delegates
        {
            get => _delegates;
            private set
            {
                if (_delegates == value) return;
                _delegates = value;
                OnPropertyChanged();
            }
        }

        List<Type> _tables = new();
        public List<Type> Tables
        {
            get => _tables;
            private set
            {
                if (_tables == value) return;
                _tables = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Collections
        List<MoonrakerClient> _clients = new();
        public List<MoonrakerClient> Clients
        {
            get => _clients;
            private set
            {
                if (_clients == value) return;
                _clients = value;
                OnPropertyChanged();
                OnClientsChanged(new ClientsChangedDatabaseEventArgs()
                {
                    Clients = value,
                });
            }
        }
        #endregion

        #region Constructor
        public DatabaseHandler()
        {

        }
        public DatabaseHandler(string databasePath, bool updateInstance = true)
        {
            DatabasePath = databasePath;
            DatabaseAsync = new SQLiteAsyncConnection(databasePath);
            Database = new SQLiteConnection(databasePath);
            InitTables();
            IsInitialized = true;
            if (updateInstance) Instance = this;
        }
        #endregion

        #region Methods

        #region Private

        #endregion

        #region Public

        #region Init
        public void InitTables()
        {
            Database?.CreateTable<MoonrakerClient>();
            //Database?.CreateTable<DatabaseSettingsKeyValuePair>();
        }

        public async Task InitTablesAsync()
        {
            await DatabaseAsync.CreateTableAsync<MoonrakerClient>();
            //await DatabaseAsync?.CreateTableAsync<DatabaseSettingsKeyValuePair>();
        }

        public void CreateTable(Type table)
        {
            Database?.CreateTable(table);
        }

        public void CreateTables(List<Type> tables)
        {
            Database?.CreateTables(CreateFlags.None, tables?.ToArray());
        }

        #endregion

        #region Delegates
        public async Task UpdateAllDelegatesAsync()
        {
            //var actions = Delegates.Select(task => new Task(task));
            List<Task> tasks = new(Delegates.Select(task => new Task(task)));
            await Task.WhenAll(tasks);
        }
        #endregion

        #region Database
        public void InitDatabase(string databasePath)
        {
            DatabaseAsync = new SQLiteAsyncConnection(databasePath);
            Database = new SQLiteConnection(databasePath);
            InitTables();
            IsInitialized = true;
            Instance = this;
        }

        public async Task InitDatabaseAsync(string databasePath)
        {
            DatabaseAsync = new SQLiteAsyncConnection(databasePath);
            Database = new SQLiteConnection(databasePath);
            await InitTablesAsync();
            IsInitialized = true;
            Instance = this;
        }

        public async Task CloseDatabaseAsync()
        {
            Database?.Close();
            await DatabaseAsync?.CloseAsync();
        }

        public List<TableMapping> GetTableMappings(string databasePath = "")
        {
            if (DatabaseAsync == null && !string.IsNullOrWhiteSpace(databasePath))
            {
                InitDatabase(databasePath);
            }
            return DatabaseAsync?.TableMappings.ToList();
        }

        public async Task RebuildAllTableAsync()
        {
            await InitTablesAsync();
        }

        public async Task DropAllTableAsync()
        {
            //List<Task> tasks = new();
            foreach (TableMapping mapping in DatabaseAsync.TableMappings)
            {
                await DatabaseAsync?.DropTableAsync(mapping);
                //tasks.Add(Database?.DeleteAllAsync(mapping));
            }
            //await Task.WhenAll(tasks);
        }

        public async Task TryDropAllTableAsync()
        {
            foreach (TableMapping mapping in DatabaseAsync.TableMappings)
            {
                try
                {
                    await DatabaseAsync?.DropTableAsync(mapping);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public async Task<bool> TryDropTableAsync(TableMapping mapping)
        {
            try
            {
                int result = await DatabaseAsync?.DropTableAsync(mapping);
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task ClearAllTableAsync()
        {
            foreach (TableMapping mapping in DatabaseAsync.TableMappings)
            {
                await DatabaseAsync?.DeleteAllAsync(mapping);
            }
        }

        public async Task ClearTableAsync(TableMapping mapping)
        {
            await DatabaseAsync?.DeleteAllAsync(mapping);
        }

        public async Task TryClearAllTableAsync()
        {
            foreach (TableMapping mapping in DatabaseAsync.TableMappings)
            {
                try
                {
                    await DatabaseAsync?.DeleteAllAsync(mapping);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public async Task BackupDatabaseAsync(string targetFolder, string databaseName)
        {
            await DatabaseAsync?.BackupAsync(targetFolder, databaseName);
        }

        public void BackupDatabase(string targetFolder, string databaseName)
        {
            Database?.Backup(targetFolder, databaseName);
        }

        public void Close()
        {
            Database?.Close();
            DatabaseAsync?.CloseAsync();
        }

        public void Dispose()
        {
            Close();
        }
        #endregion

        #region Static

        public static async Task<Tuple<T, TimeSpan?>> StopWatchFunctionAsync<T>(Func<T> action, bool inNewTask = false)
        {
            Stopwatch timer = new();
            timer.Start();
            T result;
            if (inNewTask)
            {
                result = await Task.Run(() =>
                {
                    return action();
                });
            }
            else
            {
                result = action();
            }
            timer.Stop();
            //var t = new Tuple<T, TimeSpan?>(result, timer?.Elapsed);
            return new Tuple<T, TimeSpan?>(result, timer?.Elapsed);
        }

        public static T StopWatchFunction<T>(Func<T> action, out TimeSpan? duration)
        {
            Stopwatch timer = new();
            timer.Start();
            T result = action();

            timer.Stop();
            duration = timer?.Elapsed;

            return result;
        }

        #endregion

        #endregion

        #endregion
    }
}
