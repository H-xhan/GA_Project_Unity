using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemCell : MonoBehaviour
{
    public TMP_Text nameText;
    public Image icon;

    public void Setup(string itemName, Sprite sprite)
    {
        if (nameText) nameText.text = itemName;
        if (icon)
        {
            icon.sprite = sprite;
            icon.preserveAspect = true;
            icon.enabled = sprite != null;
        }
    }
}

