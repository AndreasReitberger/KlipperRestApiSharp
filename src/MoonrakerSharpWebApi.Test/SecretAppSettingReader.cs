using Microsoft.Extensions.Configuration;

namespace MoonrakerSharpWebApi.Test
{
    public class SecretAppSettingReader
    {
        // Source: https://www.programmingwithwolfgang.com/use-net-secrets-in-console-application/
        public static T ReadSection<T>(string sectionName)
        {
        // This only works on Windows, otherwise the secret.json file is not found
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddUserSecrets<Tests>()
        ;
        IConfigurationRoot configurationRoot = builder.Build();          
        return configurationRoot.GetSection(sectionName).Get<T>();
        }
    }
    
}
