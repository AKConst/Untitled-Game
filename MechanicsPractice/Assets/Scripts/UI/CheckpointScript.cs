using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckpointScript : MonoBehaviour
{
    public UIDocument document;
    private VisualElement menuRoot;

    private ListView itemsListUI;
    private const int itemSize = 100;
    private List<string> itemsDis = new List<string>(itemSize);

    private ListView abilityListUI;
    private const int abilitySize = 7;
    private List<string> abilityDis = new List<string>(abilitySize);

    private void Start()
    {
        menuRoot = document.rootVisualElement; //set up the menu root element for more convenient access when writing code

        //setup of functionality of the items section
        itemsListUI = menuRoot.Q<ListView>(); //initializing the value on waking

        //iterating over each item and adding it's name string to our string list
        foreach(itemSO item in ItemsHolder.instance.items)
        {
            itemsDis.Add(item.itemName);
        }
        itemsListUI.itemsSource = itemsDis; //assigning the value from our finalized list

        itemsListUI.makeItem = () => new Label(); //lambda function assigned, will be automatically called by the listview when creating items
        itemsListUI.bindItem = (elem, index) => ((Label)elem).text = itemsDis[index]; //binds each element to it's value
        itemsListUI.selectionType = SelectionType.Single; //selection type, we allow only for single selection
        //lambda function that will be called when an item is seleceted, will return the index of the selected
        //item from the listview
        itemsListUI.selectedIndicesChanged += (selectedIndices) => 
        {
            Debug.Log("Index selected: " + string.Join(", ", selectedIndices));
        };


        //same thing we did with the items but for the list of abilities
        abilityListUI = menuRoot.Q<ListView>("AbilityList");

        Debug.Log(AbilityHolder.instance);
        foreach(Ability ability in AbilityHolder.instance.allAbilities)
        {
            abilityDis.Add(ability.name);
            Debug.Log(ability.name);
        }
        abilityListUI.itemsSource = abilityDis;

        abilityListUI.makeItem = () => new Label();
        abilityListUI.bindItem = (elem, index) => ((Label)elem).text = abilityDis[index];
        abilityListUI.selectionType = SelectionType.Single;
        abilityListUI.selectedIndicesChanged += (selectedIndices) =>
        {
            Debug.Log("Assigned ability of index: " + selectedIndices);
            int newIndex = int.Parse(selectedIndices.ToString()); //need to parse the selected index
            AbilityHolder.instance.SetCurrAbilityIndex(newIndex); //assign the new active ability index
        };
    }
}
