using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor.Experimental.GraphView;

public class EnemyPathfindingGeneric : MonoBehaviour
{
    [Header("Pathfinding Attributes")]
    [SerializeField] private Transform target; //reference to chase target
    [SerializeField] private float steerRadius;
    [SerializeField] private Rigidbody2D rb;

    [Header("Tilemap data reference")]
    [SerializeField] private Tilemap groundTiles;

    [Header("Chase Values")]
    [SerializeField] private float speed;

    private bool hasLOS = false;
    private bool closeToWall = false;
    private bool closeToAnother = false;
    private Vector3Int currPos;
    private Vector3Int targetPos;

    private Vector3Int activeTilePos = new(); //temp value for debugging

    void Update()
    {
        currPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
        targetPos = new Vector3Int(Mathf.FloorToInt(target.position.x), Mathf.FloorToInt(target.position.y), Mathf.FloorToInt(target.position.z));

        closeToWall = isCloseToWall(LayerMask.GetMask("Obstacle")); //check if we are close to a wall
        closeToAnother = isCloseToWall(LayerMask.GetMask("Enemy")); //check if we are close to another enemy

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
            moveTowards(transform.position, target.position, speed);
        }
        else
        {
            moveTowards(transform.position, findActiveTile(), speed);
        }
    }

    //updating the state for the close to wall boolean
    private bool isCloseToWall(LayerMask layer)
    {
        var ictw = Physics2D.OverlapCircleAll(transform.position, steerRadius, layer);
        if (ictw.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //under construction
    private Vector2 CalculateSteer(float sr, LayerMask layer)
    {
        Vector2 dir = default;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, sr, layer);

        foreach(Collider2D collider in colliders)
        {
            Vector2 diff = (collider.ClosestPoint(transform.position) - (Vector2)transform.position);
            float distance = Mathf.Max(diff.magnitude, 0.1f);
            dir -= (diff / distance) / (distance * distance);
        }

        return dir.normalized;
    }

    private Vector3Int findActiveTile()
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

        activeTilePos = furthestActiveTile; //temp, remove later
        return furthestActiveTile;
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

    private void moveTowards(Vector3 from, Vector3 to, float speed)
    {
        if (closeToWall)
        {
            Vector2 steer = CalculateSteer(steerRadius, LayerMask.GetMask("Obstacle"));
            Vector2 movDir = (to - from).normalized;
            rb.MovePosition((Vector2)transform.position + (movDir + steer) * speed * Time.deltaTime);
        }
        else if (closeToAnother)
        {
            Vector2 steer = CalculateSteer(steerRadius, LayerMask.GetMask("Enemy"));
            Vector2 movDir = (to - from).normalized;
            rb.MovePosition((Vector2)transform.position + (movDir + steer) * speed * Time.deltaTime);
        }
        else
        {
            Vector3 dir = (to - from).normalized;
            rb.MovePosition(transform.position + dir * speed * Time.deltaTime);
        }
    }
}
