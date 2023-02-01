using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.Moonraker.Utilities
{
    public partial class DatabaseHandler
    {
        #region Methods

        #region Public

        #region Clients
        public async Task<List<MoonrakerClient>> GetClientsWithChildrenAsync()
        {
            // To trigger event
            Clients = await DatabaseAsync
                .GetAllWithChildrenAsync<MoonrakerClient>(recursive: true)
                ;
            // Ensures to recalculate some data
            Clients.ForEach(depot => depot?.CheckOnlineAsync());
            return Clients;
        }

        public async Task<MoonrakerClient> GetClientWithChildrenAsync(Guid id)
        {
            MoonrakerClient client = await DatabaseAsync
                .GetWithChildrenAsync<MoonrakerClient>(id, recursive: true)
                ;
            client.CheckOnlineAsync();
            return client;
        }

        public async Task SetClientWithChildrenAsync(MoonrakerClient client)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(client, recursive: true)
                ;
        }

        public async Task SetClientsWithChildrenAsync(List<MoonrakerClient> clients, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(clients);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(clients);
        }

        public async Task SetClientsWithChildrenAsync(ObservableCollection<MoonrakerClient> clients, bool replaceExisting = true)
        {
            await SetClientsWithChildrenAsync(clients.ToList(), replaceExisting);
        }

        public async Task<int> DeleteClientAsync(MoonrakerClient client)
        {
            return await DatabaseAsync.DeleteAsync<MoonrakerClient>(client?.Id);
        }

        #endregion

        #endregion

        #endregion
    }
}
