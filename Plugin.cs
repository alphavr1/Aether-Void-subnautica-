using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Handlers.TitleScreen;
using Nautilus.Utility;
using SubLibrary.Handlers;
using SubLibrary.Monobehaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using Testbiome;
using Violet.Testbiome;

namespace Testbiome
{
    [BepInPlugin(GUID, pluginName, versionString)]
    [BepInDependency("com.snmodding.nautilus",BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("Esper89.TerrainPatcher")]
    [BepInDependency("com.prototech.prototypesub", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.indigocoder.sublibrary",BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "com.Ather.test";
        private const string pluginName = "test";
        private const string versionString = "1.0.0";

        const string soundId = "AtherVoidMusic";

        private static readonly Harmony Harmony = new Harmony(GUID);
        public static ManualLogSource Log;


        internal static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public static string AssetsFolderPath { get; } = Path.Combine(Path.GetDirectoryName(Assembly.Location), "Assets");
        public static AssetBundle AudioBundle { get; private set; }
        public static AssetBundle SubBundle { get; private set; }
        public static AssetBundle MeshBundle { get; private set; }
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

            // Basic plugin info log
            Logger.LogInfo($"{pluginName} {versionString} loaded.");
            Log = Logger;
 

            // Load Asset Bundles 
            string audioBundlePath = Path.Combine(AssetsFolderPath, "audio");
            AudioBundle = AssetBundle.LoadFromFile(audioBundlePath);
            if (AudioBundle == null)
            {
                Logger.LogError($"Failed to load AudioBundle from {audioBundlePath}");
                return; // Stop if bundle load fails
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
                return; // Stop if bundle load fails
            }
            foreach (var assetName in MeshBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"MeshBundle contains: {assetName}");
            }

            string SubBundlePath = Path.Combine(AssetsFolderPath, "sub");
            SubBundle = AssetBundle.LoadFromFile(SubBundlePath);
            if (SubBundle == null)
            {
                Logger.LogError($"Failed to load SubBundle from {SubBundlePath}");
                return; // Stop if bundle load fails
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
                return; // Stop if bundle load fails
            }
            foreach (var assetName in TextureBundle.GetAllAssetNames())
            {
                Logger.LogInfo($"TextureBundle contains: {assetName}");
            }


            // Fmod stuff



            //register PDA voice lines
            pdavoicelinereg.AetherVoidBiomePDA.Register();

            // Create and register biome spawning coroutine
            CustomBiome.RegisterCustomBiome();
            CustomBiome.RegisterCustomBiome2();
            var spawner = new SpawnBiomes();
            spawner.StartSpawnCoroutine(this);

            // Register sky prefab fix (example, ensure this doesn't depend on bundles if it uses them)
            var skyPrefabFixer = BiomeUtils.CreateSkyPrefab("SkyDeepGrandReef", null, true, true);
            Texture specularCubeTexture = skyPrefabFixer.specularCube;

            // Register prefabs and addons
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

            // Scene Logger
            var loggerObj = new GameObject("SceneLogger");
            loggerObj.AddComponent<SceneObjectLogger>().logEveryFrame = false;
            DontDestroyOnLoad(loggerObj);
        }

        private MusicTitleAddon GetMusicAddon()
        {
            // Creates a sound using the standard modes for a "streamed" sound, loading the audio clip by name from the Asset Bundle
            var sound = AudioUtils.CreateSound(AudioBundle.LoadAsset<AudioClip>("Into-the-Aether"), AudioUtils.StandardSoundModes_Stream);

            // Register the sound under the Music bus
            CustomSoundHandler.RegisterCustomSound(soundId, sound, AudioUtils.BusPaths.Music);

            return new MusicTitleAddon(AudioUtils.GetFmodAsset(soundId));


        }

            public static bool piracytestenabled = true;
            public static readonly HashSet<string> PiracyFiles = new HashSet<string> { "steam_api64.cdx", "steam_api64.ini", "steam_emu.ini", "valve.ini", "chuj.cdx", "SteamUserID.cfg", "Achievements.bin", "steam_settings", "user_steam_id.txt", "account_name.txt", "ScreamAPI.dll", "ScreamAPI32.dll", "ScreamAPI64.dll", "SmokeAPI.dll", "SmokeAPI32.dll", "SmokeAPI64.dll", "Free Steam Games Pre-installed for PC.url", "Torrent-Igruha.Org.URL", "oalinst.exe", };

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

            // Apply Subnautica shaders AFTER setting up renderers
            MaterialUtils.ApplySNShaders(obj);

            var renderers = obj.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                var mats = r.materials;
                for (int i = 0; i < mats.Length; i++)
                    mats[i] = new Material(mats[i]);
                r.materials = mats;
            }

            // Grab renderer for color changer
            var rend = obj.GetComponentInChildren<Renderer>();

            // AudioSource setup
            var src = obj.GetComponent<AudioSource>() ?? obj.AddComponent<AudioSource>();
            src.clip = AudioBundle.LoadAsset<AudioClip>("Into-the-Aether");
            src.loop = false;
            src.playOnAwake = true; // automatically play on spawn
            src.volume = 0.1f;         // keep volume at 1 so FFT works

            // Load mixer and assign group
            var mixer = AudioBundle.LoadAsset<UnityEngine.Audio.AudioMixer>("assets/test scripts/mutedmixer.mixer");
            if (mixer != null)
            {
                var groups = mixer.FindMatchingGroups("");
                AudioMixerGroup group = groups.Length > 0 ? groups[0] : null;

                if (group != null)
                {
                    src.outputAudioMixerGroup = group;

                    // Mute via exposed parameter
                    // Make sure "VisualizerVolume" is exposed in Unity
                    bool paramExists = group.audioMixer.GetFloat("VisualizerVolume", out _);
                    if (paramExists)
                        group.audioMixer.SetFloat("VisualizerVolume", -80f); // mute
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

            // Add color changer
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


            // Add & configure SkyApplier properly
            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            // Apply Subnautica shaders AFTER setting up renderers
            MaterialUtils.ApplySNShaders(obj);

            var renderers = obj.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                var mats = r.materials;
                for (int i = 0; i < mats.Length; i++)
                    mats[i] = new Material(mats[i]);
                r.materials = mats;
            }

            // Grab renderer for color changer
            var rend = obj.GetComponentInChildren<Renderer>();

            // AudioSource setup
            var src = obj.GetComponent<AudioSource>() ?? obj.AddComponent<AudioSource>();
            src.clip = AudioBundle.LoadAsset<AudioClip>("Into-the-Aether");
            src.loop = false;
            src.playOnAwake = true; // automatically play on spawn
            src.volume = 0.1f;         // keep volume at 1 so FFT works

            // Load mixer and assign group
            var mixer = AudioBundle.LoadAsset<UnityEngine.Audio.AudioMixer>("assets/test scripts/mutedmixer.mixer");
            if (mixer != null)
            {
                var groups = mixer.FindMatchingGroups("");
                AudioMixerGroup group = groups.Length > 0 ? groups[0] : null;

                if (group != null)
                {
                    src.outputAudioMixerGroup = group;

                    // Mute via exposed parameter
                    // Make sure "VisualizerVolume" is exposed in Unity
                    bool paramExists = group.audioMixer.GetFloat("VisualizerVolume", out _);
                    if (paramExists)
                        group.audioMixer.SetFloat("VisualizerVolume", -80f); // mute
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

            // Add color changer
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


            // Add & configure SkyApplier properly
            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            // Apply Subnautica shaders AFTER setting up renderers
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


            // Add & configure SkyApplier properly
            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            // Apply Subnautica shaders AFTER setting up renderers
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


            // Add & configure SkyApplier properly
            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            // Apply Subnautica shaders AFTER setting up renderers
            MaterialUtils.ApplySNShaders(obj);

            Texture2D myTexture = MeshBundle.LoadAsset<Texture2D>("Screenshot 2025-08-09 205356");

            if (myTexture != null)
            {
                // Get all renderers to apply texture to their materials
                var renderers = obj.GetComponentsInChildren<Renderer>(true);

                foreach (var renderer in renderers)
                {
                    // Assign texture to the main texture slot of the material
                    // You can also create a new material if you want
                    foreach (var mat in renderer.materials)
                    {
                        mat.mainTexture = myTexture;
                    }
                }
            }
            else
            {
                Log.LogError("Texture 'Screenshot 2025-08-09 205356' not found!");
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


            // Add & configure SkyApplier properly
            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            // Apply Subnautica shaders AFTER setting up renderers
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


            // Add & configure SkyApplier properly
            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            // Apply Subnautica shaders AFTER setting up renderers
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


            // Add & configure SkyApplier properly
            var sky = obj.GetComponent<SkyApplier>();
            if (sky == null)
                sky = obj.AddComponent<SkyApplier>();

            sky.renderers = obj.GetComponentsInChildren<Renderer>(true);
            sky.anchorSky = Skies.Auto;

            // Apply Subnautica shaders AFTER setting up renderers
            MaterialUtils.ApplySNShaders(obj);

            obj.AddComponent<ColorCycler5>();

            return obj;

        }

        private static IEnumerator GetSubPrefab(IOut<GameObject> prefabOut)
        {
            // Load your model prefab from AssetBundle
            GameObject model = Plugin.SubBundle.LoadAsset<GameObject>("place holder vehicle 1");

            model.SetActive(false);
            GameObject prefab = GameObject.Instantiate(model);

            // 1. Wait for shaders to be ready
            yield return new WaitUntil(() => MaterialUtils.IsReady);

            // 2. Apply Subnautica shaders
            MaterialUtils.ApplySNShaders(prefab);

            // 3. Ensure Cyclops reference is retrieved (needed for copying Cyclops systems)
            yield return CyclopsReferenceHandler.EnsureCyclopsReference();

            // 4. Invoke all CyclopsReferencer components in your prefab
            yield return InterfaceCallerHandler.InvokeCyclopsReferencers(prefab);

            // 5. Run PrefabModifiers
            foreach (var modifier in prefab.GetComponentsInChildren<PrefabModifier>(true))
            {
                modifier.OnAsyncPrefabTasksCompleted();
                modifier.OnLateMaterialOperation(); // MUST be after SN shaders applied
            }

            prefabOut.Set(prefab);
        }

        [RequireComponent(typeof(AudioSource))]
        class AudioTwoColorChanger : MonoBehaviour
        {
            public Renderer targetRenderer;
            public string colorProperty = "_Color";

            // Forced two colors
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
                // Get AudioSource used for mixer capture
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

                // Capture audio spectrum
                captureSource.GetSpectrumData(spectrumData, 0, fftWindow);

                // Calculate average loudness
                float average = 0f;
                for (int i = 0; i < spectrumData.Length; i++)
                {
                    average += spectrumData[i];
                }
                average /= spectrumData.Length;

                // Scale intensity
                float targetIntensity = Mathf.Clamp01(average * sensitivity);

                // Smooth intensity changes
                smoothedIntensity = Mathf.Lerp(smoothedIntensity, targetIntensity, Time.deltaTime * smoothSpeed);

                // Blend between quiet and loud colors
                Color targetColor = Color.Lerp(quietColor, loudColor, smoothedIntensity);

                // Apply color to shader
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

