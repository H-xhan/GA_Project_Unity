using UnityEngine;
using UnityEngine.UI;        // Image
using TMPro;
using System.Collections.Generic;

public class LinearRunnerUI : MonoBehaviour
{
    public TMP_InputField inputTarget;   // �˻��� �Է�
    public TMP_Text resultText;          // ��� ���
    public Inventory linear;             // ���� Inventory(FindItem ����)

    public Image resultIcon;             // ��� ������ ǥ�ÿ�
    [System.Serializable]
    public class IconMap { public string name; public Sprite sprite; }  // �̸�-��������Ʈ ��
    public List<IconMap> iconMaps = new List<IconMap>();
    public Sprite fallbackIcon;          // �� ã���� �� �⺻ ������(����)

    private Dictionary<string, Sprite> dict;  // ��Ÿ�� ��ȸ�� ��ųʸ�(��ҹ��� ����)

    void Awake()
    {
        dict = new Dictionary<string, Sprite>(System.StringComparer.OrdinalIgnoreCase);
        foreach (var m in iconMaps)
        {
            if (m != null && !string.IsNullOrEmpty(m.name) && m.sprite != null)
                dict[m.name] = m.sprite; // �ߺ� Ű�� ���������� ���
        }
    }

    public void Run()
    {
        if (linear == null || inputTarget == null || resultText == null) return;

        string target = inputTarget.text;                    // �Է°�
        var item = linear.FindItem(target);                  // ���� �Լ� �״�� ȣ��

        if (item != null)
        {
            resultText.text = $"[Linear] Found: {item.itemName}";  // ��� �ؽ�Ʈ
            SetIcon(item.itemName);                                 // ������ ����
        }
        else
        {
            resultText.text = "[Linear] Not found";
            SetIcon(null);                                         // �⺻/����
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
            resultIcon.sprite = fallbackIcon;  // ������ �⺻
            resultIcon.enabled = (fallbackIcon != null);
        }
    }
}
