using HarmonyLib;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace ACUBreedControl.Patches
{
    [HarmonyPatch(typeof(WaterParkCreature))]
    [HarmonyPatch(nameof(WaterParkCreature.ManagedUpdate))]
    public static class WaterParkCreature_Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(BreakableResource __instance)
        {
            
            return false;
        }

        private static void manipulatedOriginal (WaterParkCreature __instance)
        {
            __instance.ManagedUpdate();
            if (__instance.currentWaterPark == null)
            {
                return;
            }
            __instance.UpdateMovement();
            double timePassed = DayNightCycle.main.timePassed;
            if (!__instance.isMature)
            {
                float num = (float)(__instance.matureTime - (double)__instance.data.growingPeriod);
                __instance.age = Mathf.InverseLerp(num, (float)__instance.matureTime, (float)timePassed);
                __instance.transform.localScale = Mathf.Lerp(__instance.data.initialSize, __instance.data.maxSize, __instance.age) * Vector3.one;
                if (__instance.age == 1f)
                {
                    __instance.isMature = true;
                    if (__instance.data.canBreed)
                    {
                        __instance.breedInterval = (double)(__instance.data.growingPeriod * 0.5f);
                        if (__instance.timeNextBreed < 0f)
                        {
                            __instance.ResetBreedTime();
                        }
                    }
                    else
                    {
                        AssetReferenceGameObject adultPrefab = __instance.data.adultPrefab;
                        if (adultPrefab != null && adultPrefab.RuntimeKeyIsValid())
                        {
                            WaterParkCreature.Born(adultPrefab, __instance.currentWaterPark, __instance.transform.position);
                            __instance.SetWaterPark(null);
                            UnityEngine.Object.Destroy(__instance.gameObject);
                            return;
                        }
                    }
                }
            }
            if(Plugin.ICMConfig.Config_BreedActive)
            {
                if (__instance.GetCanBreed() && timePassed > (double)__instance.timeNextBreed)
                {
                    __instance.ResetBreedTime();
                    WaterParkCreature breedingPartner = __instance.currentWaterPark.GetBreedingPartner(__instance);
                    if (breedingPartner != null)
                    {
                        breedingPartner.ResetBreedTime();
                        if (Plugin.ICMConfig.Config_BreedChance <= Random.RandomRangeInt(1, 100))
                        {
                            return;
                        }
                        WaterParkCreature.Born(__instance.data.eggOrChildPrefab, __instance.currentWaterPark, __instance.transform.position + Vector3.down);
                    }
                }
            }
        }
    }
}
