using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable; // 지나갈 수 있는지 체크
    }
    private Tilemap tilemap;
    private CellData[,] BoardData;
    private Grid grid;

    public PlayerCtrl playerCtrl;
    public int width;
    public int height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    // Start is called before the first frame update
    public void Init()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        grid = GetComponentInChildren<Grid>();

        // width, height 전체 배열에 대한 데이터를 초기화
        BoardData = new CellData[width, height];
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Tile tile;

                // x * y 좌표의 각 어레이 celldata 설정
                BoardData[x, y] = new CellData();
                if(x == 0 || y == 0 || x == width - 1 || y == width - 1)
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    BoardData[x, y].Passable = true;
                }
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 CellToWorld(Vector2Int cellIdx)
    {
        return grid.GetCellCenterWorld((Vector3Int)cellIdx);
    }

    public CellData GetCellData(Vector2Int cellIdx)
    {
        // 이동 불가능 셀 예외 처리
        if(cellIdx.x < 0 || cellIdx.x >= width || cellIdx.y < 0 || cellIdx.y >= height)
        {
            return null;
        }

        // 이동 가능할 경우 Data 제공
        return BoardData[cellIdx.x, cellIdx.y];
    }
}
