using UnityEngine;

[System.Serializable] //serializable in order to allow us to break down and rebuild the instances of data of this type.
public class SaveData
{
    //model for the save data that we will store
    public itemSO[] itemList;
    public int playerHP;
}
