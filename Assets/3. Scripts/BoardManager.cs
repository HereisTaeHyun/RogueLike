using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap tilemap;

    public int width;
    public int height;
    public Tile[] tiles;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                int tileNum = Random.Range(0, tiles.Length);
                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[tileNum]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
