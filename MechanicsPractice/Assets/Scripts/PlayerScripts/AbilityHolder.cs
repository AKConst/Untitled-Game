using System.Data.SqlTypes;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public static AbilityHolder instance; //instance used for referencing in other scripts

    public Ability[] allAbilities = new Ability[10]; //reference to all currently equipped abilities
    private int abilityIndex = 0; //current ability index

    private Ability ability; //currently active ability
    float cooldownTime; //ability cooldown time
    float activeTime; //ability active time

    enum AbilityState //State machine for ability 
    {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready; //At start we set it as ready
    [SerializeField] private KeyCode key; //Key that needs to be pressed for ability to be activated
    public bool active = false;

    private void Awake()
    {
        //set the instance if null and also set the current ability to the one of the current index
        if (instance == null)
        {
            instance = this;
        }
        ability = allAbilities[abilityIndex];
    }

    private void cycleAbilities()
    {
        //only one ability so far so implementation with changing sprites will be made in the future
        Debug.Log("Switched active ability to " + allAbilities[abilityIndex]);
    }

    void Update()
    {
        //cycle the ability list if player presses 'X'
        if (Input.GetKeyDown(KeyCode.X))
        {
            //checking if the index is out of range, resetting if so.
            if (abilityIndex == 9) abilityIndex = 0;
            else
            {
                abilityIndex++;
            }
            //cycle the abilities
            cycleAbilities();
        }

        switch (state) //What follows depends on which state the machine is in
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject); //Run ability
                    state = AbilityState.active;
                    activeTime = ability.activeTime;
                    active = true;
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
                    active = false;
                }
            break;
        }
    }

    //getter for currently active ability
    public Ability GetCurrentAbility()
    {
        return ability;
    }
}
