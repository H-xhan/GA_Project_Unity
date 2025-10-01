using UnityEngine;
using UnityEngine.UI;      // UI Image
using TMPro;               // TMP
using System.Collections.Generic;

public class BinaryRunnerUI : MonoBehaviour
{
    [Header("UI Refs")]
    public TMP_InputField inputTarget;   // 검색어 입력창
    public TMP_Text resultText;          // 결과 텍스트 표기

    [Header("Logic")]
    public InventoryBinary binary;       // 네가 만든 이진 탐색 스크립트(FindItem 보유)

    [Header("Icon (Optional)")]
    public Image resultIcon;             // 결과 아이콘 표시용
    [System.Serializable]
    public class IconMap { public string name; public Sprite sprite; }  // 이름-스프라이트 매핑
    public List<IconMap> iconMaps = new List<IconMap>();
    public Sprite fallbackIcon;          // 못 찾았을 때 기본 아이콘(비워도 됨)

    private Dictionary<string, Sprite> dict;  // 대소문자 무시 딕셔너리

    void Awake()
    {
        // 아이콘 이름-스프라이트 매핑 캐시
        dict = new Dictionary<string, Sprite>(System.StringComparer.OrdinalIgnoreCase);
        foreach (var m in iconMaps)
        {
            if (m != null && !string.IsNullOrEmpty(m.name) && m.sprite != null)
                dict[m.name] = m.sprite;
        }
    }

    // 버튼 OnClick에서 호출
    public void Run()
    {
        if (binary == null || inputTarget == null || resultText == null)
        {
            Debug.LogWarning("BinaryRunnerUI: missing refs");
            return;
        }

        // 안전 초기화: 원본 InventoryBinary가 Strat() 오타로 Start가 안 돌 수 있어
        // 리스트가 비어 있으면 한 번만 샘플 생성 + 정렬(이진 탐색 전제 보장)
        if (binary.items != null && binary.items.Count == 0)
        {
            binary.items.Add(new Item("potion", 5));
            binary.items.Add(new Item("High-Potion", 2));
            binary.items.Add(new Item("Elixir", 1));
            binary.items.Add(new Item("Sword", 1));
            binary.items.Sort((a, b) => a.itemName.CompareTo(b.itemName));
        }

        string target = inputTarget.text;
        var item = binary.FindItem(target);   // 기존 함수 그대로 호출

        if (item != null)
        {
            resultText.text = $"[Binary] Found: {item.itemName}";
            SetIcon(item.itemName);
        }
        else
        {
            resultText.text = "[Binary] Not found";
            SetIcon(null);
        }
    }

    private void SetIcon(string itemName)
    {
        if (resultIcon == null) return;

        if (!string.IsNullOrEmpty(itemName) && dict.TryGetValue(itemName, out var sp))
        {
            resultIcon.sprite = sp;
            resultIcon.enabled = true;
        }
        else
        {
            resultIcon.sprite = fallbackIcon;
            resultIcon.enabled = (fallbackIcon != null);
        }
    }
}
