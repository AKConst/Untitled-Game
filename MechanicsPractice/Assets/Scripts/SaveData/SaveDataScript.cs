using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataScript : MonoBehaviour
{ 
    public static SaveDataScript instance; //instance to itself, static so its the only one belonging to this class
    public string saveLocation; //location where the save data will be located

    private void Awake()
    {
        instance = this; //assign the instance to the instance of the class
        //assign the save location to the predefined unity persistant data path with the file name appended
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void SaveGame()
    {
        Debug.Log("Saved Game!");
        Debug.Log("Saved player HP as " + PlayerManager.playerHealth);
        //creating our save data instance and assigning it the appropriate values
        SaveData saveData = new SaveData
        {
            abilityList = AbilityHolder.instance.allAbilities,
            currAbilityIndex = AbilityHolder.instance.GetCurrAbilityIndex(),
            itemsList = ItemsHolder.instance.items,
            playerHP = PlayerManager.playerHealth,
            maxHeals = PlayerManager.maxHealAmt,
            currHeals = PlayerManager.healAmt,
            sceneNum = SceneManager.GetActiveScene().buildIndex,
            scenePos = PlayerManager.playerPos
        };
        //writing the data of the save data instance to a json file
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        Debug.Log("Loaded Game!");
        //checking if the save file exists
        if (File.Exists(saveLocation))
        {
            //if it does exist, we read the save data into a new instance of save data class
            //then we read from that object and assign the values to the appropriate variables in the game
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            AbilityHolder.instance.allAbilities = saveData.abilityList;
            AbilityHolder.instance.SetCurrAbilityIndex(saveData.currAbilityIndex);
            ItemsHolder.instance.items = saveData.itemsList;
            PlayerManager.playerHealth = saveData.playerHP;
            PlayerManager.maxHealAmt = saveData.maxHeals;
            PlayerManager.healAmt = saveData.currHeals;
        }
        else
        {
            //if it doesnt, we create it, we load from it then exit out of the function
            SaveGame();
            LoadGame();
            return;
        }
    }
}
