using UnityEngine;

public class ItemController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private AudioSource hoverAudio;

    private bool _isSelected = false;

    void OnMouseEnter() {
        ItemHoverUIController.Instance.EnableUI(itemSO);
        hoverAudio.Play();
    }

    void OnMouseOver() {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        ItemHoverUIController.Instance.UpdatePosition(mouseWorldPos);
    }

    void OnMouseExit() {
        ItemHoverUIController.Instance.DisableUI();
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
