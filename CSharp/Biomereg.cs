
using Nautilus.Handlers;
using Nautilus.Utility;
using Testbiome;
using UnityEngine;

namespace Violet.Testbiome
{
    public static class CustomBiome
    {
        public static void RegisterCustomBiome()
        {
            // Create biome settings with your desired parameters
            var biomeSettings = BiomeUtils.CreateBiomeSettings(
                new Vector3(100f, 18.3f, 3.53f), // absorption
                1f,                              // scattering
                Color.blue,                     // scatteringColor
                1f,                              // murkiness
                Color.black,                     // emissive
                1f,                              // emissiveScale
                25f,                             // startDistance
                1f,                              // sunlightScale
                1f,                              // ambientScale
                24f                              
            );

            // Create the sky prefab using the provided reflection texture
            var skyReference = new BiomeHandler.SkyReference("SkyGrandReef");

            BiomeHandler.RegisterBiome("AetherVoidsurface", biomeSettings, skyReference);
            Plugin.Log.LogMessage("Registerd Aether Void Surface");


            var fmodAsset = ScriptableObject.CreateInstance<FMODAsset>();
            fmodAsset.path = "event:/env/background/grandreef_background";
            BiomeHandler.AddBiomeAmbience("AetherVoidsurface", fmodAsset, FMODGameParams.InteriorState.Always);
        }

        public static void RegisterCustomBiome2()
        {
            // Create biome settings with your desired parameters
            var biomeSettings = BiomeUtils.CreateBiomeSettings(
                new Vector3(100f, 18.3f, 3.53f), // absorption
                1f,                              // scattering
                Color.blue,                     // scatteringColor
                1f,                              // murkiness
                Color.black,                     // emissive
                1f,                              // emissiveScale
                25f,                             // startDistance
                1f,                              // sunlightScale
                1f,                              // ambientScale
                24f                              // temperature
            );

            // Create the sky prefab using the provided reflection texture
            var skyReference = new BiomeHandler.SkyReference("SkyDeepGrandReef");

            BiomeHandler.RegisterBiome("AetherVoid", biomeSettings, skyReference);
            Plugin.Log.LogMessage("Registerd Aether Void Deep");



            var fmodAsset = ScriptableObject.CreateInstance<FMODAsset>();
            fmodAsset.path = "event:/env/background/grandreef_background";
            BiomeHandler.AddBiomeAmbience("AetherVoid", fmodAsset, FMODGameParams.InteriorState.Always);


        }
    }
  }
