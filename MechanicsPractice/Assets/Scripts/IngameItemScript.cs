using UnityEngine;

public class IngameItemScript : MonoBehaviour
{
    //references to the item that the in-game item pickup stores as well as its interact indicator
    [SerializeField] private itemSO itemS;
    [SerializeField] private SpriteRenderer indicator;

    //boolean for keeping track of interactable status
    private bool isInteractable = false;

    private void Update()
    {
        //Check if the 'E' key is pressed and the item is interactable
        if (Input.GetKeyDown(KeyCode.E) && isInteractable)
        {
            Debug.Log("Add item to inventory!");
            //we run the add item function from the inventory script, and pass the item this object holds to it
            InventoryScript.instance.AddItem(itemS);
            //the item destroys its in-game representation
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        //check if the player has entered the area, if so, then we set the boolean as true and enable the indicator
        if (collider.CompareTag("PlayerTag"))
        {
            isInteractable = true;
            indicator.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        //check if the player has left the area, if so, then we set the boolean as false and disable the indicator
        if (collider.gameObject.CompareTag("PlayerTag"))
        {
            isInteractable = false;
            indicator.enabled = false;
        }
    }
}
