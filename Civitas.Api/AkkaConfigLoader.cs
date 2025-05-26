namespace Civitas.Api;

using Akka.Configuration;

/// <summary>
/// AkkaConfigLoader is a class that loads the Akka.NET configuration from a specified file.
/// </summary>
public class AkkaConfigLoader
{
    /// <summary>
    /// Loads the Akka.NET configuration from a file.
    /// </summary>
    /// <param name="fileName">Name of the configuration file. Default is "akka.conf".</param>
    /// <returns>Returns the loaded configuration.</returns>
    public Config LoadConfig(string fileName = "akka.conf")
    {
        var hocon = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, fileName));
        return ConfigurationFactory.ParseString(hocon);
    }
}