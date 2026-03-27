using NUnit.Framework.Constraints;
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

    VisualElement itemDisplay, itemContentDisplay;

    private void Start()
    {
        menuRoot = document.rootVisualElement; //set up the menu root element for more convenient access when writing code
        //assign values to our relevant containers
        itemDisplay = menuRoot.Q<VisualElement>("ItemDisplay");
        itemContentDisplay = menuRoot.Q<VisualElement>("ItemContentDisplay");
        itemContentDisplay.style.display = DisplayStyle.None;

        //setup of functionality of the items section
        itemsListUI = menuRoot.Q<ListView>(); //initializing the value on waking

        //iterating over each item and adding it's name string to our string list
        foreach(itemSO item in ItemsHolder.instance.items)
        {
            itemsDis.Add(item.itemName);
        }
        itemsListUI.itemsSource = itemsDis; //assigning the value from our finalized list

        //lambda function assigned, will be automatically called by the listview when creating items
        itemsListUI.makeItem = () =>
        {
            Label nl = new Label();
            nl.RegisterCallback<PointerDownEvent>(evt =>
            {
                int index = (int)nl.userData; //we take the index written to the metadata in our binding function
                DisplayItemTerminal(index); //we call our item display function and pass it the index of the item clicked;
            });

            return nl;
        };
        //binds each element to it's value
        itemsListUI.bindItem = (elem, index) =>
        {
            Label item = (Label)elem;
            item.text = itemsDis[index];
            item.userData = index; //assign index to metadata
        };
        itemsListUI.selectionType = SelectionType.None; //selection type, we allow only for single selection


        //same thing we did with the items but for the list of abilities
        abilityListUI = menuRoot.Q<ListView>("AbilityList");

        foreach(Ability ability in AbilityHolder.instance.allAbilities)
        {
            abilityDis.Add(ability.name);
        }
        abilityListUI.itemsSource = abilityDis;

        abilityListUI.makeItem = () =>
        {
            Label nl = new Label();
            nl.RegisterCallback<PointerDownEvent>(evt =>
            {
                int index = (int)nl.userData;
                AbilityHolder.instance.SetCurrAbilityIndex(index);
                AbilityHolder.instance.SetCurrentAbility();
            });

            return nl;
        };
        abilityListUI.bindItem = (elem, index) =>
        {
            Label ability = (Label)elem;
            ability.text = abilityDis[index];
            ability.userData = index;
        };
        abilityListUI.selectionType = SelectionType.None;
    }

    private void DisplayItemTerminal(int index)
    {
        itemDisplay.style.display = DisplayStyle.None;

        Button exitBtn = itemContentDisplay.Q<Button>("exitBtn");
        exitBtn.RegisterCallback<ClickEvent>(HideCurrItemDisplay);

        if (itemContentDisplay.style.display == DisplayStyle.None)
        {
            itemContentDisplay.style.display = DisplayStyle.Flex;

            Label itemNameDis = itemContentDisplay.Q<Label>("itemNameDisplay");
            Label itemContentDis = itemContentDisplay.Q<Label>("itemContent");

            itemNameDis.text = ItemsHolder.instance.items[index].name;
            itemContentDis.text = ItemsHolder.instance.items[index].itemContent;
        }
    }

    private void HideCurrItemDisplay(ClickEvent evt)
    {
        itemContentDisplay.style.display = DisplayStyle.None;
        itemDisplay.style.display = DisplayStyle.Flex;
    }
}
