using BepInEx.Configuration;
using BepInEx;
using System.IO;

public static class jetpackConfig
{
    public static ConfigFile ConfigFile { get; private set; }

    public static ConfigEntry<int> Speed;

    public static void Initialize()
    {
        ConfigFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "MonkeJetpack.cfg"), true);

        Speed = ConfigFile.Bind("Configuration", "Jetpack Speed", 1, "This will determine how fast the jetpack will go.");
    }
}