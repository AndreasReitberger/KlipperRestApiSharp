using AndreasReitberger.API.Moonraker.Models.Events;

namespace AndreasReitberger.API.Moonraker.Utilities
{
    public partial class DatabaseHandler
    {
        #region Events
        public event EventHandler<DatabaseEventArgs> DataChanged;
        protected virtual void OnDataChanged(DatabaseEventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }

        public event EventHandler<DatabaseEventArgs> QueryFinished;
        protected virtual void OnQueryFinished(DatabaseEventArgs e)
        {
            QueryFinished?.Invoke(this, e);
        }

        public event EventHandler<ClientsChangedDatabaseEventArgs> ClientsChanged;
        protected virtual void OnClientsChanged(ClientsChangedDatabaseEventArgs e)
        {
            ClientsChanged?.Invoke(this, e);
        }

        #endregion
    }
}
