using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExfilFlare
{
    [BepInPlugin("com.SkebbZ.ExfilFlare", "ExfilFlare", "1.0.0")]
    public class FlareEventNotifierPlugin : BaseUnityPlugin
    {
        // Create a static logger instance that other classes can access
        public static ManualLogSource Log = null!;

        internal void Awake()
        {
            Log = Logger;

            new FlarePatch().Enable();

            Log.LogInfo("ExfilFlare has loaded and is patching.");
        }
    }

}