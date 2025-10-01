using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InventoryBigTest : MonoBehaviour
{

    List<Item> items = new List<Item>();

    private System.Random rand = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10000; i++)
        {
            string name = $"Item_{i:D5}";
            int qty = rand.Next(1, 100);
            items.Add(new Item(name, qty));
        }

        string target = "Item_45672";
        Stopwatch sw = Stopwatch.StartNew();
        Item foundLinear = FindItemLinear(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[Lineary] Found Item: {foundLinear.itemName}, Count: {foundLinear.quantity}, 시간: {sw.ElapsedMilliseconds} ms");



        sw.Restart();
        Item foundBinary = FindItemBinary(target);
        sw.Stop();
        UnityEngine.Debug.Log($"Lineary] Found Item: {foundBinary.itemName}, : Count{foundBinary.quantity}, 시간: {sw.ElapsedMilliseconds} ms");
        // Update is called once per frame

    }

    public Item FindItemLinear(string targetName)
    {
        foreach (var item in items)
        {
            if (item.itemName == targetName)
                return item;
        }
        return null;
    }

    public Item FindItemBinary(string targetName)
    {
     
        int left = 0;
        int right = items.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int cmp = items[mid].itemName.CompareTo(targetName);
            if (cmp == 0)
            {
                return items[mid];
            }
            else if (cmp < 0)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }
        return null;
    }
}