using UnityEngine;
using System.Collections.Generic;

public class AbilityHolder : MonoBehaviour
{
    public static AbilityHolder instance; //instance used for referencing in other scripts

    public List<Ability> allAbilities = new List<Ability>(); //dynamic list of all equipped items
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

    void Awake()
    {
        //set the instance if null and also set the current ability to the one of the current index
        if (instance == null)
        {
            instance = this;
        }
        ability = allAbilities[abilityIndex];
    }

    void Update()
    {
        switch (state) //What follows depends on which state the machine is in
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    if (ability != null)
                    {
                        ability.Activate(gameObject); //Run ability
                        state = AbilityState.active;
                        activeTime = ability.activeTime;
                        active = true;
                    }
                    else
                    {
                        Debug.Log("[-] Ability is null!");
                    }
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
    public int GetCurrAbilityIndex()
    {
        return abilityIndex;
    }
    public void SetCurrAbilityIndex(int cai)
    {
        abilityIndex = cai;
    }
}
