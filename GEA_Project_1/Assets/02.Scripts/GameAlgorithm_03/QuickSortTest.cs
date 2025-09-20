using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSortTest : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        int[] data = GenerateRandomArray(100);
        StartQuickSort(data, 0, data.Length - 1);
        foreach (var item in data)
        {
            Debug.Log(item);
        }
    }

    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();
        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000); // 0���� 999������ ���� ����
        }
        return arr;
    }

    // Update is called once per frame
    public void StartQuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int piovtIndex = Partition(arr, low, high);
            StartQuickSort(arr, low, piovtIndex - 1);  // �ǹ����� ���� �κ� ����
            StartQuickSort(arr, piovtIndex + 1, high); // �ǹ����� ū �κ� ����
        }
    }

    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high]; // �ǹ��� �迭�� ������ ��ҷ� ����
        int i = low - 1; // �ǹ����� ���� ����� �ε���
        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                // arr[i]�� arr[j] ��ȯ
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
        // �ǹ��� �ùٸ� ��ġ�� ��ȯ
        int temp1 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp1;
        return i + 1; // �ǹ��� �ε��� ��ȯ
    }
}
