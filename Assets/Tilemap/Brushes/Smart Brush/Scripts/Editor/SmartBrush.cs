using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomGridBrush(false, true, true, "Smart Brush")]
public class SmartBrush : GridBrush
{
    private Tilemap _targetTilemap;

    private readonly List<string> _blocksList = new List<string>
    {
        "Metal", "Spike", "Spike-Down", "Spike-Left", "Spike-Right"
    };

    private readonly List<string> _bridgesList = new List<string>
    {
        "Bridge"
    };

    private readonly List<string> _objectsList = new List<string>
    {
        "Arrow-Down", "Arrow-Left", "Arrow-Right", "Arrow-Up", "CheckPoint", "Coint-1", "Coint-5", "Coint-10", "End",
        "Key-Blue", "Key-Red", "Key-Yellow", "Lock-Blue", "Lock-Red", "Lock-Yellow"
    };

    public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pickStart)
    {
        base.Pick(gridLayout, brushTarget, position, pickStart);
        var currentTile = cells[0].tile;
        if (currentTile != null)
        {
            var tilemaps = FindObjectsOfType<Tilemap>().ToList();
            var tilemapName = GetCorrespondingTilemapName(currentTile.name);
            _targetTilemap = tilemaps.Find(x => x.name.Equals(tilemapName));
            Selection.activeObject = _targetTilemap != null ? _targetTilemap.gameObject : null;
        }
    }

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if (brushTarget != null)
        {
            Undo.RegisterCompleteObjectUndo(_targetTilemap, string.Empty);
            base.Paint(gridLayout, brushTarget, position);
        }
    }

    public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
        if (brushTarget != null)
        {
            Undo.RegisterCompleteObjectUndo(_targetTilemap, string.Empty);
            base.BoxFill(gridLayout, brushTarget, position);
        }
    }

    public override void FloodFill(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if (brushTarget != null)
        {
            Undo.RegisterCompleteObjectUndo(_targetTilemap, string.Empty);
            base.FloodFill(gridLayout, brushTarget, position);
        }
    }

    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if (brushTarget != null)
        {
            Undo.RegisterCompleteObjectUndo(_targetTilemap, string.Empty);
            base.Erase(gridLayout, brushTarget, position);
        }
    }


    public override void BoxErase(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
        if (brushTarget != null)
        {
            Undo.RegisterCompleteObjectUndo(_targetTilemap, string.Empty);
            base.BoxErase(gridLayout, brushTarget, position);
        }
    }

    // Here i get the corresponding tilemap name.
    private string GetCorrespondingTilemapName(string tileName)
    {
        if (_blocksList.Contains(tileName))
        {
            return "Blocks";
        }

        if (_bridgesList.Contains(tileName))
        {
            return "Bridges";
        }

        if (_objectsList.Contains(tileName))
        {
            return "Objects";
        }

        return string.Empty;
    }
}