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
    public ExitCellObject ExitCellPrefab;

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

        // player가 위치한 1, 1은 미사용
        EmptyCell.Remove(new Vector2Int(1, 1));

        // 출구 설정
        Vector2Int endCoord = new Vector2Int(width - 2, height - 2);
        ExitCellObject newExit = Instantiate(ExitCellPrefab);
        AddObject(newExit, endCoord);
        EmptyCell.Remove(endCoord);

        GenerateWall();
        GenerateFood();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 좌표 가져오기
    public Vector3 CellToWorld(Vector2Int cellIdx)
    {
        return grid.GetCellCenterWorld((Vector3Int)cellIdx);
    }

    // Cell 타일 배치
    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y), tile);
    }

    // Cell 타일 가져오기
    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }

    // Cell 데이터 획득하기
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

    // 음식 생성
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

    // 벽 생성
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

    // 맵 위에 오브젝트 추기
    // data를 가져오고 오브젝트에 위치, 데이터를 넣은 후 맵에 Init으로 배치
    // CellObject 하위 타겟들이 주로 사용함
    private void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }

    // 다음 레벨을 가게 하기 위헤 타일 리셋
    public void ResetField()
    {
        if(BoardData == null)
        {
            return;
        }
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                var cellData = BoardData[x, y];
                if (cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject);
                }
                SetCellTile(new Vector2Int(x, y), null);
            }
        }
    }
}
