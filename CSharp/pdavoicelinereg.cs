using Nautilus.Extensions;
using Nautilus.FMod;
using Nautilus.FMod.Interfaces;
using Nautilus.Handlers;
using AV;
using UnityEngine;

namespace Violet.AV
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

                IFModSoundBuilder eventBuilder = builder.CreateNewEvent(
                    "Warning_Dimension_St_855pdavoiceline",
                    "bus:/master/SFX_for_pause/PDA_pause/all/SFX"
                );

                eventBuilder.SetSound("Warning_Dimension_St_855pdavoiceline")
                    .SetFadeDuration(0.5f)
                    .SetMode2D(false);

                eventBuilder.Register();

                var fmodAsset = Nautilus.Utility.AudioUtils.GetFmodAsset("Warning_Dimension_St_855pdavoiceline");

                IFModSoundBuilder eventBuilder2 = builder.CreateNewEvent(
                    "Placeholder",
                    "bus:/master/SFX_for_pause"
                );

                eventBuilder2.SetSound("Placeholder")
                    .SetFadeDuration(0.5f)
                    .SetMode2D(false);

                eventBuilder2.Register();

                var fmodAssetplaceholder = Nautilus.Utility.AudioUtils.GetFmodAsset("Placeholder");

                IFModSoundBuilder eventBuilder3 = builder.CreateNewEvent(
                    "Finaly a voice actor",
                    "bus:/master/SFX_for_pause"
                );

                eventBuilder3.SetSound("Finaly a voice actor")
                    .SetFadeDuration(0.5f)
                    .SetMode2D(false);

                eventBuilder3.Register();

                var fmodAssetGooberman = Nautilus.Utility.AudioUtils.GetFmodAsset("Finaly a voice actor");




                PDAHandler.AddLogEntry(
                    "AetherVoidWarning",
                    "AetherVoidWarning",
                   fmodAsset
                );

                PDAHandler.AddLogEntry(
                    "AetherVoidgooberman",
                    "AetherVoidgooberman",
                   fmodAssetGooberman,
                   mySprite
                );

                PDAHandler.AddEncyclopediaEntry(
                    "AetherVoidSurfaceData",
                    "PlanetaryGeology",
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
                "AetherVoidSurfaceData",
                Story.GoalType.Encyclopedia,
                biomeName: "AetherVoidsurface",
                minStayDuration: 3f,
                delay: 9f
);



            }
        }
    }
}
