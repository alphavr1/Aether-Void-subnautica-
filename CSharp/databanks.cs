using Nautilus.FMod;
using Nautilus.FMod.Interfaces;
using Nautilus.Handlers;
using Violet.AV;
using AV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Violet.AV
{
    internal class databanks
    {
        public class data
        {
            public static bool isdoneregistering = false;
            public static void Register()
            {
                var bundleSource = new AssetBundleSoundSource(Plugin.AudioBundle);
                FModSoundBuilder builder = new FModSoundBuilder(bundleSource);

                IFModSoundBuilder eventBuilder = builder.CreateNewEvent(
                    "Placeholder",
                    "bus:/master/SFX_for_pause/PDA_pause/all/SFX"
                );

                eventBuilder.SetSound("Placeholder")
                    .SetFadeDuration(0.5f)
                    .SetMode2D(true);

                eventBuilder.Register();


                IFModSoundBuilder eventBuilder2 = builder.CreateNewEvent(
                    "BloopersViolet",
                    "bus:/master/SFX_for_pause/PDA_pause/all/SFX"
                );

                eventBuilder2.SetSound("BloopersViolet")
                    .SetFadeDuration(0.5f)
                    .SetMode2D(true);

                eventBuilder2.Register();


                IFModSoundBuilder eventBuilder3 = builder.CreateNewEvent(
                    "BloopersHavok",
                    "bus:/master/SFX_for_pause/PDA_pause/all/SFX"
                );

                eventBuilder3.SetSound("BloopersHavok")
                    .SetFadeDuration(0.5f)
                    .SetMode2D(true);

                eventBuilder3.Register();


                var fmodAsset = Nautilus.Utility.AudioUtils.GetFmodAsset("Placeholder");
                var fmodAsset2 = Nautilus.Utility.AudioUtils.GetFmodAsset("BloopersViolet");
                var fmodAsset3 = Nautilus.Utility.AudioUtils.GetFmodAsset("BloopersHavok");

                // early acey (yes i misspelled that on purpous...)
                PDAHandler.AddLogEntry(
                            "Bloopers",
                            "Bloopers",
                           fmodAsset
                        );

                PDAHandler.AddEncyclopediaEntry(
                        "Bloopersdata2",
                        "DownloadedData/PublicDocs",
                        "Violet(ProjectManager,Lead CSharp Dev, Voice Actor) Bloopers",
                        "Hello! Welcome To the bloopers! i hope you enjoy how stupid some of these were!",
                        null, null,
                        PDAHandler.UnlockBasic,
                        fmodAsset2
                    );


                PDAHandler.AddEncyclopediaEntry(
                        "Bloopersdata",
                        "DownloadedData/PublicDocs",
                        "Havok/Systemz(Voice Actor) Bloopers",
                        "Hello! Welcome To the bloopers For Havok!!! i hope you enjoy how stupid some of these were! Beaware these do have swares and some slures!!! (ngl understandable though)",
                        null, null,
                        PDAHandler.UnlockImportant,
                        fmodAsset3
                    );

                LanguageHandler.SetLanguageLine(
                    "Bloopers",
                    "Hey there, thank you for playing Aether Void. This is of course the early access version. If you guys love this mod, when I mean you guys, I mean you, Riley Robinson, whoever you are controlling him, thank you for playing my mod. What I'm about to send to your PDA is a blooper log. Pretty much it contains, for each one of my friends, it contains all of our bloopers that we've done, that way you guys can listen to them and laugh along with us. However, I will warn you that my friend is not the most family-friendly one, but I'm still gonna contain it in there, because why not? It's funny. Anyways, enjoy- I was trying to say enjoy, by the way, just to clarify, but... Huh. You know, um, speaking of which, where even am I? Help! Help me! Somebody! Well, I'll just leave it to it.",
                    "English"
                );


                StoryGoalHandler.RegisterBiomeGoal(
                    "Bloopersdata",
                    Story.GoalType.Encyclopedia,
                    biomeName: "safeShallows",
                    minStayDuration: 1f,
                    delay: 0f
                    );
                StoryGoalHandler.RegisterBiomeGoal(
                    "Bloopersdata2",
                    Story.GoalType.Encyclopedia,
                    biomeName: "safeShallows",
                    minStayDuration: 1f,
                    delay: 0f
                    );

                isdoneregistering = true;
            }
        }
    }
}
