using Microsoft.Extensions.Configuration;

namespace Bootcamp.Config
{
    public static class TestConfiguration
    {
        private const string AppSettingsFileName = "appsettings.json";
        private const string EnvironmentFileName = "env.json";
        private const string ConfigFolderName = "Config";
        private const string EnvironmentVariableKey = "ENVIRONMENT";        

        public static IConfigurationRoot GetConfig()
        {
            var builder = new ConfigurationBuilder();
            string projectDirectory = GetProjectDirectory();
            string configDirectory = Path.Combine(projectDirectory, ConfigFolderName);

            AddBaseConfigurationFiles(builder, configDirectory);
            AddEnvironmentSpecificConfigurationFiles(builder, configDirectory);

            return builder.Build();
        }

        private static string GetProjectDirectory()
        {
            var currentDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;

            if (projectDirectory == null)
            {
                throw new DirectoryNotFoundException("Project directory could not be determined.");
            }

            return projectDirectory;
        }

        private static void AddBaseConfigurationFiles(IConfigurationBuilder builder, string configDirectory)
        {
            builder.AddJsonFile(Path.Combine(configDirectory, AppSettingsFileName), optional: false, reloadOnChange: true)
                   .AddJsonFile(Path.Combine(configDirectory, EnvironmentFileName), optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();
        }

        private static void AddEnvironmentSpecificConfigurationFiles(IConfigurationBuilder builder, string configDirectory)
        {
            var tempConfig = builder.Build();
            var environment = tempConfig[EnvironmentVariableKey];

            if (string.IsNullOrEmpty(environment))
            {
                throw new InvalidOperationException($"Environment variable '{EnvironmentVariableKey}' is not set.");
            }

            var environmentSpecificFiles = Directory.GetFiles(configDirectory, $"appsettings.{environment}.json");

            foreach (var file in environmentSpecificFiles)
            {
                builder.AddJsonFile(file, optional: true, reloadOnChange: true);
            }
        }
    }
}

