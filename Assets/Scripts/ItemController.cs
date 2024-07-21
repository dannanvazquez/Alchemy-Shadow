using TMPro;
using UnityEngine;

public class ItemController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private Canvas nameCanvas;
    [SerializeField] private TMP_Text nameText;

    private bool _isSelected = false;

    private void Awake() {
        nameText.text = itemSO.itemName;
    }

    void OnMouseEnter() {
        nameCanvas.enabled = true;
    }

    void OnMouseExit() {
        nameCanvas.enabled = false;
    }

    void OnMouseDown() {
        if (GameManager.Instance.isSelectingItems) {
            ToggleSelection(!_isSelected);
        }
    }

    public void ToggleSelection(bool isSelected) {
        if (_isSelected != isSelected) {
            _isSelected = isSelected;
            GameManager.Instance.ToggleSelectedItem(itemSO, isSelected);
        }
    }
}
