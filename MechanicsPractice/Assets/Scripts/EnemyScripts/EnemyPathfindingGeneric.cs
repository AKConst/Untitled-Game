using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfindingGeneric : MonoBehaviour
{
    [SerializeField] private Transform target; //reference to chase target
    private bool hasLOS = false;

    void Update()
    {
        RaycastHit2D losRay = Physics2D.Raycast(transform.position, target.transform.position-transform.position, 15, 7);
        Debug.Log(losRay.collider.tag);
    }
}
