using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<Item> items = new List<Item>();


    // Start is called before the first frame update
    void Start()
    {

        items.Add(new Item("Sword"));
        items.Add(new Item("Shield"));
        items.Add(new Item("Potion"));

        Item found = FindItem("Potion");

        if (found != null)

            Debug.Log("Found Item: " + found.itemName);

        else

            Debug.Log("Not Found");

    }

    public Item FindItem(string _itemName)
    {
        foreach (var item in items)
        {
            if (item.itemName == _itemName)

                return item;

        }
        return null;
    }



}
