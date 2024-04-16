using System;
using System.Collections.Generic;
using System.Text;

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

            public MoonrakerConnectionBuilder WithServerAddress(string serverAddress, int port = 80, bool https = false)
            {
                _client.IsSecure = https;
                _client.ServerAddress = serverAddress;
                _client.Port = port;
                return this;
            }

            public MoonrakerConnectionBuilder WithApiKey(string apiKey)
            {
                _client.ApiKey = apiKey;
                return this;
            }

            public MoonrakerConnectionBuilder WithName(string name)
            {
                _client.ServerName = name;
                return this;
            }

            #endregion
        }
    }
}
