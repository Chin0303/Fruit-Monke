using System.Reflection;
using UnityEngine;

namespace FruitMonke;

public static class AssetLoader
{
    private const string ResourcePath = "FruitMonke.Resources.fruitbundle";

    private static AssetBundle CachedBundle;
    private static Dictionary<string, UnityEngine.Object> CachedAssets = new Dictionary<string, UnityEngine.Object>();

    // TODO: Change this to be async
    public static UnityEngine.Object LoadAsset(string name)
    {
        string id = ResourcePath + "_" + name;

        if (CachedAssets.TryGetValue(id, out UnityEngine.Object cachedAsset)) return cachedAsset;

        if (CachedBundle is not object)
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourcePath))
            {
                CachedBundle = AssetBundle.LoadFromStream(stream);
            }

        var asset = CachedBundle.LoadAsset(name);
        CachedAssets.Add(id, asset);
        return asset;
    }
}