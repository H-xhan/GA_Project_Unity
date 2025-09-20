using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayIventory : MonoBehaviour
{
    public int iventorySize = 10; // 인벤토리 칸 개수
    public Item[] items;

    private void Start()
    {
        items = new Item[iventorySize];
    }

    public void AddItem(string itemName)
    {
        // 빈칸 찾기 
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new Item(itemName, 1);
                Debug.Log(itemName + "추가됨 (슬롯" + i + ")");
                return;
            }
        }
        Debug.Log("인벤토리가 가득 찼습니다");
    }

    public void RemoveItem(string itemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemName == itemName)
            {
                Debug.Log(itemName + "삭제됨(슬롯" + i + ")");
                items[i] = null;
                return;
            }
        }
        Debug.Log(itemName + "아이템 없습니다");
    }

    public void PrintInventory()
    {
        Debug.Log("=== 배열 인벤토리 상태 ====");
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                Debug.Log(i + "번 슬롯: " + items[i].itemName + "x" + items[i].quantity);
            }
            else
            {
                Debug.Log(i + "번 슬롯: 비어있음");
            }
        }
    }
}