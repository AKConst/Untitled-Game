using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Collections;

public class EnemyPathfindingGeneric : MonoBehaviour
{
    [Header("Pathfinding Attributes")]
    [SerializeField] private Transform target; //reference to chase target
    [SerializeField] private float steerRadius;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float pathfindInterval;
    private float pathfindTimer = 0f;
    private Coroutine followCoroutine;

    [Header("Tilemap data reference")]
    [SerializeField] private Tilemap groundTiles;

    [Header("Chase Values")]
    [SerializeField] private float speed;

    private bool hasLOS = false;
    private bool isMoving = false;

    private Vector3Int currPos;
    private Vector3Int targetPos;

    private Vector3Int activeTilePos = new(); //temp value


    void Update()
    {
        currPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
        targetPos = new Vector3Int(Mathf.FloorToInt(target.position.x), Mathf.FloorToInt(target.position.y), Mathf.FloorToInt(target.position.z));

        if (TileGridData.instance.HasLOS(currPos, targetPos))
        { 
            hasLOS = true;
        }
        else
        {
            hasLOS = false;
        }
    }

    private void FixedUpdate()
    {
        if (hasLOS)
        {
            pathfindTimer += Time.fixedDeltaTime;
            if (pathfindTimer >= pathfindInterval)
            {
                pathfindTimer = 0f;
                if (followCoroutine != null) StopCoroutine(followCoroutine);
                followCoroutine = StartCoroutine(followPath());
            }
        }
    }

    private IEnumerator followPath()
    {
        foreach (var tile in findPath(currPos, targetPos))
        {
            Vector3 targetPos = groundTiles.GetCellCenterWorld(tile);

            while (Vector3.Distance(transform.position, targetPos) > 0.5f)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void findActiveTile()
    {   
        Vector3Int furthestActiveTile = new();

        foreach (Vector3Int pos in TileGridData.instance.tiles.Keys)
        {
            if (TileGridData.instance.tiles[pos].isActive && TileGridData.instance.HasLOS(groundTiles.WorldToCell(transform.position), pos))
            {
                int currDist = 100;
                int currMaxDist = 0;

                currDist = TileGridData.instance.gridDistance(groundTiles.WorldToCell(transform.position), pos);
                if(currDist > currMaxDist)
                {
                    currMaxDist = currDist;
                    furthestActiveTile = pos;
                }
            }
        }

        if(!isMoving) activeTilePos = furthestActiveTile; //temp, remove later
    }

    //temp, debugging purpose
    public void OnDrawGizmos()
    {
        if (!hasLOS)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(activeTilePos, 0.35f);
        }
    }

    public List<Vector3Int> findPath(Vector3Int from, Vector3Int to)
    {
        //Convert our data to tilemap grid coordinates
        Vector3Int fromToGrid = groundTiles.WorldToCell(from);
        Vector3Int toToGrid = groundTiles.WorldToCell(to);

        //Reference to our tiles, as well as instances of open and closed tiles
        Dictionary<Vector3Int, TileInfo> tiles = TileGridData.instance.tiles;
        foreach(var tile in tiles.Values)
        {
            tile.gCost = 0;
            tile.hCost = 0;
            tile.parent = null;
        }

        List<TileInfo> open = new List<TileInfo>();
        HashSet<TileInfo> closed = new HashSet<TileInfo>();

        //get our starting tile and set its initial values
        TileInfo startTile = tiles[fromToGrid];
        startTile.gCost = 0;
        startTile.hCost = ManhattanDistance(fromToGrid, toToGrid);
        open.Add(startTile);

        while(open.Count > 0)
        {
            //Getting the tile with the currently lowest total estimated path value
            TileInfo curr = open.OrderBy(t => t.fCost).First();

            if(curr.GridPos == toToGrid)
            {
                return reconstructPath(curr);
            }

            open.Remove(curr);
            closed.Add(curr);

            foreach(Vector3Int neighborPos in getNeighbors(curr.GridPos))
            {
                if (!tiles.ContainsKey(neighborPos)) continue;

                TileInfo neightbor = tiles[neighborPos];

                if (neightbor.isWall || closed.Contains(neightbor)) continue;

                int tentativeG = curr.gCost + 1;

                if(!open.Contains(neightbor) || tentativeG < neightbor.gCost)
                {
                    neightbor.gCost = tentativeG;
                    neightbor.hCost = ManhattanDistance(neighborPos, to);
                    neightbor.parent = curr;

                    if(!open.Contains(neightbor)) open.Add(neightbor);
                }
            }
        }

        return null;
    }

    private List<Vector3Int> reconstructPath(TileInfo end)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        TileInfo curr = end;
        while(curr != null)
        {
            path.Add(curr.GridPos);
            curr = curr.parent;
        }

        path.Reverse();
        return path;
    }

    private List<Vector3Int> getNeighbors(Vector3Int pos)
    {
        return new List<Vector3Int>
        {
            pos + new Vector3Int(1, 0, 0),
            pos + new Vector3Int(-1, 0, 0),
            pos + new Vector3Int(0, 1, 0),
            pos + new Vector3Int(0, -1, 0),
            pos + new Vector3Int(1, 1, 0),
            pos + new Vector3Int(1, -1, 0),
            pos + new Vector3Int(-1, 1, 0),
            pos + new Vector3Int(-1, -1, 0),
        };
    }

    private int ManhattanDistance(Vector3Int from, Vector3Int to)
    {
        return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
    }
}
