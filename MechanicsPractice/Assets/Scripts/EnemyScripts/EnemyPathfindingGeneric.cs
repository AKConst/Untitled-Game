using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

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
        for (int x = -eSightRange; x < eSightRange; x++)
        {
            for (int y = -eSightRange; y < eSightRange; y++)
            {
                Vector3Int cPos = currPos + new Vector3Int(x, y, 0);

                if (!TileGridData.instance.tiles.TryGetValue(cPos, out _)) continue;

                if (TileGridData.instance.tiles[cPos].isActive)
                {
                    return cPos;
                }
            }
        }

        return currPos;
    }

    private void moveTowards(Vector3 from, Vector3 to, float speed)
    {
        transform.position = Vector3.MoveTowards(from, to, speed * Time.deltaTime);
    }
}
