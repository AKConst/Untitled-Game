using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

[CreateAssetMenu]
public class ShieldAbility : Ability 
{
    [SerializeField] private ParticleSystem shieldParticle;
    public override void Activate(GameObject parent)
    {
        ParticleSystem shield = Instantiate(shieldParticle, parent.transform.position, Quaternion.identity, parent.transform);
        MonoBehaviour parentMonoBehaviour = parent.GetComponent<MonoBehaviour>();
        if (parentMonoBehaviour != null)
        {
            parentMonoBehaviour.StartCoroutine(deleteIndicator(shield));
        }
    }

    private IEnumerator deleteIndicator(ParticleSystem ind)
    {
        yield return new WaitForSeconds(activeTime);
        Destroy(ind);
    }
}
