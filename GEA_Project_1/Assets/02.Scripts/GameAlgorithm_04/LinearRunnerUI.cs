using UnityEngine;
using UnityEngine.UI;        // Image
using TMPro;
using System.Collections.Generic;

public class LinearRunnerUI : MonoBehaviour
{
    public TMP_InputField inputTarget;   // 검색어 입력
    public TMP_Text resultText;          // 결과 출력
    public Inventory linear;             // 기존 Inventory(FindItem 보유)

    public Image resultIcon;             // 결과 아이콘 표시용
    [System.Serializable]
    public class IconMap { public string name; public Sprite sprite; }  // 이름-스프라이트 맵
    public List<IconMap> iconMaps = new List<IconMap>();
    public Sprite fallbackIcon;          // 못 찾았을 때 기본 아이콘(선택)

    private Dictionary<string, Sprite> dict;  // 런타임 조회용 딕셔너리(대소문자 무시)

    void Awake()
    {
        dict = new Dictionary<string, Sprite>(System.StringComparer.OrdinalIgnoreCase);
        foreach (var m in iconMaps)
        {
            if (m != null && !string.IsNullOrEmpty(m.name) && m.sprite != null)
                dict[m.name] = m.sprite; // 중복 키면 마지막으로 덮어씀
        }
    }

    public void Run()
    {
        if (linear == null || inputTarget == null || resultText == null) return;

        string target = inputTarget.text;                    // 입력값
        var item = linear.FindItem(target);                  // 기존 함수 그대로 호출

        if (item != null)
        {
            resultText.text = $"[Linear] Found: {item.itemName}";  // 결과 텍스트
            SetIcon(item.itemName);                                 // 아이콘 갱신
        }
        else
        {
            resultText.text = "[Linear] Not found";
            SetIcon(null);                                         // 기본/비우기
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
            resultIcon.sprite = fallbackIcon;  // 없으면 기본
            resultIcon.enabled = (fallbackIcon != null);
        }
    }
}
