using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Testbiome;
using UnityEngine;

namespace Violet.Testbiome
{
    public class SpawnBiomes
    {
        private bool aethervoidsurfaceSpawned = false;
        private bool aethervoidSpawned = false;


        public void StartSpawnCoroutine(MonoBehaviour coroutineRunner)
        {
            coroutineRunner.StartCoroutine(WaitForPlayerAndSpawn());
        }

        private System.Collections.IEnumerator WaitForPlayerAndSpawn()
        {
            while (Player.main == null)
                yield return null;

            if (!aethervoidsurfaceSpawned)
            {
                SpawnBiomeVolume();
                aethervoidsurfaceSpawned = true;
                Plugin.Log.LogInfo("Aether Void Surface Spawned :D");
            }
            else
            {
                Plugin.Log.LogError("Aether Void Surface Failed To Spawn (i hope i never see this message in a logoutput file...)");
            }

            if (!aethervoidSpawned)
            {
                SpawnBiomeVolume2();
                aethervoidSpawned = true;
                Plugin.Log.LogInfo("Aether Void Spawned :D");
            }
            else
            {
                Plugin.Log.LogError("Aether Void Failed To Spawn (i hope i never see this message in a logoutput file...)");
            }
        }

        public void SpawnBiomeVolume()
        {
            // Create a prefab for the biome volume
            PrefabInfo volumePrefabInfo = PrefabInfo.WithTechType("AetherVoidSurfaceCubeVolume");
            CustomPrefab volumePrefab = new CustomPrefab(volumePrefabInfo);

            // Atmosphere volume linked to your custom biome "AtherVoid"
            AtmosphereVolumeTemplate volumeTemplate = new AtmosphereVolumeTemplate(
                volumePrefabInfo,
                AtmosphereVolumeTemplate.VolumeShape.Cube,
                "AetherVoidsurface"
            );

            volumePrefab.SetGameObject(volumeTemplate);
            volumePrefab.Register();

            // Place the biome in the world at your coordinates
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(
                new SpawnInfo(volumePrefabInfo.ClassID,
                              new Vector3(1311.1f, -50f, -1361f),
                              Quaternion.identity,
                              new Vector3(250f, 250f, 250f)) // volume scale
            );

            ConsoleCommandsHandler.AddGotoTeleportPosition("AetherVoidsurface", new Vector3(1311.1f, -50f, -1361f));
            ConsoleCommandsHandler.AddGotoTeleportPosition("AetherVoid", new Vector3(1311.1f, -1500f, -1361f));



        }

        public void SpawnBiomeVolume2()
        {
            // Create a prefab for the biome volume
            PrefabInfo volumePrefabInfo = PrefabInfo.WithTechType("AetherVoidCubeVolume");
            CustomPrefab volumePrefab = new CustomPrefab(volumePrefabInfo);

            // Atmosphere volume linked to your custom biome "AtherVoid"
            AtmosphereVolumeTemplate volumeTemplate = new AtmosphereVolumeTemplate(
                volumePrefabInfo,
                AtmosphereVolumeTemplate.VolumeShape.Cube,
                "AetherVoid"
            );

            volumePrefab.SetGameObject(volumeTemplate);
            volumePrefab.Register();

            // Place the biome in the world at your coordinates
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(
                new SpawnInfo(volumePrefabInfo.ClassID,
                              new Vector3(1311.1f, -1500f, -1361f),
                              Quaternion.identity,
                              new Vector3(500f, 500f, 500f)) // volume scale
            );

            

        }

    }



}





