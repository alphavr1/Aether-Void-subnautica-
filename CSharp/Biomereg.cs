using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using AV;

namespace Violet.AV
{
    public static class Biomereg
    {
        public static void RegisterCustomBiome()
        {
            
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

            var skyReference = new BiomeHandler.SkyReference("SkyGrandReef");

            BiomeHandler.RegisterBiome("AetherVoidsurface", biomeSettings, skyReference);
            Plugin.Log.LogMessage("Registerd Aether Void Surface");


            var fmodAsset = ScriptableObject.CreateInstance<FMODAsset>();
            fmodAsset.path = "event:/env/background/grandreef_background";
            BiomeHandler.AddBiomeAmbience("AetherVoidsurface", fmodAsset, FMODGameParams.InteriorState.Always);
        }

        public static void RegisterCustomBiome2()
        {
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

            var skyReference = new BiomeHandler.SkyReference("SkyDeepGrandReef");

            BiomeHandler.RegisterBiome("AetherVoid", biomeSettings, skyReference);
            Plugin.Log.LogMessage("Registerd Aether Void Deep");



            var fmodAsset = ScriptableObject.CreateInstance<FMODAsset>();
            fmodAsset.path = "event:/env/background/grandreef_background";
            BiomeHandler.AddBiomeAmbience("AetherVoid", fmodAsset, FMODGameParams.InteriorState.Always);


        }

        public static void RegisterCustomBiome3()
        {
            var biomeSettings = BiomeUtils.CreateBiomeSettings(
                new Vector3(100f, 18.3f, 3.53f), // absorption
                1f,                              // scattering
                Color.white,                     // scatteringColor
                1f,                              // murkiness
                Color.green,                     // emissive
                1f,                              // emissiveScale
                25f,                             // startDistance
                1f,                              // sunlightScale
                1f,                              // ambientScale
                15f                              // temperature
            );

            var skyReference = new BiomeHandler.SkyReference("SkyPrecursorPrisonAquarium");

            BiomeHandler.RegisterBiome("PrecursorFacilityIonPolymer", biomeSettings, skyReference);
            Plugin.Log.LogMessage("Registerd Precursor Facility Ion Polymer");



            var fmodAsset = ScriptableObject.CreateInstance<FMODAsset>();
            fmodAsset.path = "event:/env/background/grandreef_background";
            BiomeHandler.AddBiomeAmbience("AetherVoid", fmodAsset, FMODGameParams.InteriorState.Always);


        }
    }
  }
