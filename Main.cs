using BepInEx;
using BepInEx.Logging;
using Utilla;

namespace FruitMonke;

[BepInPlugin("com.Chin.gorillatag.FruitMonke", "FruitMonke", "2.0.0"), BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
[ModdedGamemode("FruitMonke", "FruitMonke", typeof(Behaviors.GameModeManager))]

public class Main : BaseUnityPlugin
{
    private static Main Instance;

    public Main()
    {
        if (Instance is object)
        {
            Log("Attempting to create multiple instances", LogLevel.Warning);
            return;
        }
        Instance = this;
    }

    public static void Log(object data, LogLevel logLevel = LogLevel.Info)
    {
        if (Instance is object)
        {
            Instance.Logger.Log(logLevel, data);
            return;
        }
        UnityEngine.Debug.Log("[Fruit Monke]: " + data);
    }
}
