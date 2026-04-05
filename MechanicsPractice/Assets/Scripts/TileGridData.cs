using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridData : MonoBehaviour
{
    public static TileGridData instance; //public static instance for reference to this script

    [Header("Tilemap references")]
    [SerializeField] private Tilemap groundTiles; //reference to our ground tiles
    [SerializeField] private Tilemap wallTiles; //reference to our wall tiles

    [Header("Player Relevant Information")]
    [SerializeField] private Transform playerPos; //position of our player, used to update active tile data
    [SerializeField] private int activeRange; //how large is the radius for the active tiles

    public Dictionary<Vector3Int, TileInfo> tiles = new(); //Dictionary of the positions mapped onto the data for each tile

    private void Awake()
    {
        //if the instance is null, we set it to itself
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        CreateTileData();
    }

    private void CreateTileData()
    {
        //getting the bounds of the tilemap
        BoundsInt bounds = groundTiles.cellBounds;

        //iterating position within our bounds, checking if there exists a tile, then instantiating and
        //adding it to our dictionary as appropriate
        foreach(Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!groundTiles.HasTile(pos)) continue;

            TileInfo newInfo = new TileInfo
            {
                GridPos = pos,
                isWall = wallTiles.HasTile(pos),
                isActive = false
            };
            tiles[pos] = newInfo; 
        }
    }

    private void Update()
    {
        UpdateActiveTiles();
    }

    public void UpdateActiveTiles()
    {
        Vector3Int pPos = groundTiles.WorldToCell(playerPos.position);

        foreach (Vector3Int pos in tiles.Keys)
        {
            if (Mathf.Clamp(pos.x, playerPos.position.x - activeRange, playerPos.position.x + activeRange) != pos.x ||
                    Mathf.Clamp(pos.y, playerPos.position.y - activeRange, playerPos.position.y + activeRange) != pos.y)
            {
                tiles[pos].isActive = false;
            }
            else
            {
                if (HasLOS(pPos, pos))
                {
                    tiles[pos].isActive = true;
                }
            }
        }
    }

    
    private void OnDrawGizmos()
    {
        foreach(var tile in tiles.Keys)
        {
            if (tiles[tile].isActive)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(tile, 0.35f);
            }
        }
    }
    

    public bool HasLOS(Vector3Int from, Vector3Int to)
    {
        //iterate over each tile in the line
        foreach(Vector3Int cell in GetLine(from, to))
        {
            if (cell == from) continue; //it is the tile we are standing on
            if (!tiles.TryGetValue(cell, out _)) return false; //it doesn't exist
            if (tiles[cell].isWall) return false; //it's a wall, so it needs to be inactive
        }
        return true;
    }


    public int gridDistance(Vector3Int from, Vector3Int to)
    {
        int distance = 0;
        foreach(var tile in GetLine(from, to))
        {
            distance++;
        }

        return distance;
    }


    //Bresenhams algorithm
    private IEnumerable<Vector3Int> GetLine(Vector3Int from, Vector3Int to)
    {
        int x = from.x, y = from.y; //our original location
        int mx = Mathf.Abs(to.x - from.x); //magnitude on x axis
        int my = Mathf.Abs(to.y - from.y); //magnitude on y axis 
        int ax = from.x < to.x ? 1 : -1; //angle for x axis
        int ay = from.y < to.y ? 1 : -1; //angle for y axis
        int err = mx - my; //error deviation in grid

        while (true)
        {
            yield return new Vector3Int(x, y, 0);
            if (x == to.x && y == to.y) break;

            int err2 = err * 2;
            if (err2 > -my) { err -= my; x += ax; }
            if (err2 < mx) { err += mx; y += ay;}
        }
    }
}
