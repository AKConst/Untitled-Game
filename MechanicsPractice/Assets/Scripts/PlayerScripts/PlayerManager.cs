using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public static int playerHealth = 1000; //value to track player HP

    public static int maxHealAmt = 2;
    public static int healAmt = 0;
    public static Transform playerPos;
    [SerializeField] private Sprite[] flaskSprites = new Sprite[3];

    public bool Shielded = false;

    [SerializeField] private UIDocument guiDisplay; //reference to our main GUI

    private void Start()
    {
        instance = this; //assigning the instance to 
        playerPos = gameObject.transform;
        healAmt = maxHealAmt;

        //set the HP bar to the proper value of the current player HP
        float HpVal = ((float)playerHealth / 100) * 325;
        guiDisplay.rootVisualElement.Q<VisualElement>("HPbar").style.maxHeight = HpVal;
        guiDisplay.rootVisualElement.Q<Label>("hpField").text = healAmt.ToString();
    }

    private void Update()
    {
        //we heal the player when they use the healing ability.
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if (healAmt > 0)
            {
                updatePlayerStatus(0, 25, true);
                healAmt -= 1;

                if (healAmt == maxHealAmt)
                {
                    guiDisplay.rootVisualElement.Q<VisualElement>("HealingSprite").style.backgroundImage = new StyleBackground(flaskSprites[0]);
                }
                else if (healAmt <= maxHealAmt / 2 && healAmt > 0)
                {
                    guiDisplay.rootVisualElement.Q<VisualElement>("HealingSprite").style.backgroundImage = new StyleBackground(flaskSprites[1]);
                }
                else if (healAmt <= 0)
                {
                    guiDisplay.rootVisualElement.Q<VisualElement>("HealingSprite").style.backgroundImage = new StyleBackground(flaskSprites[2]);
                }

                guiDisplay.rootVisualElement.Q<Label>("hpField").text = healAmt.ToString();
            }
            else
            {
                Debug.Log("No healing items available!");
            }
        }
    }

    //function used to check player status and execute the proper response as necessary
    public void checkPlayerStatus()
    {
        //if player HP reaches or is below 0, we move to the death screen and destroy the player object
        if (playerHealth <= 0)
        {
            Debug.Log("Player Killed!");
            enabled = false;
            GameManagerScript.instance.ChangeLevel(4);
            Destroy(gameObject);
        }
    }

    //function for updating player status, invoked to change specific values regarding player (only hp for now)
    public void updatePlayerStatus(int valKey, int amp, bool pos=false) //pos value checks if the change effects positively or not
    {
        switch (valKey)
        {
            case 0:
                if (!pos) {
                    if (AbilityHolder.instance.active && AbilityHolder.instance.GetCurrentAbility().name == "Shield")
                    {
                        Shielded = true;
                    }

                    if (Shielded == false)
                    {
                        Debug.Log("Player took " + amp + " damage!");
                        damagePlayer(amp, pos);
                    }
                    else
                    {
                        Debug.Log("Player is shielded");
                        Shielded = false;
                    }
                    break;
                }
                else
                {
                    damagePlayer(amp, pos);
                    break;
                }
            default:
                Debug.Log("[-] Attempted to change improper value!");
                break;
        }
    }

    private void damagePlayer(int dmg, bool pos=false)
    {
        if (!pos)
        {
            playerHealth -= dmg;
            float newVal = ((float)playerHealth / 100f) * 325f;
            guiDisplay.rootVisualElement.Q<VisualElement>("HPbar").style.maxHeight = newVal;
        }
        else
        {
            if (playerHealth+dmg > 100) { playerHealth = 100; }
            else { playerHealth += dmg; }
            float newVal = ((float)playerHealth / 100f) * 325f;
            guiDisplay.rootVisualElement.Q<VisualElement>("HPbar").style.maxHeight = newVal;
        }
    }
}
