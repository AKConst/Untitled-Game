using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability; //We inherit from this class
    float cooldownTime;
    float activeTime;

    enum AbilityState //State machine for ability 
    {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready; //At start we set it as ready
    public KeyCode key; //Key that needs to be pressed for ability to be activated

    void Update()
    {
        switch (state) //What follows depends on which state the machine is in
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject); //Run ability
                    state = AbilityState.active;
                    activeTime = ability.activeTime;
                }
            break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                }
                break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.cooldown;
                    cooldownTime = ability.cooldownTime;
                }
            break;
        }
    }
}
