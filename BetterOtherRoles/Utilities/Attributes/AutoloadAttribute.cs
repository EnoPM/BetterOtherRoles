using System;

namespace BetterOtherRoles.Utilities.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AutoloadAttribute : Attribute
{
    public static void Initialize()
    {
        var items = Helpers.GetClassesByAttribute<AutoloadAttribute>(Helpers.AllAssemblies);
        foreach (var item in items)
        {
            if (!item.Type.IsClass || !item.Type.IsAbstract || !item.Type.IsSealed)
            {
                BetterOtherRolesPlugin.Logger.LogWarning($"Unable to load {item.Type.FullName} because is not a static type");
                continue;
            }
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(item.Type.TypeHandle);
        }
    }
}