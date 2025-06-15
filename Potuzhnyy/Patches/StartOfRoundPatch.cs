using HarmonyLib;
using UnityEngine;

namespace Potuzhnyy.Patches
{
    internal static class StartOfRoundPatch
    {
        [HarmonyPatch(typeof(StartOfRound), nameof(Start))]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.High)]
        private static void Start(StartOfRound __instance)
        {
            var cabinet = GameObject.Find("StorageCloset");
            if (cabinet && cabinet.transform.childCount > 1 && cabinet.transform.GetChild(0).childCount > 0 &&
                cabinet.transform.GetChild(1).childCount > 0
                && cabinet.transform.GetChild(0).GetChild(0).TryGetComponent<InteractTrigger>(out _)
                && cabinet.transform.GetChild(1).GetChild(0).TryGetComponent<InteractTrigger>(out _))
            {
                Object.Destroy(cabinet.transform.GetChild(0).gameObject);
                //Object.Destroy(cabinet.transform.GetChild(1).gameObject);
            }
        }
    }
}