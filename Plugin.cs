using System.IO;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ElectroMenu;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("TootTallyMultiplayer", BepInDependency.DependencyFlags.HardDependency)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance;

    private void Awake()
    {
        Instance = this;
        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();
    }

    [HarmonyPatch(typeof(HomeController), nameof(HomeController.Start))]
    public static class HomeControllerStartPatches
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix()
        {
            OverwriteImage("MULTIContainer", "MultiplayerButtonElectro.png");
            OverwriteImage("COLLECTcontainer", "CollectButtonElectro.png");
            OverwriteImage("IMPROVcontainer", "ImprovButtonElectro.png");
        }
    }

    private static void OverwriteImage(string containerName, string imageName)
    {
        var container = GameObject.Find($"MainCanvas/MainMenu/{containerName}");
        var gameObject = container.transform.Find("FG").gameObject;
        var component = gameObject.GetComponent<Image>();
        component.useSpriteMesh = true;
        component.color = Color.white;

        var bytes = File.ReadAllBytes($"{Path.GetDirectoryName(Plugin.Instance.Info.Location)}/Assets/{imageName}");
        var texture = new Texture2D(0, 0);
        texture.LoadImage(bytes);
        component.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 300f);
    }
}
