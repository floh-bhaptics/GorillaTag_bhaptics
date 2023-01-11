using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MelonLoader;
using HarmonyLib;

using MyBhapticsTactsuit;

[assembly: MelonInfo(typeof(GorillaTag_bhaptics.GorillaTag_bhaptics), "GorillaTag_bhaptics", "2.0.0", "Florian Fahrenberger")]
[assembly: MelonGame("Another Axiom", "Gorilla Tag")]


namespace GorillaTag_bhaptics
{
    public class GorillaTag_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;

        public override void OnInitializeMelon()
        {
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
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
