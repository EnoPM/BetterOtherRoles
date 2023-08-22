namespace BetterOtherRoles.Modules;

public static class CustomRegions
{
    public static readonly IRegionInfo[] DefaultRegions = {
        new StaticHttpRegionInfo("North America",
            StringNames.ServerNA,
            "matchmaker.among.us",
            new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1]
            {
                new("Http-1", "https://matchmaker.among.us", 443, false)
            })).CastFast<IRegionInfo>(),
        new StaticHttpRegionInfo("Europe",
            StringNames.ServerEU,
            "matchmaker-eu.among.us",
            new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1]
            {
                new("Http-1", "https://matchmaker-eu.among.us", 443, false)
            })).CastFast<IRegionInfo>(),
        new StaticHttpRegionInfo("Asia",
            StringNames.ServerAS,
            "matchmaker-as.among.us",
            new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1]
            {
                new("Http-1", "https://matchmaker-as.among.us", 443, false)
            })).CastFast<IRegionInfo>(),
        new StaticHttpRegionInfo("<color=#2CC71EFF>MODDED [NA]</color>",
            StringNames.NoTranslation,
            "www.aumods.xyz",
            new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1]
            {
                new("Http-1", "https://www.aumods.xyz", 443, false)
            })).CastFast<IRegionInfo>(),
        new StaticHttpRegionInfo("<color=#DE0D68FF>MODDED [EU]</color>",
            StringNames.NoTranslation,
            "https://au-eu.duikbo.at",
            new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1]
            {
                new("Http-1", "https://au-eu.duikbo.at", 443, false)
            })).CastFast<IRegionInfo>(),
        new StaticHttpRegionInfo("<color=#C7B816FF>MODDED [AS]</color>",
            StringNames.NoTranslation,
            "https://au-as.duikbo.at",
            new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1]
            {
                new("Http-1", "https://au-as.duikbo.at", 443, false)
            })).CastFast<IRegionInfo>(),
    };
}