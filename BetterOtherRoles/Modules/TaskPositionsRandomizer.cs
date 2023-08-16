using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Utilities.Attributes;
using BetterOtherRoles.Utilities.Extensions;
using UnityEngine;

namespace BetterOtherRoles.Modules;

[Autoload]
public static class TaskPositionsRandomizer
{
    private static CustomOption RandomizeWireTaskPositions => CustomOptionHolder.RandomizeWireTaskPositions;
    private static CustomOption RandomizeUploadTaskPosition => CustomOptionHolder.RandomizeUploadTaskPosition;

    static TaskPositionsRandomizer()
    {
        GameEvents.OnGameStarted += ApplyModule;
    }

    private static void ApplyModule()
    {
        RelocatedWires.Clear();
        RelocatedDownloads.Clear();
        var consoles = ShipStatus.Instance.AllConsoles.ToList();
        if (RandomizeWireTaskPositions.getBool())
        {
            var wires = consoles.FindAll(o => o.name.StartsWith("panel_electrical"));
            if (wires.Count > 0)
            {
                MoveWires(wires);
            }
        }

        if (RandomizeUploadTaskPosition.getBool())
        {
            var upload = consoles.Find(o => o.name == "panel_datahome");
            var downloads = consoles.FindAll(o => o.name == "panel_data");
            if (upload != null && downloads.Count > 0)
            {
                MoveUpload(upload, downloads);
            }
        }

    }

    public static readonly Dictionary<string, SystemTypes> RelocatedWires = new();

    private static void MoveWires(List<Console> wires)
    {
        var positions = wires
            .Select(o => o.transform.position)
            .ToList();
        var sprites = wires
            .Select(o => o.GetComponent<SpriteRenderer>().sprite)
            .ToList();
        var randomizedList = wires
            .OrderBy(_ => BetterOtherRoles.Rnd.Next())
            .ToList();
        for (var i = 0; i < randomizedList.Count; i++)
        {
            randomizedList[i].transform.position = positions[i];
            randomizedList[i].GetComponent<SpriteRenderer>().sprite = sprites[i];
            var pos = randomizedList[i].transform.position;
            RelocatedWires[$"{pos.x}#{pos.y}"] = wires[i].Room;
        }
    }

    public static readonly Dictionary<SystemTypes, SystemTypes> RelocatedDownloads = new();

    private static void MoveUpload(Console upload, List<Console> downloads)
    {
        if (BetterOtherRoles.Rnd.Next(downloads.Count + 1) == 1) return;
        var download = downloads.GetOneRandom();
        var uploadObj = upload.gameObject;
        var downloadObj = download.gameObject;

        var uploadPos = uploadObj.transform.position;
        var downloadPos = downloadObj.transform.position;

        var uploadSprite = uploadObj.GetComponent<SpriteRenderer>().sprite;
        var downloadSprite = downloadObj.GetComponent<SpriteRenderer>().sprite;

        var uploadSize = uploadObj.GetComponent<BoxCollider2D>().size;
        var downloadSize = downloadObj.GetComponent<BoxCollider2D>().size;

        var uploadUsableDistance = upload.UsableDistance;
        var downloadUsableDistance = download.UsableDistance;

        uploadObj.transform.position = downloadPos;
        downloadObj.transform.position = uploadPos;

        uploadObj.GetComponent<SpriteRenderer>().sprite = downloadSprite;
        downloadObj.GetComponent<SpriteRenderer>().sprite = uploadSprite;

        uploadObj.GetComponent<BoxCollider2D>().size = downloadSize;
        downloadObj.GetComponent<BoxCollider2D>().size = uploadSize;
        
        if (upload.onlySameRoom)
        {
            download.checkWalls = true;
        }

        upload.onlySameRoom = false;
        download.onlySameRoom = false;

        upload.usableDistance = downloadUsableDistance;
        download.usableDistance = uploadUsableDistance;

        RelocatedDownloads[upload.Room] = download.Room;
        RelocatedDownloads[download.Room] = upload.Room;
        
        BetterOtherRolesPlugin.Logger.LogMessage($"Upload task are moved in {download.Room}");
    }
}