using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using AV;
using Violet.AV.MaterialUtilitys;

namespace Violet.AV
{
    internal static class AetherVoidSpawner
    {
        public static GameObject AetherVoidObject = null;

        public static void PatchAetherVoid()
        {
            var info = PrefabInfo.WithTechType(
                "AetherVoid",
                "Aether Void",
                null
            );

            var prefab = new CustomPrefab(info);

            var bundle = Plugin.ItemMeshBundle;
            if (bundle == null)
            {
                Plugin.Log.LogError($"Failed to load AssetBundle at {bundle}");
                return;
            }

            var AetherVoidObj = bundle.LoadAsset<GameObject>("AetherVoidUVFix1");
            if (AetherVoidObj == null)
            {
                Plugin.Log.LogError("Failed to find 'AetherVoidPrefab' in AssetBundle.");
                return;
            }
            PrefabUtils.AddBasicComponents(
                AetherVoidObj,
                info.ClassID,
                info.TechType,
                LargeWorldEntity.CellLevel.Global
            );

            foreach (Transform child in AetherVoidObj.transform)
            {
                GameObject gameobject = child.gameObject;
                if (gameobject.name.ToString().Contains("CrystalBlue"))
                {
                    ApplyCrystalMaterial.ApplyMaterialToObject(gameobject, 1);
                }
                else if (gameobject.name.ToString().Contains("CrystalPurple"))
                {
                    ApplyCrystalMaterial.ApplyMaterialToObject(gameobject, 0);
                }
                else if (gameobject.name.ToString().Contains("CrystalCyan"))
                {
                    ApplyCrystalMaterial.ApplyMaterialToObject(gameobject, 2);
                }

            }

            prefab.SetGameObject(AetherVoidObj);

            prefab.SetSpawns(new SpawnLocation(
                new Vector3(1700.7f, -1655.9f, -1675.7f),
                new Vector3(0f, 0f, 0f),
                new Vector3(215f, 215f, 215f)
            ));


            prefab.Register();

            Plugin.Log.LogInfo("AetherVoid registered and will spawn in world.");
        }
    }
}
