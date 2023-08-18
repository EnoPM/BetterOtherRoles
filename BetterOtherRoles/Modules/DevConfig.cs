using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BepInEx.Configuration;

namespace BetterOtherRoles.Modules;

public static class DevConfig
{
    private static ConfigEntry<string> DingusModeKey { get; set; }
    public static bool DisableEndGameConditions { get; set; }
    public static bool DisablePlayerRequirementToLaunch { get; set; }
    public static Guid CurrentGuid { get; set; }
    public static bool IsDingusRelease { get; set; }

    static DevConfig()
    {
        DingusModeKey = BetterOtherRolesPlugin.Instance.Config.Bind("Special Edition Flags", "Dingus", "Unknown", "Password to unlock Dingus special edition");
        if (DingusModeKey.Value != string.Empty)
        {
            IsDingusRelease = Encrypt(DingusModeKey.Value) == "310fa3717f2b746dc22bb95470ae458e2a07322ee8e631007b670fb34baa8913";
        }
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

    public static string Encrypt(string entry)
    {
        var sb = new StringBuilder();
        var sha = SHA256.Create();
        var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(entry));
        foreach (var b in hashed)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}