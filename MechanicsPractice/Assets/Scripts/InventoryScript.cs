using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance; //instance to itself

    public static Ability[] rAbilities = new Ability[10]; //empty list the size of 6 items
    [SerializeField] private Image[] abilities; //the list of images that act as the display for the items

    private void Awake()
    {
        instance = this; //assigning the instance to itself
    }

    public void Start()
    {
        InitializeInventory(); //initate the inventory
    }

    private void Update()
    {
        //check if ANY key is pressed
        if (Input.anyKeyDown)
        {
            //we create an empty keycode variable
            KeyCode pressed = new KeyCode();
            //we check every key on the keyboard and see if it is the one that was pressed
            //if so, we assign its value to our previously instantiated empty keycode
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    pressed = key;
                }
            }

            //depending on the one that is pressed, we check for the suitable matches
            //we use the item at the appropriate index based on the key pressed
            switch (pressed)
            {
                case KeyCode.Alpha1:
                    UseItem(0);
                    break;
                case KeyCode.Alpha2:
                    UseItem(1);
                    break;
                case KeyCode.Alpha3:
                    UseItem(2);
                    break;
                case KeyCode.Alpha4:
                    UseItem(3);
                    break;
                case KeyCode.Alpha5:
                    UseItem(4);
                    break;
                case KeyCode.Alpha6:
                    UseItem(5);
                    break;
            }
        }
    }

    public void AddItem(Ability i)
    {
        //we iterate over all of the item objects
        for (int j = 0; j < rAbilities.Length; j++)
        {
            //check if the list position is not occupied as well as the sprite for the item UI
            if (rAbilities[j] == null && abilities[j].sprite == null)
            {
                //if not, then we can add our item to that spot
                rAbilities[j] = i; //we set the value of that index position in the items list to be the item that needs to be 
                abilities[j].sprite = i.itemSprite; //we set the sprite at the item UI index to the items sprite
                abilities[j].color = Color.white; //we set the color at the item UI index to be white so it can be seen 
                return;
            }
        }
    }

    public void UseItem(int itemId)
    {
        //Check if the item slot that was attempted to be used is free or not
        if (rAbilities[itemId] == null)
        {
            Debug.Log("Empty Item Slot");
            return;
        }
        else
        {
            //if not, we use the item
            //for now, we just remove the item from the item list by setting its index value to null
            //then setting the sprite and color of the index in the item UI list to null and color to RGBA(0,0,0,0)
            Debug.Log("Used item-" + rAbilities[itemId].name);
            rAbilities[itemId] = null;
            abilities[itemId].sprite = null;
            abilities[itemId].color = new Color(0, 0, 0, 0);
        }
    }

    public void InitializeInventory()
    {
        Debug.Log("Initializing Inventory!");
        //we iterate over the items list
        for (int i = 0; i < rAbilities.Length; i++)
        {
            //if the position is not empty, we set the UI to be the items sprite and color to white
            if (rAbilities[i] != null)
            {
                Debug.Log("Initialized-"+ rAbilities[i].name);
                abilities[i].sprite = rAbilities[i].itemSprite;
                abilities[i].color = Color.white;
            }
            else
            {
                //if empty then we set the sprite to null
                Debug.Log("Item is null!");
                abilities[i].sprite = null;
                abilities[i].color = new Color(0, 0, 0, 0);
            }
        }
    }
}
