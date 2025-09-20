using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;  // UI 버튼, 텍스트 연결에 필요

public class UI_Sort : MonoBehaviour
{
    public Text resultText; // 결과 표시할 UI Text (Canvas 안에 Text 만들고 드래그)

    // [버튼 이벤트] 선택 정렬
    public void OnClickSelectionSort()
    {
        int[] data = GenerateRandomArray(10000);
        Stopwatch sw = new Stopwatch();

        sw.Start();
        SelectionSort(data);
        sw.Stop();

        long time = sw.ElapsedMilliseconds;
        resultText.text = $"선택 정렬: {time} ms";
        UnityEngine.Debug.Log($"선택 정렬: {time} ms");
    }

    // [버튼 이벤트] 버블 정렬
    public void OnClickBubbleSort()
    {
        int[] data = GenerateRandomArray(10000);
        Stopwatch sw = new Stopwatch();

        sw.Start();
        BubbleSort(data);
        sw.Stop();

        long time = sw.ElapsedMilliseconds;
        resultText.text = $"버블 정렬: {time} ms";
        UnityEngine.Debug.Log($"버블 정렬: {time} ms");
    }

    // [버튼 이벤트] 퀵 정렬
    public void OnClickQuickSort()
    {
        int[] data = GenerateRandomArray(10000);
        Stopwatch sw = new Stopwatch();

        sw.Start();
        QuickSort(data, 0, data.Length - 1);
        sw.Stop();

        long time = sw.ElapsedMilliseconds;
        resultText.text = $"퀵 정렬: {time} ms";
        UnityEngine.Debug.Log($"퀵 정렬: {time} ms");
    }

    // 랜덤 배열 생성
    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();

        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000);
        }
        return arr;
    }
    // 선택 정렬
    public static void SelectionSort(int[] arr)
    {
        int n = arr.Length;

        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;

            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }

            // swap
            int temp = arr[minIndex];
            arr[minIndex] = arr[i];
            arr[i] = temp;
        }
    }

    // 버블 정렬
    public static void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    // swap
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                }
            }
            if (!swapped) break;
        }
    }

    // 퀵 정렬
    public static void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(arr, low, high);

            QuickSort(arr, low, pivotIndex - 1);   // 피벗 왼쪽 정렬
            QuickSort(arr, pivotIndex + 1, high);  // 피벗 오른쪽 정렬
        }
    }

    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = (low - 1);

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;

                // swap
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        // pivot 자리 교환
        int temp2 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp2;

        return i + 1;
    }
}
