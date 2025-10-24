using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using AV;
using UnityEngine;

namespace Violet.AV
{
    public class spawnbiomes
    {
        private bool aethervoidsurfaceSpawned = false;
        private bool aethervoidSpawned = false;
        private bool PrecursorFacilitySpawned = false;


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

            if (!PrecursorFacilitySpawned)
            {
                SpawnBiomeVolume3();
                PrecursorFacilitySpawned = true;
                Plugin.Log.LogInfo("Precursor Ion Polymer Facility Biome Spawned :D");
            }
            else
            {
                Plugin.Log.LogError("Precursor Ion Polymer Facility Biome Failed To Spawn (i hope i never see this message in a logoutput file...)");
            }
        }

        public void SpawnBiomeVolume()
        {
            PrefabInfo volumePrefabInfo = PrefabInfo.WithTechType("AetherVoidSurfaceCubeVolume");
            CustomPrefab volumePrefab = new CustomPrefab(volumePrefabInfo);

            AtmosphereVolumeTemplate volumeTemplate = new AtmosphereVolumeTemplate(
                volumePrefabInfo,
                AtmosphereVolumeTemplate.VolumeShape.Cube,
                "AetherVoidsurface"
            );

            volumePrefab.SetGameObject(volumeTemplate);
            volumePrefab.Register();

            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(
                new SpawnInfo(volumePrefabInfo.ClassID,
                              new Vector3(1311.1f, -50f, -1361f),
                              Quaternion.identity,
                              new Vector3(250f, 250f, 250f))
            );

            ConsoleCommandsHandler.AddGotoTeleportPosition("AetherVoidsurface", new Vector3(1311.1f, -50f, -1361f));
            ConsoleCommandsHandler.AddGotoTeleportPosition("AetherVoid", new Vector3(1311.1f, -1500f, -1361f));



        }

        public void SpawnBiomeVolume2()
        {
            PrefabInfo volumePrefabInfo = PrefabInfo.WithTechType("AetherVoidCubeVolume");
            CustomPrefab volumePrefab = new CustomPrefab(volumePrefabInfo);

            AtmosphereVolumeTemplate volumeTemplate = new AtmosphereVolumeTemplate(
                volumePrefabInfo,
                AtmosphereVolumeTemplate.VolumeShape.Cube,
                "AetherVoid"
            );

            volumePrefab.SetGameObject(volumeTemplate);
            volumePrefab.Register();

            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(
                new SpawnInfo(volumePrefabInfo.ClassID,
                              new Vector3(1311.1f, -1500f, -1361f),
                              Quaternion.identity,
                              new Vector3(500f, 500f, 500f))
            );

            

        }

        public void SpawnBiomeVolume3()
        {
            PrefabInfo volumePrefabInfo = PrefabInfo.WithTechType("PrecursorFacilityIonPolymer");
            CustomPrefab volumePrefab = new CustomPrefab(volumePrefabInfo);

            AtmosphereVolumeTemplate volumeTemplate = new AtmosphereVolumeTemplate(
                volumePrefabInfo,
                AtmosphereVolumeTemplate.VolumeShape.Cube,
                "PrecursorFacilityIonPolymer"
            );

            volumePrefab.SetGameObject(volumeTemplate);
            volumePrefab.Register();

            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(
                new SpawnInfo(volumePrefabInfo.ClassID,
                              new Vector3(-175.9f, -385f, 1209.8f),
                              Quaternion.identity,
                              new Vector3(100f, 50f, 150f))
            );
        }
    }
}
