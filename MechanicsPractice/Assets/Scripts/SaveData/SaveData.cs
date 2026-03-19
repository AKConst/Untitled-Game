using UnityEngine;

[System.Serializable] //serializable in order to allow us to break down and rebuild the instances of data of this type.
public class SaveData
{
    //model for the save data that we will store
    public Ability[] abilityList; //list of abilities
    public itemSO[] itemsList; //list of items
    public int playerHP; //player HP
    public int maxHeals; //current maximum amount of heals
    public int currHeals; //currently held amount of heals
    public int sceneNum; //last saved scene
    public Transform scenePos; //position of player on last save
}
