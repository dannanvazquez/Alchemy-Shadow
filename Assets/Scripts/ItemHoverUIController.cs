using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(RectTransform))]
public class ItemHoverUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemPriceText;
    [SerializeField] private RectTransform containerRectTransform;

    private Canvas canvas;
    private RectTransform rectTransform;

    private Vector2 containerSize;

    public static ItemHoverUIController Instance { get; private set; }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void EnableUI(ItemSO item) {
        itemNameText.text = item.itemName;
        itemPriceText.text = $"Price: <color=green>${item.price}</color>";
        canvas.enabled = true;
    }

    public void UpdatePosition(Vector3 position) {
        if (containerRectTransform.sizeDelta != containerSize) {
            containerSize = containerRectTransform.sizeDelta;
            containerRectTransform.localPosition = new Vector3(containerSize.x / 2, containerSize.y / 2, 0);
        }
        rectTransform.position = position;
    }

    public void DisableUI() {
        canvas.enabled = false;
    }
}
