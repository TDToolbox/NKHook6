﻿using Assets.Scripts.Models;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Objects;
using Harmony;
using NKHook6.Api.Events;
using NKHook6.Api.Events._Bloons;

namespace NKHook6.Patches._Bloons
{
    [HarmonyPatch(typeof(Bloon), "Initialise")]
    class InitialiseHook
    {
        [HarmonyPostfix]
        internal static void Postfix(ref Bloon __instance, ref Entity target, ref Model modelToUse)
        {
            var o = new BloonEvents.CreatedEvent(ref __instance, ref target, ref modelToUse); //Create CreatedEvent instance
            EventRegistry.instance.dispatchEvent(ref o); //Dispatch it
            target = o.entity; //Get the changed values and modify them
            modelToUse = o.model; //Same here
        }
    }
}
