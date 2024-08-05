using TMPro;
using UnityEngine;

public class ItemController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private Canvas nameCanvas;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private AudioSource hoverAudio;

    private bool _isSelected = false;

    private void Awake() {
        nameText.text = $"{itemSO.itemName} (${itemSO.price})";
    }

    void OnMouseEnter() {
        nameCanvas.enabled = true;
        hoverAudio.Play();
    }

    void OnMouseExit() {
        nameCanvas.enabled = false;
        hoverAudio.Stop();
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
