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

                var fmodAsset2 = Nautilus.Utility.AudioUtils.GetFmodAsset("BloopersViolet");
                var fmodAsset3 = Nautilus.Utility.AudioUtils.GetFmodAsset("BloopersHavok");

                // early acey (yes i misspelled that on purpous...)
                PDAHandler.AddLogEntry(
                            "Bloopers",
                            "Bloopers",
                           AV.pdavoicelinereg.AetherVoidBiomePDA.placeholdervoiceline
                        );

                LanguageHandler.SetLanguageLine("EncyPath_DownloadedData/Precursor", "TestingFacilitys");

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

                PDAHandler.AddEncyclopediaEntry(
                        "IonPolymerfacilitylogsency",
                        "DownloadedData/Precursor/TestingFaciltys",
                        "Precursor Facility Audit Log – Ion Polymer Testing Site",
                        "Automated System Output\r\n\r\n9:34 – Geological Event Detected\r\nExternal Event Detected Underwater landslide registered on external sensors. Impact confirmed against lower facility structure. Integrity scan initiated.\r\n\r\n9:43 – Structural Status\r\nExternal Event Ended Minor-to-moderate internal damage reported in affected sectors. Core systems remain stable. No oxygen-field deviation detected.\r\n\r\n9:47 – Power Assessment\r\nPrimary power grid stable. Localized fluctuations noted in impacted areas. Nonessential modules placed in standby.\r\n\r\n9:50 - Affected Areas\r\nIon Polymer Main Production Testing Area Offline Detecting Water Leakage.\r\n\r\n9:51 - Prevention\r\nActivating water drainage system, Activating first water lock area seal, First area seal failed activating secondary emergency seal, Emergency seal success\r\n\r\n9:53 - Lockdown\r\nFacility drained locking down facility main entrance, Awaiting facility High Authority key\r\n\r\n9:57 – Access Artifact Detection\r\nHigh Facility Authority key identified within 300 m of main structure. Attempting to use tracking beacon.\r\n\r\n9:59 – Perimeter Scan\r\nUnable to use primary system scanning or tracking beacon reverting to secondary scanning systems. Possible biological movement detected near High Authority key coordinates. Caution advisory issued.\r\n\r\n10:03 – System Advisory\r\nMonitoring cycle extended. Full facility damage unknown triggering emergency evacuation until further notice. Facility Message Broadcasting Active. Standing by for key Input\r\n\r\n3:34 - High Authority Facility Inserted\r\nResumed after 28 years. Facility lock down lifted evacuation canceled. Sending Audit Log To Entrance Lower Section Terminal.",
                        null, null, PDAHandler.UnlockImportant,null
                    );

                LanguageHandler.SetLanguageLine(
                    "Bloopers",
                    "Hey there, thank you for playing Aether Void. This is of course the early access version. If you guys love this mod, when I mean you guys, I mean you, Riley Robinson, whoever you are controlling him, thank you for playing my mod. What I'm about to send to your PDA is a blooper log. Pretty much it contains, for each one of my friends, it contains all of our bloopers that we've done, that way you guys can listen to them and laugh along with us. However, I will warn you that my friend is not the most family-friendly one, but I'm still gonna contain it in there, because why not? It's funny. Anyways, enjoy- I was trying to say enjoy, by the way, just to clarify, but... Huh. You know, um, speaking of which, where even am I? Help! Help me! Somebody! Well, I'll just leave it to it.",
                    "English"
                );

                StoryGoalHandler.RegisterCustomEvent("IonPolymerfacilitylogsency", () =>
                {
                    PDAEncyclopedia.Add("IonPolymerfacilitylogsency",true);

                });

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
