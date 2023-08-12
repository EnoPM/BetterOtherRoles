using UnityEngine;

namespace BetterOtherRoles.Utilities.Extensions;

public static class PlayerControlExtensions
{
    private static readonly int Outline = Shader.PropertyToID("_Outline");
    private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
    
    public static void SetOutline(this PlayerControl pc, float size, Color color)
    {
        if (!pc.cosmetics) return;
        var material = pc.cosmetics.currentBodySprite.BodySprite.material;
        material.SetFloat(Outline, size);
        material.SetColor(OutlineColor, color);
    }
}