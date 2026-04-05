using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class EnemyPathfindingGeneric : MonoBehaviour
{
    [Header("Pathfinding Attributes")]
    [SerializeField] private int eSightRange; //how far can the enemy see
    [SerializeField] private Transform target; //reference to chase target

    [Header("Tilemap data reference")]
    [SerializeField] private Tilemap groundTiles;

    [Header("Chase Values")]
    [SerializeField] private float speed;

    private bool hasLOS = false; //temp variable
    private Vector3Int currPos;
    private Vector3Int targetPos;

    void Update()
    {
        currPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
        targetPos = new Vector3Int(Mathf.FloorToInt(target.position.x), Mathf.FloorToInt(target.position.y), Mathf.FloorToInt(target.position.z));

        if (Mathf.Clamp(targetPos.x, currPos.x-eSightRange, currPos.x+eSightRange) == targetPos.x && Mathf.Clamp(targetPos.y, currPos.y-eSightRange, currPos.y+eSightRange) == targetPos.y)
        {
            if (TileGridData.instance.HasLOS(currPos, targetPos))
            {
                hasLOS = true;
            }
            else
            {
                hasLOS = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (hasLOS)
        {
            moveTowards(transform.position, target.position, speed);
        }
        else
        {
            moveTowards(transform.position, findActiveTile(), speed);
        }
    }

    private Vector3Int findActiveTile()
    {   
        Dictionary<Vector3Int, TileInfo> tiles = TileGridData.instance.tiles;
        Vector3Int closestActiveTile = new();

        foreach (Vector3Int pos in tiles.Keys)
        {
            int currDist = -2;
            int currMaxDist = 50;

            if (Mathf.Clamp(pos.x, transform.position.x - eSightRange, transform.position.x + eSightRange) == pos.x &&
                    Mathf.Clamp(pos.y, transform.position.y - eSightRange, transform.position.y + eSightRange) == pos.y &&
                        tiles[pos].isActive)
            {
                currDist = TileGridData.instance.gridDistance(groundTiles.WorldToCell(transform.position), pos);
                if(currDist < currMaxDist)
                {
                    currMaxDist = currDist;
                    closestActiveTile = pos;
                }
            }
        }

        return closestActiveTile;
    }

    private void moveTowards(Vector3 from, Vector3 to, float speed)
    {
        transform.position = Vector3.MoveTowards(from, to, speed * Time.deltaTime);
    }
}
