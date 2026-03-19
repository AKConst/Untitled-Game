using UnityEngine;
using UnityEngine.UIElements;

public class TerminalSave : MonoBehaviour
{
    [Header("UI Settings")]
    public UIDocument checkpointMenuDocument; 

    public float Range = 3.0f;
    public LayerMask playerLayer;
    private bool MenuOpen = false;
    private bool playerInRange = true;
    private VisualElement menuRoot;

    private void Start()
    {
        menuRoot = checkpointMenuDocument.rootVisualElement;
        menuRoot.style.display = DisplayStyle.None;
 
    }
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.F) && !MenuOpen)
        {
            if (checkpointMenuDocument != null && menuRoot != null)
            {
                menuRoot.style.display = DisplayStyle.Flex;
                MenuOpen = true;
                
               
                Time.timeScale = 0f;
            }
        }

        if(MenuOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            menuRoot.style.display = DisplayStyle.None;
            MenuOpen = false;

            Time.timeScale = 1f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (playerLayer == other.gameObject.layer)
        {
            playerInRange = true;
            Debug.Log("Player entered");    
        }
    }
}
