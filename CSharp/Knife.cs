using ArchitectsLibrary.Handlers;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using AV;
using UnityEngine;


namespace Violet.AV
{

    public class CrystalKnifeClass
    {

        // All this code was possible thanks to Lee23
        // Lee23 Thank you so much for allowing modders to use code from your projects!

       // Most/Basically all Credits gose to Lee23!



        static readonly AssetBundle ItemMeshbundle = Plugin.ItemMeshBundle;
        public static AssetBundle bundle = null;
        public static bool Hasregistedknife = false;


        public static class CrystalKnife
        {
            static Sprite CrystalKnifeIcon = Plugin.TextureBundle.LoadAsset<Sprite>("CrystalKnifeIcon");
            public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("CrystalKnife").WithIcon(CrystalKnifeIcon);
            public static void Register()
            {
                

                var CrystalKnife = new CustomPrefab(Info);
                var CrystalKnifeTemplate = new CloneTemplate(Info, TechType.Knife);
                CrystalKnifeTemplate.ModifyPrefab += ModifyPrefab;
                CrystalKnife.SetGameObject(CrystalKnifeTemplate);
                CrystalKnife.SetEquipment(EquipmentType.Hand);
                CrystalKnife.SetRecipe(new RecipeData(new Ingredient(TechType.Knife, 1),
                        new Ingredient(Violet.AV.Items.IonPolymer, 1),
                        new Ingredient(AUHandler.ReinforcedGlassTechType, 2)))
                    .WithCraftingTime(8)
                    .WithFabricatorType(CraftTree.Type.Workbench);
                CrystalKnife.SetPdaGroupCategoryAfter(TechGroup.Personal, TechCategory.Tools, TechType.Knife);
                CrystalKnife.Register();
                Hasregistedknife = true;
            }

            private static void ModifyPrefab(GameObject prefab)
            {
                var renderer = prefab.GetComponentInChildren<Renderer>();
                Object.DestroyImmediate(renderer.gameObject.GetComponent<VFXFabricating>());
                renderer.enabled = false;

                

                var newModel = Object.Instantiate(Plugin.ItemMeshBundle.LoadAsset<GameObject>("AetherVoidknife"), prefab.transform);
                newModel.transform.localPosition = new Vector3(0.15f, -0.05f, 0f);
                newModel.transform.localEulerAngles = new Vector3(-85,0,0);
                MaterialUtils.ApplySNShaders(newModel);
                
                var handle = newModel.gameObject.transform.Find("Cube").gameObject;

                var Blade = newModel.gameObject.transform.Find("Icosphere").gameObject;
 
                var handlemeshrend = handle.GetComponent<MeshRenderer>();
                handlemeshrend.material = MaterialUtils.IonCubeMaterial;

                
                var skyApplier = prefab.GetComponentInChildren<SkyApplier>();
                skyApplier.renderers = prefab.GetComponentsInChildren<Renderer>();

                var oldKnifeComponent = prefab.GetComponent<Knife>();

                var newKnifeComponent = prefab.AddComponent<CrystalKnifeHandler.CrystalKnifeTool>();
                newKnifeComponent.attackSound = oldKnifeComponent.attackSound;
                newKnifeComponent.underwaterMissSound = oldKnifeComponent.underwaterMissSound;              
                newKnifeComponent.surfaceMissSound = oldKnifeComponent.surfaceMissSound;
                newKnifeComponent.damageType = oldKnifeComponent.damageType;
                newKnifeComponent.damage = 60;
                newKnifeComponent.crystaldamage = 60;
                newKnifeComponent.attackDist = 5;
                newKnifeComponent.vfxEventType = VFXEventTypes.knife;
                newKnifeComponent.mainCollider = oldKnifeComponent.mainCollider;
                newKnifeComponent.drawSound = oldKnifeComponent.drawSound;
                newKnifeComponent.firstUseSound = oldKnifeComponent.firstUseSound;
                newKnifeComponent.hitBleederSound = oldKnifeComponent.hitBleederSound;
                newKnifeComponent.bleederDamage = 999;
                newKnifeComponent.socket = oldKnifeComponent.socket;
                newKnifeComponent.ikAimRightArm = true;
                newKnifeComponent.drawTime = 0;
                newKnifeComponent.holsterTime = 0.1f;
                newKnifeComponent.pickupable = oldKnifeComponent.pickupable;
                newKnifeComponent.hasFirstUseAnimation = true;
                newKnifeComponent.hasBashAnimation = true;
                Object.DestroyImmediate(oldKnifeComponent);

                PrefabUtils.AddVFXFabricating(prefab, newModel.gameObject.name, -0.05f, 0.05f, default, 0.03f, Vector3.up * 90);
            }
        }
    }
}
