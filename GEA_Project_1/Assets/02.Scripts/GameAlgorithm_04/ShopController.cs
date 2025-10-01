using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    [Header("UI Refs")]
    public TMP_InputField inputSearch;
    public Button btnLinear;
    public Button btnBinary;
    public Transform content;        // ScrollView > Viewport > Content
    public GameObject cellPrefab;    // ShopItemCell ������
    public Sprite defaultIcon;       // ������ ���� �� �⺻

    // ���� ����
    private List<string> items = new List<string>();       // ���ĵ� �̸� ���
    private Dictionary<string, ShopItemCell> cells = new Dictionary<string, ShopItemCell>();

    void Start()
    {
        // 1) ������ ���� (Item_00 ~ Item_99)
        for (int i = 0; i < 100; i++) items.Add($"Item_{i:D2}");
        items.Sort(); // BinarySearch ����

        // 2) �׸��� ����
        BuildGrid();

        // 3) ��ư ���ε�
        if (btnLinear) btnLinear.onClick.AddListener(() => SearchLinear());
        if (btnBinary) btnBinary.onClick.AddListener(() => SearchBinary());

        // 4) ���ʿ� ��ü ǥ��
        ShowAll();
    }

    void BuildGrid()
    {
        foreach (Transform child in content) Destroy(child.gameObject);
        cells.Clear();

        foreach (var name in items)
        {
            var go = Instantiate(cellPrefab, content);
            var cell = go.GetComponent<ShopItemCell>();
            if (!cell) cell = go.AddComponent<ShopItemCell>();

            // �����տ� TMP_Text/Image�� ����� �־�� ��
            // �⺻ ������(������ �� �簢�� �״��)
            cell.Setup(name, defaultIcon);

            cells[name] = cell;
        }
    }

    // �� �Է��̸� ��ü, �ƴϸ� �ش� �����۸� On
    void ShowAll()
    {
        foreach (var kv in cells) kv.Value.gameObject.SetActive(true);
    }
    void ShowOnly(string name)
    {
        foreach (var kv in cells) kv.Value.gameObject.SetActive(kv.Key == name);
    }
    void ShowNone()
    {
        foreach (var kv in cells) kv.Value.gameObject.SetActive(false);
    }

    // ���� Ž��
    void SearchLinear()
    {
        string q = (inputSearch ? inputSearch.text : "").Trim();
        if (string.IsNullOrEmpty(q)) { ShowAll(); return; }

        foreach (var name in items)
        {
            if (name == q) { ShowOnly(name); return; }
        }
        ShowNone();
    }

    // ���� Ž�� (items�� �̸� ���ĵ�)
    void SearchBinary()
    {
        string q = (inputSearch ? inputSearch.text : "").Trim();
        if (string.IsNullOrEmpty(q)) { ShowAll(); return; }

        int left = 0, right = items.Count - 1;
        while (left <= right)
        {
            int mid = (left + right) / 2;
            int cmp = string.Compare(items[mid], q, System.StringComparison.Ordinal);
            if (cmp == 0) { ShowOnly(items[mid]); return; }
            if (cmp < 0) left = mid + 1; else right = mid - 1;
        }
        ShowNone();
    }
}
