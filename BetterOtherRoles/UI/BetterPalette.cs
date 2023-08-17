using UnityEngine;

namespace BetterOtherRoles.UI;

public class BetterPalette
{
    public static readonly Color Primary = new(0.185f, 0.192f, 0.211f);
    public static readonly Color Secondary = new(0.725f, 0.733f, 0.745f);
    public static readonly Color Dark = new(0.145f, 0.145f, 0.160f);
    public static readonly Color Black = new(0f, 0f, 0f, 0.5f);
    public static readonly Color Gray = new(0.250f, 0.267f, 0.294f);
    public static readonly Color Success = new(0.027f, 0.361f, 0.062f);
    public static readonly Color Warning = new(0.980f, 0.435f, 0f);
    public static readonly Color Danger = new(0.439f, 0.067f, 0.027f);
    public static Color LightDanger => Danger.SetAlpha(0.7f);
    public static readonly Color Info = new(0.258f, 0.529f, 0.961f);
    public static readonly Color Twitch = new(0.568f, 0.274f, 1f);
    public static readonly Color Transparent = new(0f, 0f, 0f, 0f);
    public static Color RainbowColor => Color.HSVToRGB(Mathf.PingPong(Time.time * 0.30f, 1), 1, 1);
}