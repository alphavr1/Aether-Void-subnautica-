using Microsoft.Win32;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using Oculus.Platform;
using Story;
using System.Collections.Generic;
using System.Reflection;
using Testbiome;
using UnityEngine;
using Violet.Testbiome;
using static ICSharpCode.SharpZipLib.Zip.Compression.DeflaterHuffman;
using static uGUI_TabbedControlsPanel;
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
            Sprite Cloudniteoredatapop = Plugin.TextureBundle.LoadAsset<Sprite>("databank-popup-Cloudnite");
            Texture2D Cloudniteoredatabank = Plugin.TextureBundle.LoadAsset<Texture2D>("databank-image-Cloudnite");

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
                    "This Ore is extremely light weight and durable you can probably use it for light weight submarines",
                    Cloudniteoredatabank,
                    Cloudniteoredatapop,
                    PDAHandler.UnlockBasic,
                    null
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
