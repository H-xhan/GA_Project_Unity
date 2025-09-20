using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> inventory = new List<string>();
        inventory.Add("Potion");
        inventory.Add("Sword");

        Debug.Log(inventory[0]);
        Debug.Log(inventory[1]);
        Debug.Log(inventory[2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
