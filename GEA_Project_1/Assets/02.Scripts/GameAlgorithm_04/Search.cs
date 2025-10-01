using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Search : MonoBehaviour
{
    [Header("UI References (TMP)")]

    public TMP_InputField inputItemCount;

    public TMP_InputField inputSearchCount;

    public TMP_Text resultText;


    private List<Item> items = new List<Item>();


    private long sortSteps;

    private long linearSteps;

    private long binarySteps;
    // Start is called before the first frame update

    public void OnFindButton()
    {
        if (!int.TryParse(inputItemCount.text, out int itemCount)) itemCount = 10000;
        if (!int.TryParse(inputSearchCount.text, out int searchCount)) searchCount = 1000;


        items.Clear();
        for (int i = 0; i < itemCount; i++)
        {
            items.Add(new Item($"Item_{Random.Range(0, itemCount):D5}", 1));
        }

        List<string> targets = new List<string>();
        for (int i = 0; i < searchCount; i++)
        {
            targets.Add($"Item_{Random.Range(0, itemCount):D5}");
        }

        linearSteps = 0;
        foreach (var t in targets)
        {
            linearSteps += FindItemBinarySteps(t);
        }

        sortSteps = 0;
        QuickSort(items, 0, items.Count - 1);

        binarySteps = 0;
        foreach (var t in targets)
        {
            binarySteps += FindItemBinarySteps(t);
        }

        resultText.text =
        $"Item Count: {itemCount}\n" +
        $"Search Count: {searchCount}\n\n" +
        $"Linear Search Total Comparisons: {linearSteps}\n" +
        $"Quick Sort Comparisons: {sortSteps}\n" +
        $"Binary Search Total Comparisons: {binarySteps}\n" +
        $"Total (Sort + Binary): {sortSteps + binarySteps}";

    }

    private void QuickSort(List<Item> list, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(list, left, right);
            QuickSort(list, left, pivotIndex - 1);
            QuickSort(list, pivotIndex + 1, right);
        }
    }

    private int Partition(List<Item> list, int left, int right)
    {
        Item pivot = list[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            sortSteps++;
            if (list[j].itemName.CompareTo(pivot.itemName) <= 0)
            {
                i++;
                Swap(list, i, j);
            }
        }
        Swap(list, i + 1, right);
        return i + 1;
    }

    private void Swap(List<Item> list, int a, int b)
    {
        Item temp = list[a];
        list[a] = list[b];
        list[b] = temp;
    }

    private int FindItemBinarySteps(string target)
    {
        int steps = 0;
        int left = 0;
        int right = items.Count - 1;

        while (left <= right)
        {
            steps++;
            int mid = left + (right - left) / 2;
            int cmp = items[mid].itemName.CompareTo(target);

            if (cmp == 0)
                return steps;
            else if (cmp < 0)
                left = mid + 1;
            else
                right = mid - 1;
        }
        return steps;
    }
}
