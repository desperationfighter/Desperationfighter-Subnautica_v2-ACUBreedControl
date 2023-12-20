using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace ACUBreedControl.Management
{
    [Menu("ACU Breed Control Settings", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Toggle("Breeding Active", Tooltip = "Enable or Disable Breeding in general", Order = 1)]
        public bool Config_BreedActive = true;

        [Slider("Breeding Chance", 1, 100, DefaultValue = 100, Tooltip = "On every Breeding instance you can configure how successfully it is. (100 is Game Default)", Order = 2)]
        public int Config_BreedChance = 100;
    }
}