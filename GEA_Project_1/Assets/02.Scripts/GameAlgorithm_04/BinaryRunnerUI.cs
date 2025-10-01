using UnityEngine;
using UnityEngine.UI;      // UI Image
using TMPro;               // TMP
using System.Collections.Generic;

public class BinaryRunnerUI : MonoBehaviour
{
    [Header("UI Refs")]
    public TMP_InputField inputTarget;   // �˻��� �Է�â
    public TMP_Text resultText;          // ��� �ؽ�Ʈ ǥ��

    [Header("Logic")]
    public InventoryBinary binary;       // �װ� ���� ���� Ž�� ��ũ��Ʈ(FindItem ����)

    [Header("Icon (Optional)")]
    public Image resultIcon;             // ��� ������ ǥ�ÿ�
    [System.Serializable]
    public class IconMap { public string name; public Sprite sprite; }  // �̸�-��������Ʈ ����
    public List<IconMap> iconMaps = new List<IconMap>();
    public Sprite fallbackIcon;          // �� ã���� �� �⺻ ������(����� ��)

    private Dictionary<string, Sprite> dict;  // ��ҹ��� ���� ��ųʸ�

    void Awake()
    {
        // ������ �̸�-��������Ʈ ���� ĳ��
        dict = new Dictionary<string, Sprite>(System.StringComparer.OrdinalIgnoreCase);
        foreach (var m in iconMaps)
        {
            if (m != null && !string.IsNullOrEmpty(m.name) && m.sprite != null)
                dict[m.name] = m.sprite;
        }
    }

    // ��ư OnClick���� ȣ��
    public void Run()
    {
        if (binary == null || inputTarget == null || resultText == null)
        {
            Debug.LogWarning("BinaryRunnerUI: missing refs");
            return;
        }

        // ���� �ʱ�ȭ: ���� InventoryBinary�� Strat() ��Ÿ�� Start�� �� �� �� �־�
        // ����Ʈ�� ��� ������ �� ���� ���� ���� + ����(���� Ž�� ���� ����)
        if (binary.items != null && binary.items.Count == 0)
        {
            binary.items.Add(new Item("potion", 5));
            binary.items.Add(new Item("High-Potion", 2));
            binary.items.Add(new Item("Elixir", 1));
            binary.items.Add(new Item("Sword", 1));
            binary.items.Sort((a, b) => a.itemName.CompareTo(b.itemName));
        }

        string target = inputTarget.text;
        var item = binary.FindItem(target);   // ���� �Լ� �״�� ȣ��

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
