using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Handlers.TitleScreen;
using Nautilus.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using Violet.AV;

namespace AV
{
    [BepInPlugin(GUID, pluginName, versionString)]
    [BepInDependency("com.snmodding.nautilus",BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("Esper89.TerrainPatcher")]
    [BepInDependency("com.prototech.prototypesub", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.aotu.architectslibrary",BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.indigocoder.sublibrary",BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "com.Ather.test";
        private const string pluginName = "test";
        private const string versionString = "1.0.0";

        const string soundId = "AtherVoidMusic";

        private static readonly Harmony Harmony = new Harmony(GUID);
        public static ManualLogSource Log;

        //Config Bools/needed stuff
        public static bool useknifebundle = true;
        internal static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public static string AssetsFolderPath { get; } = Path.Combine(Path.GetDirectoryName(Assembly.Location), "Assets");
        public static AssetBundle AudioBundle { get; private set; }
        public static AssetBundle SubBundle { get; private set; }
        public static AssetBundle TitleBundle { get; private set; }
        public static AssetBundle ItemMeshBundle { get; private set; }
        public static AssetBundle TextureBundle { get; private set; }

        private static WorldObjectTitleAddon objectAddon;
        private static WorldObjectTitleAddon objectAddon1;
        private static WorldObjectTitleAddon objectAddon2;
        private static WorldObjectTitleAddon objectAddon3;
        private static WorldObjectTitleAddon objectAddon4;
        private static WorldObjectTitleAddon objectAddon5;
        private static WorldObjectTitleAddon objectAddon6;
        private static WorldObjectTitleAddon objectAddon7;

        private void Awake()
        {
            Harmony.PatchAll();

            Logger.LogInfo($"{pluginName} {versionString} loaded.");
            Log = Logger;

            string TitleBundlePath = Path.Combine(AssetsFolderPath, "titleassets");
            TitleBundle = AssetBundle.LoadFromFile(TitleBundlePath);

            foreach (var assetName in TitleBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"Title contains: {assetName}");
            }

            // Anti Piracy
            TryFindPiracy();

            //Load And Spawn

            WaitScreenHandler.RegisterEarlyAsyncLoadTask("Aether Void", LoadAudioAssets, "Loading Audio From AssetBundels");
            WaitScreenHandler.RegisterEarlyAsyncLoadTask("Aether Void", LoadItemandmeshAssets, "Loading Mesh And Items From AssetBundels");
            WaitScreenHandler.RegisterEarlyAsyncLoadTask("Aether Void", LoadTextureAssets, "Loading Textures From AssetBundels");
            WaitScreenHandler.RegisterEarlyAsyncLoadTask("Aether Void", LoadSubAssets, "Loading Aethr Submarine From AssetBundels");
            WaitScreenHandler.RegisterAsyncLoadTask("Aether Void", RegisterAudio, "Registering Audio");
            WaitScreenHandler.RegisterAsyncLoadTask("Aether Void", RegisterItemsAndFacilitys, "Registering Items And Facilitys");
            //WaitScreenHandler.RegisterAsyncLoadTask("Aether Void", RegisterAethr, "Registering Aethr Submarine");
            WaitScreenHandler.RegisterAsyncLoadTask("Aether Void", RegisterAssets, "Registering The Rest Of The Assets Like Biomes");


            
            


            var skyPrefabFixer = BiomeUtils.CreateSkyPrefab("SkyDeepGrandReef", null, true, true);
            Texture specularCubeTexture = skyPrefabFixer.specularCube;

            // Register main menu items
            objectAddon = new WorldObjectTitleAddon(SpawnObject);
            objectAddon1 = new WorldObjectTitleAddon(SpawnObject1);
            objectAddon2 = new WorldObjectTitleAddon(SpawnObject2);
            objectAddon3 = new WorldObjectTitleAddon(SpawnObject3);
            objectAddon4 = new WorldObjectTitleAddon(SpawnObject4);
            objectAddon5 = new WorldObjectTitleAddon(SpawnObject5);
            objectAddon6 = new WorldObjectTitleAddon(SpawnObject6);
            objectAddon7 = new WorldObjectTitleAddon(SpawnObject7);

            var musicAddon = GetMusicAddon();
            var skyChangeAddon = new SkyChangeTitleAddon(1f, new SkyChangeTitleAddon.Settings(0f, fogDensity: 0.00001f));
            var theme = new TitleScreenHandler.CustomTitleData("Aether Void", objectAddon, objectAddon1, objectAddon2, objectAddon3, objectAddon4, objectAddon5, objectAddon6, objectAddon7, musicAddon, skyChangeAddon);
            TitleScreenHandler.RegisterTitleScreenObject("Aether Void", theme);

        }

        private IEnumerator RegisterAssets(WaitScreenHandler.WaitScreenTask task)
        {
            var spawner = new spawnbiomes();
            spawner.StartSpawnCoroutine(this);

            yield return new WaitUntil(() => spawner.aethervoidSpawned && spawner.aethervoidsurfaceSpawned == true);
        }

        private IEnumerator RegisterItemsAndFacilitys(WaitScreenHandler.WaitScreenTask task)
        {
            Biomereg.RegisterCustomBiome();
            Biomereg.RegisterCustomBiome2();
            Items.Registeritem1();
            Items.Registeritem2();
            Items.Registerprecursorkey();
            FacilitySpawner.Aetherkeytunlocktrigger();
            FacilitySpawner.Patch();
            
            Violet.AV.CrystalKnifeClass.CrystalKnife.Register();

            yield return new WaitUntil(() => Biomereg.hasregisteredbiome1 && Biomereg.hasregisteredbiome2 && CrystalKnifeClass.Hasregistedknife == true);

        }

        private IEnumerator RegisterAudio(WaitScreenHandler.WaitScreenTask task)
        {
            pdavoicelinereg.AetherVoidBiomePDA.Register();
            databanks.data.Register();

            yield return new WaitUntil(() => pdavoicelinereg.AetherVoidBiomePDA.isdoneregistering && databanks.data.isdoneregistering == true);
        }

        private IEnumerator RegisterAethr(WaitScreenHandler.WaitScreenTask task)
        {
            subreg.RegisterSubmarine();

            yield return new WaitUntil(() => subreg.hasregisted == true);
        }

        private IEnumerator LoadItemandmeshAssets(WaitScreenHandler.WaitScreenTask task)
        {
            string ItemTitleBundlePath = Path.Combine(AssetsFolderPath, "item mesh");
            ItemMeshBundle = AssetBundle.LoadFromFile(ItemTitleBundlePath);
              
            
            foreach (var assetName in ItemMeshBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"KnifeTitleBundle contains: {assetName}");
            }

            yield return new WaitUntil(() => ItemMeshBundle != null);

        }

        private IEnumerator LoadAudioAssets(WaitScreenHandler.WaitScreenTask task)
        {
            string audioBundlePath = Path.Combine(AssetsFolderPath, "audio");
            AudioBundle = AssetBundle.LoadFromFile(audioBundlePath);
              yield return new WaitUntil(() => AudioBundle != null);
            
            foreach (var assetName in AudioBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"AudioBundle contains: {assetName}");
            }
        }

        private IEnumerator LoadTextureAssets(WaitScreenHandler.WaitScreenTask task)
        {
            string TextureBundlePath = Path.Combine(AssetsFolderPath, "texture");
            TextureBundle = AssetBundle.LoadFromFile(TextureBundlePath);
            yield return new WaitUntil(() => TextureBundle != null);

            foreach (var assetName in TextureBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"TextureBundle contains: {assetName}");
            }

        }

        private IEnumerator LoadSubAssets(WaitScreenHandler.WaitScreenTask task)
        {
            string SubBundlePath = Path.Combine(AssetsFolderPath, "aethersub");
            SubBundle = AssetBundle.LoadFromFile(SubBundlePath);
            yield return new WaitUntil(() => SubBundle != null);

            foreach (var assetName in SubBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"SubBundle contains: {assetName}");
            }

        } 

        public static bool TryFindPiracy()
        {
            foreach (var file in PiracyFiles)
            {
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, file)))
                {
                    Console.WriteLine($"{Assembly.GetExecutingAssembly().GetName()} does not support piracy!");
                    ErrorMessage.AddError($"{Assembly.GetExecutingAssembly().GetName()} does not support piracy!");
                    Console.WriteLine("You can purchase the game at discounted prices here: https://isthereanydeal.com/game/subnautica/info/");
                    ErrorMessage.AddError("You can purchase the game at discounted prices here https://isthereanydeal.com/game/subnautica/info/");
                    return true;
                }
                else
                {
                    Console.WriteLine("Piracy Detection Passed For This File");
                }
            }

            return false;
        }

        private MusicTitleAddon GetMusicAddon()
        {
            var sound = AudioUtils.CreateSound(TitleBundle.LoadAsset<AudioClip>("into-the-aether"), AudioUtils.StandardSoundModes_Stream);

            CustomSoundHandler.RegisterCustomSound(soundId, sound, AudioUtils.BusPaths.Music);

            return new MusicTitleAddon(AudioUtils.GetFmodAsset(soundId));


        }

            public static readonly HashSet<string> PiracyFiles = new HashSet<string> { "steam_api64.cdx", "steam_api64.ini", "steam_emu.ini", "valve.ini", "chuj.cdx", "SteamUserID.cfg", "Achievements.bin", "steam_settings", "user_steam_id.txt", "account_name.txt", "ScreamAPI.dll", "ScreamAPI32.dll", "ScreamAPI64.dll", "SmokeAPI.dll", "SmokeAPI32.dll", "SmokeAPI64.dll", "Free Steam Games Pre-installed for PC.url", "Torrent-Igruha.Org.URL", "oalinst.exe", };



        GameObject SpawnObject()
        {
            var prefab = TitleBundle.LoadAsset<GameObject>("aether");
            var obj = GameObject.Instantiate(prefab);

            obj.transform.position = new Vector3(-7, 5.5f, 25);
            obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            obj.transform.localScale = new Vector3(400, 400, 400);


            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            MaterialUtils.ApplySNShaders(obj);

            var renderers = obj.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                var mats = r.materials;
                for (int i = 0; i < mats.Length; i++)
                    mats[i] = new Material(mats[i]);
                r.materials = mats;
            }

            var rend = obj.GetComponentInChildren<Renderer>();

            var src = obj.GetComponent<AudioSource>() ?? obj.AddComponent<AudioSource>();
            src.clip = TitleBundle.LoadAsset<AudioClip>("into-the-aether");
            src.loop = false;
            src.playOnAwake = true;
            src.volume = 0.05f;

            var changer = obj.AddComponent<AudioTwoColorChanger>();
            changer.targetRenderer = rend;
            changer.quietColor = Color.magenta;
            changer.loudColor = Color.cyan;
            changer.sensitivity = 11000f;
            changer.smoothSpeed = 15f;

            return obj;
        }

        GameObject SpawnObject1()
        {
            var prefab = TitleBundle.LoadAsset<GameObject>("void");
            var obj = GameObject.Instantiate(prefab);

            obj.transform.position = new Vector3(14, 0f, 25);
            obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            obj.transform.localScale = new Vector3(400, 400, 400);


            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            MaterialUtils.ApplySNShaders(obj);

            var renderers = obj.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                var mats = r.materials;
                for (int i = 0; i < mats.Length; i++)
                    mats[i] = new Material(mats[i]);
                r.materials = mats;
            }

            var rend = obj.GetComponentInChildren<Renderer>();

            var src = obj.GetComponent<AudioSource>() ?? obj.AddComponent<AudioSource>();
            src.clip = TitleBundle.LoadAsset<AudioClip>("into-the-aether");
            src.loop = false;
            src.playOnAwake = true;
            src.volume = 0.05f;

            var changer = obj.AddComponent<AudioTwoColorChanger>();
            changer.targetRenderer = rend;
            changer.quietColor = Color.cyan;
            changer.loudColor = Color.magenta;
            changer.sensitivity = 11000f;
            changer.smoothSpeed = 15f;

            return obj;

        }

        GameObject SpawnObject2()
        {
            var prefab = TitleBundle.LoadAsset<GameObject>("thankyoulist");
            var obj = GameObject.Instantiate(prefab);

            obj.transform.position = new Vector3(-17, 7f, 20);
            obj.transform.rotation = Quaternion.Euler(0f, 140f, 0f);
            obj.transform.localScale = new Vector3(40, 40, 40);


            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            MaterialUtils.ApplySNShaders(obj);

            obj.AddComponent<ColorCycler2>();

            return obj;

        }

        GameObject SpawnObject3()
        {
            var prefab = TitleBundle.LoadAsset<GameObject>("goober");
            var obj = GameObject.Instantiate(prefab);

            obj.transform.position = new Vector3(-8, -1f, 5f);
            obj.transform.rotation = Quaternion.Euler(-90f, 180f, 0f);
            obj.transform.localScale = new Vector3(40, 40, 40);


            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            MaterialUtils.ApplySNShaders(obj);

            obj.AddComponent<ColorCycler2>();

            return obj;

        }

        GameObject SpawnObject4()
        {
            var prefab = TitleBundle.LoadAsset<GameObject>("plane");
            var obj = GameObject.Instantiate(prefab);

            obj.transform.position = new Vector3(-5, -1f, 10);
            obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);


            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;


            MaterialUtils.ApplySNShaders(obj);

            Texture2D myTexture = TitleBundle.LoadAsset<Texture2D>("screenshot 2025-08-09 205356");

            if (myTexture != null)
            {

                var renderers = obj.GetComponentsInChildren<Renderer>(true);

                foreach (var renderer in renderers)
                {
                    foreach (var mat in renderer.materials)
                    {
                        mat.mainTexture = myTexture;
                    }
                }
            }
            else
            {
                Log.LogError($"Texture {myTexture} not found!");
            }


            return obj;

        }

        GameObject SpawnObject5()
        {
            var prefab = TitleBundle.LoadAsset<GameObject>("yippe");
            var obj = GameObject.Instantiate(prefab);

            obj.transform.position = new Vector3(10, 3f, 15);
            obj.transform.rotation = Quaternion.Euler(0f, 220f, 0f);
            obj.transform.localScale = new Vector3(40, 40, 40);


            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            MaterialUtils.ApplySNShaders(obj);

            obj.AddComponent<ColorCycler3>();

            return obj;

        }

        GameObject SpawnObject6()
        {
            var prefab = TitleBundle.LoadAsset<GameObject>("crystal (isuckok;_;)");
            var obj = GameObject.Instantiate(prefab);

            obj.transform.position = new Vector3(-15, -5f, 70);
            obj.transform.rotation = Quaternion.Euler(-122.005f, -116.653f, 86.159f);
            obj.transform.localScale = new Vector3(400, 400, 2500);


            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            MaterialUtils.ApplySNShaders(obj);

            obj.AddComponent<ColorCycler4>();

            return obj;

        }

        GameObject SpawnObject7()
        {
            var prefab = TitleBundle.LoadAsset<GameObject>("crystal (isuckok;_;)");
            var obj = GameObject.Instantiate(prefab);

            obj.transform.position = new Vector3(17, -5f, 70);
            obj.transform.rotation = Quaternion.Euler(-125.544f, 41.48599f, -67.10599f);
            obj.transform.localScale = new Vector3(400, 400, 2500);


            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            MaterialUtils.ApplySNShaders(obj);

            obj.AddComponent<ColorCycler5>();

            return obj;

        }

        [RequireComponent(typeof(AudioSource))]
        class AudioTwoColorChanger : MonoBehaviour
        {
            public Renderer targetRenderer;
            public string colorProperty = "_Color";

            public Color quietColor = Color.magenta;
            public Color loudColor = Color.blue;

            public float sensitivity = 200f;
            public float smoothSpeed = 15f;
            public FFTWindow fftWindow = FFTWindow.BlackmanHarris;

            private AudioSource captureSource;
            private float[] spectrumData = new float[64];
            private float smoothedIntensity = 0f;

            void Start()
            {
                captureSource = GetComponent<AudioSource>();

                if (targetRenderer == null)
                {
                    targetRenderer = GetComponent<Renderer>();
                }

                if (targetRenderer == null)
                {
                    Log.LogError("[AudioTwoColorChanger] No renderer found!");
                }
            }

            void Update()
            {
                if (captureSource == null || targetRenderer == null) return;

                captureSource.GetSpectrumData(spectrumData, 0, fftWindow);

                float average = 0f;
                for (int i = 0; i < spectrumData.Length; i++)
                {
                    average += spectrumData[i];
                }
                average /= spectrumData.Length;

                float targetIntensity = Mathf.Clamp01(average * sensitivity);

                smoothedIntensity = Mathf.Lerp(smoothedIntensity, targetIntensity, Time.deltaTime * smoothSpeed);

                Color targetColor = Color.Lerp(quietColor, loudColor, smoothedIntensity);

                if (targetRenderer.material.HasProperty(colorProperty))
                {
                    Color currentColor = targetRenderer.material.GetColor(colorProperty);
                    Color smoothColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * smoothSpeed);
                    targetRenderer.material.SetColor(colorProperty, smoothColor);
                }
            }
        }



        public class ColorCycler : MonoBehaviour
        {
            private Renderer rend;

            void Start()
            {
                rend = GetComponentInChildren<Renderer>();
            }

            void Update()
            {
                if (rend != null)
                {
                    float t = Mathf.PingPong(Time.time, 5f);
                    Color c = Color.Lerp(Color.magenta, Color.cyan, t);

                    foreach (var mat in rend.materials)
                    {
                        if (mat.HasProperty("_Color"))
                        {
                            mat.color = c;
                        }
                        else if (mat.HasProperty("_TintColor"))
                        {
                            mat.SetColor("_TintColor", c);
                        }
                    }
                }
            }
        }
        public class ColorCycler2 : MonoBehaviour
        {
            private Renderer rend;

            void Start()
            {
                rend = GetComponentInChildren<Renderer>();
            }

            void Update()
            {
                if (rend != null)
                {
                    float t = Mathf.PingPong(Time.time, 2f);
                    Color c = Color.Lerp(Color.black, Color.green, t);

                    foreach (var mat in rend.materials)
                    {
                        if (mat.HasProperty("_Color"))
                        {
                            mat.color = c;
                        }
                        else if (mat.HasProperty("_TintColor"))
                        {
                            mat.SetColor("_TintColor", c);
                        }
                    }
                }
            }
        }

        public class ColorCycler3 : MonoBehaviour
        {
            private Renderer rend;

            void Start()
            {
                rend = GetComponentInChildren<Renderer>();
            }

            void Update()
            {
                if (rend != null)
                {
                    float t = Mathf.PingPong(Time.time, 9999f);
                    Color c = Color.Lerp(Color.cyan, Color.cyan, t);

                    foreach (var mat in rend.materials)
                    {
                        if (mat.HasProperty("_Color"))
                        {
                            mat.color = c;
                        }
                        else if (mat.HasProperty("_TintColor"))
                        {
                            mat.SetColor("_TintColor", c);
                        }
                    }
                }
            }




        }
        public class ColorCycler4 : MonoBehaviour
        {
            private Renderer rend;

            void Start()
            {
                rend = GetComponentInChildren<Renderer>();
            }

            void Update()
            {
                if (rend != null)
                {
                    float t = Mathf.PingPong(Time.time, 9999f);
                    Color c = Color.Lerp(Color.magenta, Color.magenta, t);

                    foreach (var mat in rend.materials)
                    {
                        if (mat.HasProperty("_Color"))
                        {
                            mat.color = c;
                        }
                        else if (mat.HasProperty("_TintColor"))
                        {
                            mat.SetColor("_TintColor", c);
                        }
                    }
                }
            }




        }

        public class ColorCycler5 : MonoBehaviour
        {
            private Renderer rend;

            void Start()
            {
                rend = GetComponentInChildren<Renderer>();
            }

            void Update()
            {
                if (rend != null)
                {
                    float t = Mathf.PingPong(Time.time, 9999f);
                    Color c = Color.Lerp(Color.green, Color.green, t);

                    foreach (var mat in rend.materials)
                    {
                        if (mat.HasProperty("_Color"))
                        {
                            mat.color = c;
                        }
                        else if (mat.HasProperty("_TintColor"))
                        {
                            mat.SetColor("_TintColor", c);
                        }
                    }
                }
            }

            internal const string AtherBiome = "AtherVoid";

        }
    }
}

