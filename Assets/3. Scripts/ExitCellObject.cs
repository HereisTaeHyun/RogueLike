using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject
{
    public Tile tile;
    public override void Init(Vector2Int inputCell)
    {
        base.Init(inputCell);
        GameManager.Instance.boardManager.SetCellTile(inputCell, tile);
    }

    public override void PlayerEntered()
    {
        Debug.Log("Player Exit");
        GameManager.Instance.NewLevel();
    }
}
