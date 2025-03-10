using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile[] obstacleTiles;
    public Tile brokenTile;
    public int maxHealth = 3;
    public AudioClip BreakingClip;
    public AudioClip BreakedClip;

    private int healthPoint;
    private Tile originTile;

    public override void Init(Vector2Int inputCell)
    {
        base.Init(inputCell);

        healthPoint = maxHealth;
        originTile = GameManager.Instance.boardManager.GetCellTile(inputCell);

        for(int i = 0; i < obstacleTiles.Length; i++)
        {
            int tileType = Random.Range(0, obstacleTiles.Length);
            GameManager.Instance.boardManager.SetCellTile(inputCell, obstacleTiles[tileType]);
        }
    }
    
    public override void PlayerEntered()
    {
        base.PlayerEntered();
    }
    public override bool PlayerWantsToEnter()
    {
        healthPoint -= 1;
        GameManager.Instance.PlaySound(BreakingClip);
        if(healthPoint == 1)
        {
            GameManager.Instance.boardManager.SetCellTile(cell, brokenTile);
        }
        if(healthPoint > 0)
        {
            return false;
        }

        GameManager.Instance.boardManager.SetCellTile(cell, originTile);
        GameManager.Instance.PlaySound(BreakedClip);
        Destroy(gameObject);
        return true;
    }
}
