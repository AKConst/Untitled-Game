using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class TerminalSave : MonoBehaviour
{
    [Header("UI Settings")]
    public UIDocument checkpointMenuDocument;
    public GameObject InGameUI;

    public float Range = 3.0f;
    public LayerMask playerLayer;
    private bool MenuOpen = false;
    private bool playerInRange = true;
    private VisualElement menuRoot;

    //UI Menu Handling
    private Button ItemLogBtn, AbilityBtn;
    private VisualElement itemDisplay, abilityDisplay;

    private void Awake()
    {
        menuRoot = checkpointMenuDocument.rootVisualElement;
        menuRoot.style.display = DisplayStyle.None;

        ItemLogBtn = menuRoot.Q<Button>("Log");
        ItemLogBtn.RegisterCallback<ClickEvent>(OpenLogs);

        AbilityBtn = menuRoot.Q<Button>("Abilities");
        AbilityBtn.RegisterCallback<ClickEvent>(OpenAbilities);

        itemDisplay = menuRoot.Q<VisualElement>("ItemDisplay");
        abilityDisplay = menuRoot.Q<VisualElement>("AbilityDisplay");
    }
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.F) && !MenuOpen)
        {
            if (checkpointMenuDocument != null && menuRoot != null)
            {
                InGameUI.SetActive(false);
                menuRoot.style.display = DisplayStyle.Flex;
                MenuOpen = true;

               
                Time.timeScale = 0f;
            }
        }

        if(MenuOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            InGameUI.SetActive(true);
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

    public void OpenLogs(ClickEvent evt)
    {
        abilityDisplay.style.display = DisplayStyle.None;
        itemDisplay.style.display = DisplayStyle.Flex;
    }

    public void OpenAbilities(ClickEvent evt)
    {
        itemDisplay.style.display = DisplayStyle.None;
        abilityDisplay.style.display = DisplayStyle.Flex;
    }
}