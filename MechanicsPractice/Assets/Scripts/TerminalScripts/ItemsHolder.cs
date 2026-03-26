using System.Collections.Generic;
using UnityEngine;

public class ItemsHolder : MonoBehaviour
{
    public static ItemsHolder instance; //instance used for referencing in other scripts

    public List<itemSO> items = new List<itemSO>(); //dynamic list of all items

    void Awake()
    {
        //set the instance if null
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddItem(itemSO item)
    {
        items.Add(item);
    }
}
