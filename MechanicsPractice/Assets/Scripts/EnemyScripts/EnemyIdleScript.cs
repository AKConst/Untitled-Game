using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;
using UnityEngine.Tilemaps;

public class EnemyIdleScript : MonoBehaviour
{
    [Header("Idle Behavior Settings")]
    [SerializeField] private bool useFixedPositions = true;
    [SerializeField] private Vector3[] fixedPatrolPositions;
    [SerializeField] private Tilemap groundTiles;
    [SerializeField] private int scatterPatrolRadius;
    [SerializeField] private float lingerTime;
    private float currLingerTime;

    private NavMeshAgent agent;
    private int patrolIndex = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currLingerTime = lingerTime;
    }

    private void Update()
    {
        if (DestinationReached())
        {
            currLingerTime -= Time.fixedDeltaTime;

            if (currLingerTime < 0)
            {
                if (useFixedPositions)
                {
                    agent.SetDestination(fixedPatrolPositions[patrolIndex]);
                    patrolIndex++;

                    if (patrolIndex >= fixedPatrolPositions.Length) patrolIndex = 0;
                }
                else
                {
                    Vector3 dest = FindRandomActiveTile(scatterPatrolRadius);
                    agent.SetDestination(dest);
                }

                currLingerTime = lingerTime; 
            }
        }
    }

    private bool DestinationReached()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private Vector3 FindRandomActiveTile(int radius)
    {
        Dictionary<Vector3Int, TileInfo> tiles = TileGridData.instance.tiles;
        List<Vector3> positions = new List<Vector3>();
        Vector3Int tPos = groundTiles.WorldToCell(transform.position);

        foreach (Vector3Int pos in tiles.Keys)
        {
            if (tiles[pos].isActive)
            {
                if (Mathf.Clamp(pos.x, tPos.x - radius, tPos.x + radius) == pos.x ||
                            Mathf.Clamp(pos.y, tPos.y - radius, tPos.y + radius) == pos.y)
                {
                    positions.Add(pos);
                }
            }
        }

        return positions[Random.Range(0, positions.Count)];
    }
}
