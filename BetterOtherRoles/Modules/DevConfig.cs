using System;
using System.Reflection;

namespace BetterOtherRoles.Modules;

public static class DevConfig
{
    public static bool DisableEndGameConditions { get; set; }
    public static bool DisablePlayerRequirementToLaunch { get; set; }
    public static Guid CurrentGuid { get; set; }

    static DevConfig()
    {
#if RELEASE
        DisableEndGameConditions = false;
        DisablePlayerRequirementToLaunch = false;
        CurrentGuid = Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId;
#else
        DisableEndGameConditions = true;
        DisablePlayerRequirementToLaunch = true;
        CurrentGuid = Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId;
#endif
    }
}