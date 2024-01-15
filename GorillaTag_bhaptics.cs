using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

using MyBhapticsTactsuit;


namespace GorillaTag_bhaptics
{
    [BepInPlugin("org.bepinex.plugins.GorillaTag_bhaptics", "Gorilla Tag bhaptics integration", "3.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static TactsuitVR tactsuitVr;
#pragma warning disable CS0109 // Remove unnecessary warning
        internal static new ManualLogSource Log;
#pragma warning restore CS0109

        private void Awake()
        {
            // Make my own logger so it can be accessed from the Tactsuit class
            Log = base.Logger;
            // Plugin startup logic
            Logger.LogMessage("Plugin H3VR_bhaptics is loaded!");
            tactsuitVr = new TactsuitVR();
            // one startup heartbeat so you know the vest works correctly
            tactsuitVr.PlaybackHaptics("HeartBeat");
            // patch all functions
            var harmony = new Harmony("bhaptics.patch.h3vr");
            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(GorillaTagger), "UpdateColor", new Type[] { typeof(float), typeof(float), typeof(float) })]
        public class bhaptics_UpdateColor
        {
            [HarmonyPostfix]
            public static void Postfix(GorillaTagger __instance)
            {
                //tactsuitVr.LOG("UpdateColor: "); 
                tactsuitVr.PlaybackHaptics("ColorChange");
            }
        }

        [HarmonyPatch(typeof(GorillaTagger), "ApplyStatusEffect", new Type[] { typeof(GorillaTagger.StatusEffect), typeof(float) })]
        public class bhaptics_ApplyStatusEffect
        {
            [HarmonyPostfix]
            public static void Postfix(GorillaTagger __instance, GorillaTagger.StatusEffect newStatus, float duration)
            {
                //tactsuitVr.LOG("Status: " + newStatus.ToString() + " " + duration.ToString() + " " + __instance.myVRRig.playerName + " " + __instance.offlineVRRig.playerName);
                //tactsuitVr.LOG("Go on: " + __instance.myVRRig.isMyPlayer.ToString() + " ");
                //if (__instance.currentStatus == newStatus) return;
                tactsuitVr.PlaybackHaptics("ColorChange");
            }
        }

        [HarmonyPatch(typeof(GorillaLocomotion.Player), "IsHandTouching", new Type[] { typeof(bool) })]
        public class bhaptics_HandTap
        {
            [HarmonyPostfix]
            public static void Postfix(bool forLeftHand, bool __result)
            {
                if (!__result) return;
                tactsuitVr.Movement(!forLeftHand);
            }
        }

    }
}
