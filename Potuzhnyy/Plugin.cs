using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using LethalLib.Modules;
using Potuzhnyy.Patches;
using UnityEngine;

namespace Potuzhnyy
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            
            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(StartOfRoundPatch));

            RegisterItems();
        }

        private AssetBundle bundle;
        private void RegisterItems()
        {
            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "itemmod");
            bundle = AssetBundle.LoadFromFile(assetDir);

            RegisterItem("Assets/MaxonPillowItem.asset", 15);
            RegisterItem("Assets/ValikPillowItem.asset", 20);
        }

        private void RegisterItem(string path, int rarity)
        {
            Item item = bundle.LoadAsset<Item>(path);

            NetworkPrefabs.RegisterNetworkPrefab(item.spawnPrefab);
            Utilities.FixMixerGroups(item.spawnPrefab);
            Items.RegisterScrap(item, rarity, Levels.LevelTypes.All);

            Logger.LogInfo(path + " registered");
        }
    }
}