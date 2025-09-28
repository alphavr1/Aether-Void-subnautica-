using FMOD;
using FMODUnity;
using Nautilus.Extensions;
using Nautilus.FMod;
using Nautilus.FMod.Interfaces;
using Nautilus.Handlers;
using System.Threading.Tasks;
using Testbiome;
using UnityEngine;
using static Nautilus.Utility.AudioUtils;

namespace Violet.Testbiome
{
    internal class pdavoicelinereg
    {
        public class AetherVoidBiomePDA
        {

            public static void Register()
            {
                Sprite mySprite = Plugin.TextureBundle.LoadAsset<Sprite>("Gooberman");
                var bundleSource = new AssetBundleSoundSource(Plugin.AudioBundle);
                FModSoundBuilder builder = new FModSoundBuilder(bundleSource);

                // Create the event
                IFModSoundBuilder eventBuilder = builder.CreateNewEvent(
                    "Warning_Dimension_St_855pdavoiceline",
                    "bus:/master/SFX_for_pause/PDA_pause/all/SFX" // <-- Make sure this bus exists
                );

                eventBuilder.SetSound("Warning_Dimension_St_855pdavoiceline")
                    .SetFadeDuration(0.5f)
                    .SetMode2D(false);

                // Register it so FMOD recognizes it
                eventBuilder.Register();

                var fmodAsset = Nautilus.Utility.AudioUtils.GetFmodAsset("Warning_Dimension_St_855pdavoiceline");

                // Create the event
                IFModSoundBuilder eventBuilder2 = builder.CreateNewEvent(
                    "Placeholder",
                    "bus:/master/SFX_for_pause" // <-- Make sure this bus exists
                );

                eventBuilder.SetSound("Placeholder")
                    .SetFadeDuration(0.5f)
                    .SetMode2D(false);

                // Register it so FMOD recognizes it
                eventBuilder.Register();

                var fmodAssetplaceholder = Nautilus.Utility.AudioUtils.GetFmodAsset("Placeholder");




                PDAHandler.AddLogEntry(
                    "AetherVoidWarning",  // Unique key here
                    "AetherVoidWarning",  // Matching language key
                   fmodAsset
                );

                PDAHandler.AddLogEntry(
                    "AetherVoidgooberman",  // Unique key here
                    "AetherVoidgooberman",  // Matching language key
                   fmodAssetplaceholder,
                   mySprite
                );

                PDAHandler.AddEncyclopediaEntry(
                    "AetherVoidSurfaceData",
                    "PlanetaryGeology",                    // Use this as per docs
                    "Potential Dimension Breach Location South",
                    "This biome seems to be completely cutoff from the Crater Edge, a dimensional breach might be possible with the right equipment",
                    null, null,
                    PDAHandler.UnlockBasic,
                    null
                );

                LanguageHandler.SetLanguageLine(
                    "AetherVoidWarning",
                    "Dimension stability in this region is low. Please take extreme caution.<delay=0>Adding Report To DataBank.<delay=500>",
                    "English"
                );

                LanguageHandler.SetLanguageLine(
                    "AetherVoidgooberman",
                    "H-Hey… is someone there? <delay=0>Okay… someone is there. I can see you on my PDA near the nearby life sign scanner...<delay=600>I’m a part of th—well, what was the Degasi before we… ahm, never mind that! How exactly did you get here? <delay=3000>Not one to respond, I see… Well, anyway, I found something—this world has these strange dimensional instabilities. I’ve been looking for ways to breach them into new worlds or dimensions… as stupid an idea as that might be.<delay=2000>But anyway! I’ve been working on this—it’s… a big prototype...<delay=500>It might work at breaching the dimension in certain spots...<delay=1000> Unfortunately, I don’t know any other spots besides this one here…<delay=1000> Oh! It will require a big power source just to even try to breach it.<delay=1000>Luckily, I found these alien power sources that my PDA calls I-ion cubes…<delay=1000>Am I saying that right?<delay=500>A-anyway, that’s all just to note. I’m not the best at synthesizing blueprints, so it… might possibly explode..<delay=1000>  But you should be fine if you want to use it, that is!",
                    "English"
                );
                Language.main.Exists()?.ParseMetaData();

                StoryGoalHandler.RegisterBiomeGoal(
                    "AetherVoidWarning",
                    Story.GoalType.PDA,
                    biomeName: "AetherVoidsurface",
                    minStayDuration: 3f,
                    delay:1f
                );

                StoryGoalHandler.RegisterBiomeGoal(
                    "AetherVoidgooberman",
                    Story.GoalType.PDA,
                    biomeName: "AetherVoidsurface",
                    minStayDuration: 3f,
                    delay: 10f
                );

                StoryGoalHandler.RegisterBiomeGoal(
                "AetherVoidSurfaceData",   // unique goal key
                Story.GoalType.Encyclopedia,   // this is to unlock PDA databank
                biomeName: "AetherVoidsurface",
                minStayDuration: 3f,
                delay: 9f
);



            }
        }
    }
}
