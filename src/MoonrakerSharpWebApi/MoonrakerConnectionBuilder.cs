using AndreasReitberger.API.Moonraker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker
{

    public partial class MoonrakerClient
    {
        public class MoonrakerConnectionBuilder
        {
            #region Instance
            readonly MoonrakerClient _client = new();
            #endregion

            #region Methods

            public MoonrakerClient Build()
            {
                _client.Target = Print3dServer.Core.Enums.Print3dServerTarget.Moonraker;
                return _client;
            }

            public MoonrakerConnectionBuilder WithServerAddress(string serverAddress, string version = "")
            {
                _client.ApiTargetPath = serverAddress;
                _client.ApiVersion = version;
                return this;
            }

            public MoonrakerConnectionBuilder WithApiKey(string apiKey)
            {
                _client.ApiKey = apiKey;
                return this;
            }

            /*
            public async Task<MoonrakerConnectionBuilder> WithUserTokenAsync(string? userToken = null)
            {
                if (userToken is null)
                {
                    KlipperAccessTokenResult? tokenResult = await _client.GetOneshotTokenAsync();
                    userToken = tokenResult?.Result;
                }
                _client.OneShotToken = userToken ?? string.Empty;
                return this;
            }

            public async Task<MoonrakerConnectionBuilder> WitLoginAsync(string username, string password)
            {
                string apiToken = await _client.LoginUserForApiKeyAsync(username, password);
                _client.ApiKey = apiToken;
                await _client.LogoutCurrentUserAsync();
                return this;
            }
            */

            public MoonrakerConnectionBuilder WithName(string name)
            {
                _client.ServerName = name;
                return this;
            }

            public MoonrakerConnectionBuilder WithPingInterval(bool enablePing, int interval = 5)
            {
                _client.EnablePing = enablePing;
                _client.PingInterval = interval;
                return this;
            }

            public MoonrakerConnectionBuilder WithTimeout(int timeout = 100)
            {
                _client.DefaultTimeout = timeout;
                return this;
            }
            public MoonrakerConnectionBuilder WithWebSocket(string websocketTarget, string pingCommand = "", int pingInterval = 5, bool enablePing = true)
            {
                _client.WebSocketTargetUri = websocketTarget;
                _client.PingCommand = pingCommand;
                _client.PingInterval = pingInterval;
                _client.EnablePing = enablePing;
                return this;
            }

            #endregion
        }
    }
}
