using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private NPCController npcController;
    [SerializeField] private NPCSO npcSO;
    [SerializeField] private ItemController[] items;

    public bool _isSelectingItems { get; private set; }

    public static GameManager Instance { get; private set; }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        StartCoroutine(SpawnNPCCoroutine());
    }

    private IEnumerator SpawnNPCCoroutine() {
        yield return new WaitForSeconds(1f);

        npcController.InitializeNPC(npcSO);
    }

    public void ToggleAllSelectableItems(bool isSelectingItems) {
        _isSelectingItems = isSelectingItems;

        foreach (ItemController item in items) {
            item.ToggleSelection(false);
        }
    }

    public void ToggleSelectedItem(ItemSO item, bool isSelected) {
        npcController.ToggleSelectedItem(item, isSelected);
    }
}
