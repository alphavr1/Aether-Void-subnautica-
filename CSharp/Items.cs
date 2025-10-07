using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Extensions;
using Nautilus.FMod;
using Nautilus.FMod.Interfaces;
using Nautilus.Handlers;
using Nautilus.Utility;
using Story;
using System.Collections.Generic;
using System.Reflection;
using Testbiome;
using UnityEngine;
using Violet.Testbiome;
using Plugin = Testbiome.Plugin;

namespace Violet.Testbiome
{
    public static class Items
    {
        public static void Registeritem1()
        {
            // Grab bundle from Plugin.cs
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
                    "bus:/master/SFX_for_pause" // <-- Make sure this bus exists
                );

            Swiftercloudnitebuilder.SetSound("SwifterCloudnite")
                .SetFadeDuration(0.5f)
                .SetMode2D(false);

            // Register it so FMOD recognizes it
            Swiftercloudnitebuilder.Register();

            var fmodAssetSwiftercloudnite = Nautilus.Utility.AudioUtils.GetFmodAsset("SwifterCloudnite");


            if (bundle == null)
            {
                Debug.LogError("ItemMeshBundle is not loaded!");
                return;
            }

            //Create PrefabInfo
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

            //Create prefab
            CustomPrefab prefab = new CustomPrefab(info);

            prefab.SetGameObject(() =>
            {
                GameObject prefabModel = bundle.LoadAsset<GameObject>("cloudniteoreprefab");
                if (prefabModel == null)
                {
                    Debug.LogError("Could not find Cloudnite prefab in bundle!");
                    return null;
                }

                GameObject obj = GameObject.Instantiate(prefabModel);

                // Add Nautilus components
                PrefabUtils.AddBasicComponents(obj, info.ClassID, info.TechType, LargeWorldEntity.CellLevel.Near);
                obj.EnsureComponent<Pickupable>();



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





            //Add to fab crafting tree
            CraftTreeHandler.AddCraftingNode(
                CraftTree.Type.Fabricator,
                info.TechType,
                "Resources",
                "AdvancedMaterials"
            );

            //Register prefab
            prefab.Register();


            //Register Item Goal for unlocking when picked up
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

            // Specify what happens when the goal unlocks (unlock blueprint)
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
                    "UnlockCloudNite",  // Unique key here
                    "UnlockCloudNite",  // Matching language key
                   fmodAssetSwiftercloudnite,
                   Swiftericon
                );

            LanguageHandler.SetLanguageLine(
                    "UnlockCloudNite",
                    "<delay=0>Oh hey look at that! you found cloudnite you will need that how ever i might have used almost all of it so...<delay=1000>my bad... but dont worry! i made a synthetic version of it!<delay=1000> <delay=1000> Here il send it over.<delay=2000>Corrupted Blueprint data found attempting recovery.<delay=1000> well thats not going to recover here il try agai-<delay=1000> Recovery success huh... Storing Blueprint in PDA <delay=1000> well i guess i have to take back my words...<delay=1000> Oh by the way my name is Swifter but you can call me Swift.",
                    "English"
                );



            //Forced spawn in the world
            var cloudniteSpawnLocation = new Vector3(1385.5f, -128.3f, -1393.1f);
            var cloudniteSpawnRotation = new Vector3(-90f, 0f, 0f);
            var cloudniteSpawnSize = new Vector3(14f, 14f, 14f);

            SpawnLocation spawn = new SpawnLocation(cloudniteSpawnLocation, cloudniteSpawnRotation, cloudniteSpawnSize);
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(info.TechType, spawn);

            Plugin.Log.LogInfo("Cloud Nite Ore registered and unlock goal set correctly.");
        }
    }
}
