using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using AV;

namespace Violet.AV
{
    internal static class FacilitySpawner
    {
        public static GameObject AetherVoidObject = null;

        public static void Patch()
        {
            ConsoleCommandsHandler.AddGotoTeleportPosition("pbp", new Vector3(-161f, -394f, 1186f));
            var info = PrefabInfo.WithTechType(
                "PrecursorFacility01",
                "Precursor Facility",
                "A mysterious ancient structure hidden beneath the waves."
            );

            var prefab = new CustomPrefab(info);

            var bundle = Plugin.ItemMeshBundle;
            if (bundle == null)
            {
                Plugin.Log.LogError($"Failed to load AssetBundle at {bundle}");
                return;
            }

            var FacilityObj = bundle.LoadAsset<GameObject>("precursorfacilitypolymer");
            if (FacilityObj == null)
            {
                Plugin.Log.LogError("Failed to find 'PrecursorFacilityPrefab' in AssetBundle.");
                return;
            }
            PrefabUtils.AddBasicComponents(
                FacilityObj,
                info.ClassID,
                info.TechType,
                LargeWorldEntity.CellLevel.Global
            );

            

                prefab.SetGameObject(FacilityObj);

            prefab.SetSpawns(new SpawnLocation(
                new Vector3(-158f, -385f, 1190f),
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 1f, 1f)
            ));

            ConsoleCommandsHandler.AddGotoTeleportPosition("PrecursorIonPolymerFacility", new Vector3(-161f, -394f, 1186f));

            

            prefab.Register();

            Plugin.Log.LogInfo("PrecursorFacility prefab registered and will spawn in world.");
        }

        public static void Aetherkeytunlocktrigger()
        {
            PDAHandler.AddLogEntry(
                    "IonPolymerfacilitywarning",
                    "IonPolymerfacilitywarning",
                   AV.pdavoicelinereg.AetherVoidBiomePDA.Ionpolymerwarningvoiceline
                );

            LanguageHandler.SetLanguageLine(
                    "IonPolymerfacilitywarning",
                    "<delay=0>Facility broadcast message received. <delay=500> Attempting Translation <delay=500><delay=500>Translation Reads<delay=500> <delay=500> Warning ion polymer testing facility has suffered internal damage due to underwater landslide oxygen field not effected<delay=500> <delay=500> Facility key detected with in 300 meters of facility<delay=500> <delay=500>Possible hostiles detected near facility key caution is advised.<delay=500> <delay=0>",
                    "English"
                );

            StoryGoalHandler.RegisterBiomeGoal(
                    "IonPolymerfacilitywarning",
                    Story.GoalType.PDA,
                    biomeName: "PrecursorIonPolymerFacility",
                    minStayDuration: 5f,
                    delay: 1f
                );
        }
    }
}
