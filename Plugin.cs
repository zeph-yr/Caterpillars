using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using System;
using System.Linq;
using System.Reflection;
using Worms.Configuration;
using IPALogger = IPA.Logging.Logger;

namespace Worms
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        public const string HarmonyId = "com.zephyr.BeatSaber.Worms";
        internal static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(HarmonyId);


        [Init]
        public Plugin(IPALogger logger, Config config)
        {
            Instance = this;
            Plugin.Log = logger;
            Plugin.Log?.Debug("Logger initialized.");

            PluginConfig.Instance = config.Generated<PluginConfig>();
        }

        [OnEnable]
        public void OnEnable()
        {
            ApplyHarmonyPatches();
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("GummyWorms", "Worms.ModifierUI.bsml", ModifierUI.instance);

            if (CheckForSS() == true)
            {
                BS_Utils.Utilities.BSEvents.gameSceneLoaded += BSEvents_gameSceneLoaded;
            }
        }

        private void BSEvents_gameSceneLoaded()
        {
            if (PluginConfig.Instance.enabled)
            {
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("Worms");
            }
        }


        [OnDisable]
        public void OnDisable()
        {
            RemoveHarmonyPatches();
            BS_Utils.Utilities.BSEvents.gameSceneLoaded -= BSEvents_gameSceneLoaded;
        }

        internal static void ApplyHarmonyPatches()
        {
            try
            {
                Plugin.Log?.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error applying Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            try
            {
                harmony.UnpatchSelf();
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error removing Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }

        internal static bool CheckForSS()
        {
            var ss_installed = PluginManager.GetPluginFromId("ScoreSaber");
            return ss_installed != null;
        }
    }
}
