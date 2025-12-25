using ArchitectsLibrary.Handlers;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.FMod;
using Nautilus.FMod.Interfaces;
using Nautilus.Handlers;
using Nautilus.Utility;
using Story;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using static GameObjectPoolPrefabMap;
using static UWE.FreezeTime;
using Plugin = AV.Plugin;

namespace Violet.AV
{
    public static class Items
    {
        public static TechType IonPolymer;
        public static TechType Cloudnite;
        public static PrefabInfo prefabInfo { get; private set; }

        public static void Registeritem1()
        {
            AssetBundle bundle = Plugin.ItemMeshBundle;

            // Texture/Spites
            Sprite Cloudniteoreicon = Plugin.TextureBundle.LoadAsset<Sprite>("Cloud Nite Ore Icon");
            Sprite Swiftericon = Plugin.TextureBundle.LoadAsset<Sprite>("Swifter Icon");
            Sprite Cloudniteoredatapop = Plugin.TextureBundle.LoadAsset<Sprite>("databank-popup-Cloudnite");
            Texture2D Cloudniteoredatabank = Plugin.TextureBundle.LoadAsset<Texture2D>("databank-image-Cloudnite");
            var bundleSource = new AssetBundleSoundSource(Plugin.AudioBundle);
            FModSoundBuilder builder = new FModSoundBuilder(bundleSource);

            IFModSoundBuilder Swiftercloudnitebuilder = builder.CreateNewEvent(
                    "SwifterCloudnite",
                    "bus:/master/SFX_for_pause"
                );

            Swiftercloudnitebuilder.SetSound("SwifterCloudnite")
                .SetFadeDuration(0.5f)
                .SetMode2D(false);


            Swiftercloudnitebuilder.Register();

            var fmodAssetSwiftercloudnite = Nautilus.Utility.AudioUtils.GetFmodAsset("SwifterCloudnite");


            if (bundle == null)
            {
                Plugin.Log.LogError("ItemMeshBundle is not loaded!");
                return;
            }

            var info = PrefabInfo.WithTechType(
                classId: "OreCloudNite",
                displayName: "Cloud Nite",
                description: "A lightweight ore used for projects that require lightweight material.",
                language: "English",
                unlockAtStart: false,
                techTypeOwner: Assembly.GetExecutingAssembly()
            )
            .WithIcon(Cloudniteoreicon)
            .WithSizeInInventory(new Vector2int(1, 1));

            CustomPrefab prefab = new CustomPrefab(info);

            prefab.SetGameObject(() =>
            {
                GameObject obj = GameObject.Instantiate(bundle.LoadAsset<GameObject>("cloudniteoreprefab"));

                PrefabUtils.AddBasicComponents(obj, info.ClassID, info.TechType, LargeWorldEntity.CellLevel.Near);
                obj.EnsureComponent<Pickupable>();

                PrefabUtils.AddVFXFabricating(obj, null, -0.2f, 0.2f, new Vector3(0f, -0.05f, 0f));

                return obj;
            });




            var cloudNiteRecipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient(TechType.Aerogel, 2), // for light weight properties
                    new Ingredient(TechType.Diamond, 1), // for durablity
                    new Ingredient(TechType.Nickel, 1)   // for durablity

                }

            };

            prefab.SetRecipe(cloudNiteRecipe);





            CraftTreeHandler.AddCraftingNode(
                CraftTree.Type.Fabricator,
                info.TechType,
                "Resources",
                "AdvancedMaterials"
            );

            prefab.SetPdaGroupCategoryAfter(TechGroup.Personal, TechCategory.AdvancedMaterials, TechType.Titanium);


            prefab.Register();
            Cloudnite = info.TechType;
            

            StoryGoalHandler.RegisterItemGoal(
            key: "UnlockCloudNite",
            goalType: Story.GoalType.Encyclopedia,
            techType: info.TechType,
            delay: 20f
            );

            StoryGoalHandler.RegisterItemGoal(
            key: "UnlockCloudNite",
            goalType: Story.GoalType.PDA,
            techType: info.TechType,
            delay: 0f
               );

            StoryGoalHandler.RegisterOnGoalUnlockData(
                goal: "UnlockCloudNite",
                blueprints: new UnlockBlueprintData[]
                {
        new UnlockBlueprintData()
        {
            techType = info.TechType,
            unlockType = UnlockBlueprintData.UnlockType.Available
        }
                }
            );

            PDAHandler.AddEncyclopediaEntry(
                    "UnlockCloudNite",
                    "Advanced",
                    "Cloud Nite Resourse",
                    "This Ore is extremely light weight and durable you can probably use it for light weight submarines,Becomes white in light",
                    Cloudniteoredatabank,
                    Cloudniteoredatapop,
                    PDAHandler.UnlockBasic,
                    null
                );

            PDAHandler.AddLogEntry(
                    "UnlockCloudNite",
                    "UnlockCloudNite",
                   fmodAssetSwiftercloudnite,
                   Swiftericon
                );

            LanguageHandler.SetLanguageLine(
                    "UnlockCloudNite",
                    "<delay=0>Oh hey look at that! you found cloudnite you will need that how ever i might have used almost all of it so...<delay=1000>my bad... but dont worry! i made a synthetic version of it!<delay=1000> <delay=1000> Here il send it over.<delay=2000>Corrupted Blueprint data found attempting recovery.<delay=1000> well thats not going to recover here il try agai-<delay=1000> Recovery success huh... Storing Blueprint in PDA <delay=1000> well i guess i have to take back my words...<delay=1000> Oh by the way my name is Swifter but you can call me Swift.",
                    "English"
                );



            var cloudniteSpawnLocation = new Vector3(1385.5f, -128.3f, -1393.1f);
            var cloudniteSpawnRotation = new Vector3(-90f, 0f, 0f);
            var cloudniteSpawnSize = new Vector3(14f, 14f, 14f);

            SpawnLocation spawn = new SpawnLocation(cloudniteSpawnLocation, cloudniteSpawnRotation, cloudniteSpawnSize);
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(info.TechType, spawn);

            Plugin.Log.LogInfo("Cloud Nite Ore registered and unlock goal set correctly.");
        }

        public static void Registeritem2()
        {
          
            
            AssetBundle bundle = Plugin.ItemMeshBundle;

            // Texture/Spites
            Sprite IonPolymericon = Plugin.TextureBundle.LoadAsset<Sprite>("IonPolymerIcon");  
            Sprite IonPolymerdatapop = Plugin.TextureBundle.LoadAsset<Sprite>("IonPolymerPopup");
            Texture2D IonPolymerdatabank = Plugin.TextureBundle.LoadAsset<Texture2D>("IonPolymerdatabankency");

            if (bundle == null)
            {
                Plugin.Log.LogError("ItemMeshBundle is not loaded!");
                return;
            }

            var info = PrefabInfo.WithTechType(
                classId: "IonPolymer",
                displayName: "Ion Polymer",
                description: "A Alien Strong Alternitive To Synthetic Fibers",
                language: "English",
                unlockAtStart: false,
                techTypeOwner: Assembly.GetExecutingAssembly()
            )
            .WithIcon(IonPolymericon)
            .WithSizeInInventory(new Vector2int(2, 1));

            CustomPrefab prefab = new CustomPrefab(info);

            prefab.SetGameObject(() =>
            {
                GameObject obj = GameObject.Instantiate(bundle.LoadAsset<GameObject>("Ionpolomerprefab"));

                PrefabUtils.AddBasicComponents(obj, info.ClassID, info.TechType, LargeWorldEntity.CellLevel.Near);
                obj.EnsureComponent<Pickupable>();

                MaterialUtils.ApplySNShaders(obj);

                var rend = obj.GetComponent<MeshRenderer>();
                rend.material = MaterialUtils.IonCubeMaterial;


                PrefabUtils.AddVFXFabricating(obj, null, -0.2f, 0.2f, new Vector3(0f, -0.05f, 0f));

                return obj;
            });




            var IonPolymerRecipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient(TechType.PrecursorIonCrystal, 1),
                    new Ingredient(TechType.Diamond, 1),
                    new Ingredient(TechType.FiberMesh, 1)

                }
            };

            prefab.SetRecipe(IonPolymerRecipe);
            
            CraftTreeHandler.AddCraftingNode(
                CraftTree.Type.Workbench,
                
            info.TechType
            );

            prefab.SetPdaGroupCategoryAfter(TechGroup.Personal, TechCategory.AdvancedMaterials, TechType.Titanium);



            prefab.Register();
            IonPolymer = info.TechType;
            

            StoryGoalHandler.RegisterItemGoal(
            key: "UnlockIonPolymer",
            goalType: Story.GoalType.Encyclopedia,
            techType: info.TechType,
            delay: 2f
            );
  

            StoryGoalHandler.RegisterOnGoalUnlockData(
                goal: "UnlockIonPolymer",
                blueprints: new UnlockBlueprintData[]
                {
        new UnlockBlueprintData()
        {
            techType = info.TechType,
            unlockType = UnlockBlueprintData.UnlockType.Available
        }
                }
            );

            PDAHandler.AddEncyclopediaEntry(
                    "UnlockIonPolymer",
                    "Advanced",
                    "Ion Polymer",
                    "This Strange Polymer Is 10 Times Stronger Then Synthetic Fibers And Has A Strange Texture And Feel To It And Glows Green, Recommened To Study This Material.",
                    IonPolymerdatabank,
                    IonPolymerdatapop,
                    PDAHandler.UnlockBasic,
                    null
                );



            var IonPolymerSpawnLocation = new Vector3(-194.4f, -389.2f, 1230.2f);
            var IonPolymerSpawnRotation = new Vector3(0f, 37f, 90f);
            var IonPolymerSpawnSize = new Vector3(13f, 13f, 11.27659f);

            SpawnLocation spawn = new SpawnLocation(IonPolymerSpawnLocation, IonPolymerSpawnRotation, IonPolymerSpawnSize);
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(info.TechType, spawn);

            Plugin.Log.LogInfo("Ion Polymer registered");
        }
        public static void Registerprecursorkey()
        {
            //I didnt make this code below indigocoder did i only yoinked it because i was lazy + i didnt know how to edit the tablets
            // sooo all credits go to indigocoder for this below.

            var  prefabInfo = PrefabInfo.WithTechType("AetherVoidPrecursorKey", true, null)
                .WithIcon(Plugin.TextureBundle.LoadAsset<Sprite>("IonPolymerIcon"));

            var prefab = new CustomPrefab(prefabInfo);
            var cloneTemplate = new CloneTemplate(prefabInfo, TechType.PrecursorKey_Purple);
            cloneTemplate.ModifyPrefab += gameObject =>
            {
                Texture2D replacementGlyph = Plugin.TextureBundle.LoadAsset<Texture2D>("AetherVoidGlyph");
                var rend1 = gameObject.transform.Find("Model/Rig_J/precursor_key_C_02_symbol_05").GetComponent<Renderer>();
                var rend2 = gameObject.transform.Find("ViewModel/Rig_J/precursor_key_C_02_symbol_05").GetComponent<Renderer>();

                var tempMats = rend1.materials;
                tempMats[1].SetTexture("_MainTex", replacementGlyph);
                tempMats[1].SetTexture("_SpecTex", replacementGlyph);
                tempMats[1].SetTexture("_Illum", replacementGlyph);
                rend1.materials = tempMats;

                tempMats = rend2.materials;
                tempMats[1].SetTexture("_MainTex", replacementGlyph);
                tempMats[1].SetTexture("_SpecTex", replacementGlyph);
                tempMats[1].SetTexture("_Illum", replacementGlyph);
                rend2.materials = tempMats;

                gameObject.GetComponent<Collider>().isTrigger = false;

                
            };

            prefab.SetGameObject(cloneTemplate);

            var AetherVoidFacilitykey = new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient(TechType.PrecursorIonCrystal, 1),
                    new Ingredient(IonPolymer, 1),
                    new Ingredient(AUHandler.CobaltTechType, 2)

                }
            };
            prefab.SetRecipe(AetherVoidFacilitykey);

            CraftTreeHandler.AddCraftingNode(
                CraftTree.Type.Workbench,


                prefabInfo.TechType
                );
                
            

            var scanning = new ScanningGadget(prefab, TechType.None, 1)
                .WithScannerEntry(blueprint: prefabInfo.TechType,
                scanTime: 5f,
                isFragment: true,
                null,
                destroyAfterScan: false);

            var TabletSpawn = new Vector3(-248.6f, -448.5f, 1567.0f);
            var TabletSpawnRotation = new Vector3(0f,0f,0f);
            var TabletSpawnSize = new Vector3(1f,1f,1f);

           
            prefab.Register();

            StoryGoalHandler.RegisterItemGoal("FacilityKeyIsPickedUp", Story.GoalType.Story, prefabInfo.TechType, 0);

            StoryGoalHandler.RegisterCustomEvent("FacilityKeyIsPickedUp", () =>
            {
                GameObject Facilityforcefieldobj = GameObject.Find("ForceField_TRANSPARENT").gameObject;
                GameObject Facility = GameObject.Find("FacilityLights").gameObject;
                
                if (Facility != null)
                {
                    
                    var lights = Facility.GetComponentsInChildren<Light>(true);
                    foreach (var light in lights)
                    {
                        light.enabled = true;
                    }


                }

                if (Facilityforcefieldobj == null)
                {
                    Plugin.Log.LogError($"No Facilityforcefieldobj not found.");
                }
                else if (Facilityforcefieldobj != null && Facilityforcefieldobj.name == "ForceField_TRANSPARENT")
                {
                    var rend = Facilityforcefieldobj.GetComponent<MeshRenderer>();
                    var Collider = Facilityforcefieldobj.GetComponent<BoxCollider>();

                    rend.enabled = false;
                    Collider.enabled = false;
                }

            });

            SpawnLocation spawn = new SpawnLocation(TabletSpawn, TabletSpawnRotation, TabletSpawnSize);
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(prefabInfo.TechType, spawn);

        }

    }
}
