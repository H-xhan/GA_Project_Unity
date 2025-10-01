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
    public GameObject cellPrefab;    // ShopItemCell 프리팹
    public Sprite defaultIcon;       // 아이콘 없을 때 기본

    // 내부 상태
    private List<string> items = new List<string>();       // 정렬된 이름 목록
    private Dictionary<string, ShopItemCell> cells = new Dictionary<string, ShopItemCell>();

    void Start()
    {
        // 1) 데이터 생성 (Item_00 ~ Item_99)
        for (int i = 0; i < 100; i++) items.Add($"Item_{i:D2}");
        items.Sort(); // BinarySearch 전제

        // 2) 그리드 구성
        BuildGrid();

        // 3) 버튼 바인딩
        if (btnLinear) btnLinear.onClick.AddListener(() => SearchLinear());
        if (btnBinary) btnBinary.onClick.AddListener(() => SearchBinary());

        // 4) 최초엔 전체 표시
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

            // 프리팹에 TMP_Text/Image가 연결돼 있어야 함
            // 기본 아이콘(없으면 흰 사각형 그대로)
            cell.Setup(name, defaultIcon);

            cells[name] = cell;
        }
    }

    // 빈 입력이면 전체, 아니면 해당 아이템만 On
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

    // 선형 탐색
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

    // 이진 탐색 (items는 미리 정렬됨)
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
