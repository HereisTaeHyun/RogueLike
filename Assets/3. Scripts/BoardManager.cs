using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable; // 지나갈 수 있는지 체크
        public CellObject ContainedObject;
    }
    private Tilemap tilemap;
    private CellData[,] BoardData;
    private List<Vector2Int> EmptyCell;
    private Grid grid;

    public FoodObject[] foodPrefabs;
    public PlayerCtrl playerCtrl;
    public int width;
    public int height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    public WallObject wallPrefab;

    public int leastFood;
    public int maximumFood;
    // Start is called before the first frame update
    public void Init()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        grid = GetComponentInChildren<Grid>();
        EmptyCell = new List<Vector2Int>();

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
                    EmptyCell.Add(new Vector2Int(x, y));
                }
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        EmptyCell.Remove(new Vector2Int(1, 1));
        GenerateWall();
        GenerateFood();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 CellToWorld(Vector2Int cellIdx)
    {
        return grid.GetCellCenterWorld((Vector3Int)cellIdx);
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
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

    private void GenerateFood()
    {
        int foodCount = Random.Range(leastFood, maximumFood + 1);

        for(int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, EmptyCell.Count);
            Vector2Int coord = EmptyCell[randomIndex];
            EmptyCell.RemoveAt(randomIndex);

            int foodChoose = Random.Range(0, foodPrefabs.Length);
            FoodObject newFood = Instantiate(foodPrefabs[foodChoose]);
            AddObject(newFood, coord);
        }
    }

    private void GenerateWall()
    {
        int wallCount = Random.Range(6, 10);
        for(int i = 0; i <wallCount; i++)
        {
            int randomIndex = Random.Range(0, EmptyCell.Count);
            Vector2Int coord = EmptyCell[randomIndex];
            EmptyCell.RemoveAt(randomIndex);

            WallObject newWall = Instantiate(wallPrefab);
            AddObject(newWall, coord);
        }
    }

    private void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }
}
