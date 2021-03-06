﻿using Assets.Scripts.Models;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Unity.Towers.Mods;
using Harmony;
using NKHook6.Api.Events;
using NKHook6.Api.Events._Bloons;

namespace NKHook6.Patches._Bloons
{
    [HarmonyPatch(typeof(Bloon), "UpdatedModel")]
    class UpdatedModelHook
    {
        private static bool sendPrefixEvent = true;
        private static bool sendPostfixEvent = true;

        [HarmonyPrefix]
        internal static bool Prefix(ref Bloon __instance, ref Model modelToUse)
        {
            bool allowOriginalMethod = true;
            if (sendPrefixEvent)
            {
                var o = new BloonEvents.UpdatedModelEvent.Pre(ref __instance, ref modelToUse);
                EventRegistry.instance.dispatchEvent(ref o);
                __instance = o.instance;
                modelToUse = o.model;
                allowOriginalMethod = !o.isCancelled();
            }

            sendPrefixEvent = !sendPrefixEvent;

            return allowOriginalMethod;
        }

        [HarmonyPostfix]
        internal static void Postfix(ref Bloon __instance, ref Model modelToUse)
        {
            if (sendPostfixEvent)
            {
                var o = new BloonEvents.UpdatedModelEvent.Post(ref __instance, ref modelToUse);
                EventRegistry.instance.dispatchEvent(ref o);
                __instance = o.instance;
                modelToUse = o.model;
            }

            sendPostfixEvent = !sendPostfixEvent;
        }
    }
}
