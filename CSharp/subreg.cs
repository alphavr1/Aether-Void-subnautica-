using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using SubLibrary.Handlers;
using SubLibrary.Monobehaviors;
using SubLibrary.SaveData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plugin = AV.Plugin;

namespace Violet.AV
{
    internal static class subreg
    {
        public static bool hasregisted = false;

        public static IEnumerator RegisterSubmarine()
        {
            var model = Plugin.SubBundle.LoadAsset<GameObject>("place holder vehicle2");
            model.SetActive(false);

            var prefab = GameObject.Instantiate(model);

            
            try
            {
                MaterialUtils.ApplySNShaders(prefab);
            }
            catch (System.NullReferenceException ex)
            {
                Debug.LogWarning("[Sub Registration] Skipped shader application due to missing renderer: " + ex.Message);
            }

            //yield return CyclopsReferenceHandler.EnsureCyclopsReference();
            //yield return InterfaceCallerHandler.InvokeCyclopsReferencers(prefab);

            foreach (var modifier in prefab.GetComponentsInChildren<PrefabModifier>(true))
            {
                modifier.OnAsyncPrefabTasksCompleted();
                modifier.OnLateMaterialOperation();
            }


            prefab.EnsureComponent<SubSerializationManager>();

            var info = PrefabInfo.WithTechType(
                classId: "Aethr",
                displayName: "Aethr Sub",
                description: "A experimental sub.",
                techTypeOwner: System.Reflection.Assembly.GetExecutingAssembly()
            )
            .WithIcon(SpriteManager.Get(TechType.Cyclops));

            CustomPrefab customSub = new CustomPrefab(info);

            var subRecipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>
    {
        new Ingredient(TechType.PlasteelIngot, 3),
        new Ingredient(TechType.AdvancedWiringKit, 1),
        new Ingredient(TechType.EnameledGlass, 2),
        new Ingredient(TechType.PowerCell, 2)
    }
            };

            CraftTreeHandler.AddCraftingNode(CraftTree.Type.Constructor, info.TechType);
            customSub.SetRecipe(subRecipe);

            customSub.SetGameObject(prefab);

            customSub.Register();

            hasregisted = true;
            yield return customSub;
        }
    }
}

