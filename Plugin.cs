using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Handlers.TitleScreen;
using Nautilus.Utility;
using System;
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
    [BepInDependency("com.prototech.prototypesub", BepInDependency.DependencyFlags.SoftDependency)]
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

        public static bool useknifebundle = true;


        internal static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public static string AssetsFolderPath { get; } = Path.Combine(Path.GetDirectoryName(Assembly.Location), "Assets");
        public static AssetBundle AudioBundle { get; private set; }
        public static AssetBundle SubBundle { get; private set; }
        public static AssetBundle MeshBundle { get; private set; }
        public static AssetBundle ItemMeshBundle { get; private set; }
        public static AssetBundle KnifeMeshBundle { get; private set; }
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
 

            string audioBundlePath = Path.Combine(AssetsFolderPath, "audio");
            AudioBundle = AssetBundle.LoadFromFile(audioBundlePath);
            if (AudioBundle == null)
            {
                Logger.LogError($"Failed to load AudioBundle from {audioBundlePath}");
                return;
            }
            foreach (var assetName in AudioBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"AudioBundle contains: {assetName}");
            }

            string meshBundlePath = Path.Combine(AssetsFolderPath, "mesh");
            MeshBundle = AssetBundle.LoadFromFile(meshBundlePath);
            if (MeshBundle == null)
            {
                Logger.LogError($"Failed to load MeshBundle from {meshBundlePath}");
                return;
            }
            foreach (var assetName in MeshBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"MeshBundle contains: {assetName}");
            }

            string SubBundlePath = Path.Combine(AssetsFolderPath, "aethersub");
            SubBundle = AssetBundle.LoadFromFile(SubBundlePath);
            if (SubBundle == null)
            {
                Logger.LogError($"Failed to load SubBundle from {SubBundlePath}");
                return;
            }
            foreach (var assetName in SubBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"SubBundle contains: {assetName}");
            }

            string TextureBundlePath = Path.Combine(AssetsFolderPath, "texture");
            TextureBundle = AssetBundle.LoadFromFile(TextureBundlePath);
            if (TextureBundle == null)
            {
                Logger.LogError($"Failed to load TextureBundle from {TextureBundlePath}");
                return;
            }
            foreach (var assetName in TextureBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"TextureBundle contains: {assetName}");
            }

            string ItemMeshBundlePath = Path.Combine(AssetsFolderPath, "item mesh");
            ItemMeshBundle = AssetBundle.LoadFromFile(ItemMeshBundlePath);
            if (ItemMeshBundle == null)
            {
                Logger.LogError($"Failed to load ItemMeshBundle from {ItemMeshBundlePath}");
                return;
            }
            foreach (var assetName in ItemMeshBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"KnifeMeshBundle contains: {assetName}");
            }
            string KnifeMeshBundlePath = Path.Combine(AssetsFolderPath, "knife mesh");
            KnifeMeshBundle = AssetBundle.LoadFromFile(KnifeMeshBundlePath);
            if (KnifeMeshBundle == null)
            {
                Logger.LogError($"Failed to load KnifeMeshBundle from {KnifeMeshBundlePath}");
                return;
            }
            foreach (var assetName in KnifeMeshBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"ItemMeshBundle contains: {assetName}");
            }

            // Anti Piracy

            TryFindPiracy();






            //register PDA voice lines
            pdavoicelinereg.AetherVoidBiomePDA.Register();

            // Create and register biome spawning coroutine
            Biomereg.RegisterCustomBiome();
            Biomereg.RegisterCustomBiome2();
            Biomereg.RegisterCustomBiome3();
            StartCoroutine(subreg.RegisterSubmarine());
            FacilitySpawner.Patch();
            Items.Registeritem1();
            Items.Registeritem2();
            Violet.AV.CrystalKnifeClass.CrystalKnife.Register();
            var spawner = new spawnbiomes();
            spawner.StartSpawnCoroutine(this);



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
            var sound = AudioUtils.CreateSound(AudioBundle.LoadAsset<AudioClip>("Into-the-Aether"), AudioUtils.StandardSoundModes_Stream);

            CustomSoundHandler.RegisterCustomSound(soundId, sound, AudioUtils.BusPaths.Music);

            return new MusicTitleAddon(AudioUtils.GetFmodAsset(soundId));


        }

            public static readonly HashSet<string> PiracyFiles = new HashSet<string> { "steam_api64.cdx", "steam_api64.ini", "steam_emu.ini", "valve.ini", "chuj.cdx", "SteamUserID.cfg", "Achievements.bin", "steam_settings", "user_steam_id.txt", "account_name.txt", "ScreamAPI.dll", "ScreamAPI32.dll", "ScreamAPI64.dll", "SmokeAPI.dll", "SmokeAPI32.dll", "SmokeAPI64.dll", "Free Steam Games Pre-installed for PC.url", "Torrent-Igruha.Org.URL", "oalinst.exe", };



        GameObject SpawnObject()
        {
            var prefab = MeshBundle.LoadAsset<GameObject>("Aether");
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
            src.clip = AudioBundle.LoadAsset<AudioClip>("Into-the-Aether");
            src.loop = false;
            src.playOnAwake = true;
            src.volume = 0.1f;

            var mixer = AudioBundle.LoadAsset<UnityEngine.Audio.AudioMixer>("assets/test scripts/mutedmixer.mixer");
            if (mixer != null)
            {
                var groups = mixer.FindMatchingGroups("");
                AudioMixerGroup group = groups.Length > 0 ? groups[0] : null;

                if (group != null)
                {
                    src.outputAudioMixerGroup = group;

                    bool paramExists = group.audioMixer.GetFloat("VisualizerVolume", out _);
                    if (paramExists)
                        group.audioMixer.SetFloat("VisualizerVolume", -80f);
                    else
                        UnityEngine.Debug.LogWarning("VisualizerVolume parameter not found in mixer. Audio will be audible.");
                }
                else
                {
                    UnityEngine.Debug.LogError("No groups found in mixer!");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Mixer not loaded!");
            }

            var changer = obj.AddComponent<AudioTwoColorChanger>();
            changer.targetRenderer = rend;
            changer.quietColor = Color.magenta;
            changer.loudColor = Color.cyan;
            changer.sensitivity = 5500f;
            changer.smoothSpeed = 15f;

            return obj;
        }

        GameObject SpawnObject1()
        {
            var prefab = MeshBundle.LoadAsset<GameObject>("Void");
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
            src.clip = AudioBundle.LoadAsset<AudioClip>("Into-the-Aether");
            src.loop = false;
            src.playOnAwake = true;
            src.volume = 0.1f;

            var mixer = AudioBundle.LoadAsset<UnityEngine.Audio.AudioMixer>("assets/test scripts/mutedmixer.mixer");
            if (mixer != null)
            {
                var groups = mixer.FindMatchingGroups("");
                AudioMixerGroup group = groups.Length > 0 ? groups[0] : null;

                if (group != null)
                {
                    src.outputAudioMixerGroup = group;

                    bool paramExists = group.audioMixer.GetFloat("VisualizerVolume", out _);
                    if (paramExists)
                        group.audioMixer.SetFloat("VisualizerVolume", -80f);
                    else
                        UnityEngine.Debug.LogWarning("VisualizerVolume parameter not found in mixer. Audio will be audible.");
                }
                else
                {
                    UnityEngine.Debug.LogError("No groups found in mixer!");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Mixer not loaded!");
            }

            var changer = obj.AddComponent<AudioTwoColorChanger>();
            changer.targetRenderer = rend;
            changer.quietColor = Color.cyan;
            changer.loudColor = Color.magenta;
            changer.sensitivity = 5500f;
            changer.smoothSpeed = 15f;

            return obj;

        }

        GameObject SpawnObject2()
        {
            var prefab = MeshBundle.LoadAsset<GameObject>("thankyoulist");
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
            var prefab = MeshBundle.LoadAsset<GameObject>("goober");
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
            var prefab = MeshBundle.LoadAsset<GameObject>("plane");
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

            Texture2D myTexture = MeshBundle.LoadAsset<Texture2D>("Screenshot 2025-08-09 205356");

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
            var prefab = MeshBundle.LoadAsset<GameObject>("yippe");
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
            var prefab = MeshBundle.LoadAsset<GameObject>("Crystal (isuckok;_;)");
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
            var prefab = MeshBundle.LoadAsset<GameObject>("Crystal (isuckok;_;)");
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

