using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortTest : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        int[] data = GenerateRandomArray(100);
        StartBubbleSort(data);
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
            arr[i] = rand.Next(0, 10000); // 0부터 999까지의 랜덤 숫자
        }
        return arr;
    }

    // Update is called once per frame
    public void StartBubbleSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false; // 교환 여부를 추적
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    // 두 요소 교환
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                }
            }

            if (!swapped) break; // 교환이 없으면 정렬 완료

        }
    }
}
