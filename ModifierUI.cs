using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using Worms.Configuration;

namespace Worms
{
    class ModifierUI : NotifiableSingleton<ModifierUI>
    {
        [UIValue("enabled")]
        public bool Enabled
        {
            get => PluginConfig.Instance.enabled;
            set
            {
                PluginConfig.Instance.enabled = value;
            }
        }
        [UIAction("set_enabled")]
        void Set_Enabled(bool value)
        {
            Enabled = value;
        }


        [UIComponent("links_slider")]
        public SliderSetting Links_Slider;

        [UIValue("links_value")]
        public float Links_Value
        {
            get => PluginConfig.Instance.links_value;
            set
            {
                PluginConfig.Instance.links_value = value;
            }
        }
        [UIAction("set_links_value")]
        public void Set_Links_Value(float value)
        {
            Links_Value = value;
        }


        [UIComponent("squish_slider")]
        public SliderSetting Squish_Slider;

        [UIValue("squish_value")]
        public float Squish_Value
        {
            get => PluginConfig.Instance.squish_value;
            set
            {
                PluginConfig.Instance.squish_value = value;
            }
        }
        [UIAction("set_squish_value")]
        public void Set_Squish_Value(float value)
        {
            Squish_Value = value;
        }
    }
}
