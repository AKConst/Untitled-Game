using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class PathfindingScript : MonoBehaviour
{
    [Header("Pathfinding Attributes")]
    [SerializeField] private Transform target; //reference to chase target
    [SerializeField] private bool isMalee; //boolean to decide how the enemy pathfinding will act
    [SerializeField] private float idealRange; //used for ranged enemies to assign ideal range value
    private NavMeshAgent agent; //reference to our navmeshagent

    [Header("Tilemap data reference")]
    [SerializeField] private Tilemap groundTiles; //reference to our grind tiles tilemap

    //Vector3Int used for the current and target position to calculate LOS using the grid system
    private Vector3Int currPos; 
    private Vector3Int targetPos;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    void Update()
    {
        currPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
        targetPos = new Vector3Int(Mathf.FloorToInt(target.position.x), Mathf.FloorToInt(target.position.y), Mathf.FloorToInt(target.position.z));

        if (isMalee)
        {
            if (TileGridData.instance.HasLOS(currPos, targetPos))
            {
                agent.SetDestination(target.position);
            }
        }
        else
        {
            if (TileGridData.instance.HasLOS(currPos, targetPos))
            {
                agent.SetDestination(findIdealRange());
            }
        }
    }

    private Vector3 findIdealRange()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newTarget = target.position - direction * idealRange;

        return newTarget;
    }
}
