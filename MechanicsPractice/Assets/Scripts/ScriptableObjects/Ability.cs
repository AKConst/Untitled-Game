using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability")]
public class Ability : ScriptableObject //We make a class to be used for all abilities
{
    public new string name;
    public float cooldownTime; //wait time until next ability usage
    public float activeTime; //time while ability is active
    public Sprite itemSprite;

    public virtual void Activate(GameObject parent) // virtual class to call ability
    {

    }
}
