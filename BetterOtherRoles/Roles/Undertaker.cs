using System.Linq;
using BetterOtherRoles.Players;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Undertaker
{
    public static PlayerControl Player;
    public static readonly Color Color = Palette.ImpostorRed;
    public static DeadBody DraggedBody;
    public static DeadBody TargetBody;
    public static bool CanDropBody;

    public static Sprite DragButtonSprite =>
        Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.DragButton.png", 115f);

    public static Sprite DropButtonSprite =>
        Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.DropButton.png", 115f);

    public static void ClearAndReload()
    {
        Player = null;
        DraggedBody = null;
        TargetBody = null;
    }

    public static void RpcDropBody(Vector3 position)
    {
        if (Player == null) return;
        var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
            (byte)CustomRPC.UndertakerDropBody, Hazel.SendOption.Reliable, -1);
        writer.Write(position.x);
        writer.Write(position.y);
        writer.Write(position.z);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        DropBody(position);
    }

    public static void DropBody(Vector3 position)
    {
        if (!DraggedBody) return;
        DraggedBody.transform.position = position;
        DraggedBody = null;
        TargetBody = null;
    }

    public static void RpcDragBody(byte playerId)
    {
        if (Player == null) return;
        var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
            (byte)CustomRPC.UndertakerDragBody, Hazel.SendOption.Reliable, -1);
        writer.Write(playerId);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        DragBody(playerId);
    }

    public static void DragBody(byte playerId)
    {
        if (Player == null) return;
        var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == playerId);
        if (body == null) return;
        DraggedBody = body;
    }
}