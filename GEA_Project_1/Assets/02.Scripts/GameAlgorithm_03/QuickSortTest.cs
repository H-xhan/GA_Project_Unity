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
            arr[i] = rand.Next(0, 10000); // 0부터 999까지의 랜덤 숫자
        }
        return arr;
    }

    // Update is called once per frame
    public void StartQuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int piovtIndex = Partition(arr, low, high);
            StartQuickSort(arr, low, piovtIndex - 1);  // 피벗보다 작은 부분 정렬
            StartQuickSort(arr, piovtIndex + 1, high); // 피벗보다 큰 부분 정렬
        }
    }

    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high]; // 피벗을 배열의 마지막 요소로 설정
        int i = low - 1; // 피벗보다 작은 요소의 인덱스
        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                // arr[i]와 arr[j] 교환
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
        // 피벗을 올바른 위치로 교환
        int temp1 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp1;
        return i + 1; // 피벗의 인덱스 반환
    }
}
