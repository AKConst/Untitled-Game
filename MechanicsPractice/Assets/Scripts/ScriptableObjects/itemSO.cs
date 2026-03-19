using UnityEngine;

[CreateAssetMenu(fileName = "itemSO", menuName = "Scriptable Objects/itemSO")] //defining how the object is created
public class itemSO : ScriptableObject
{
    //Model for item object instances

    [Header("Basic Item Details")]
    public string itemName;
    public string itemContent;

    [Header("Item Sprite Information")]
    public Sprite itemSprite;
}
