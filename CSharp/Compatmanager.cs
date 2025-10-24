using BepInEx.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Violet.AV
{
    internal class Compatmanager
    {
        internal static class CompatManager
        {
            private static bool _checkedAL;
            public static bool _ALInstalled;

            public static bool AL
            {
                get
                {
                    if (!_checkedAL)
                    {
                            

                        _ALInstalled = Chainloader.PluginInfos.ContainsKey("com.aotu.architectslibrary");
                        _checkedAL = true;
                    }
                    return _ALInstalled;
                }
            }
        }
    }
}

