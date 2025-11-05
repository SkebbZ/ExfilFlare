using System.Reflection;
using EFT;
using EFT.Communications;
using EFT.Interactive;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;

namespace ExfilFlare;

public class FlarePatch : ModulePatch
{
    private static PropertyInfo _firedPlayerProperty = null!;

    protected override MethodBase GetTargetMethod()
    {
        var targetType = typeof(FlareShootDetectorZone);
        var targetMethod = AccessTools.Method(targetType, "method_1");

        var flareEventType = targetMethod.GetParameters()[0].ParameterType;

        // This is a Property. We use AccessTools.Property to get it.
        _firedPlayerProperty = AccessTools.Property(flareEventType, "FiredPlayer");

        FlareEventNotifierPlugin.Log.LogInfo("Flare patch target acquired successfully.");
        return targetMethod;
    }

    [PatchPrefix]
    public static void PatchPrefix(object __0) // '__0' is the flareEvent object
    {
        // We get the value from the Property and cast it to a Player object.
        if (_firedPlayerProperty.GetValue(__0) is Player firedPlayer)
        {
            if (firedPlayer.IsYourPlayer)
            {
                FlareEventNotifierPlugin.Log.LogInfo("Successful flare shot by local player detected! Displaying notification.");
                NotificationManagerClass.DisplayNotification(new ExfilFlareSuccessNotification());
            }
        }
    }
}

public class ExfilFlareSuccessNotification : NotificationAbstractClass
{
    public ExfilFlareSuccessNotification()
    {
        Duration = ENotificationDurationType.Long;
    }

    public override string Description => "Exfil activated";
    public override ENotificationIconType Icon => ENotificationIconType.Default;
    public override Color? TextColor => Color.green;
}