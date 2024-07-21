using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private NPCController npcController;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private TMP_Text moneyText;

    private NPCSO[] npcSOs;
    private ItemController[] items;

    [HideInInspector] public bool isSelectingItems = false;

    private int money = 0;

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
        npcSOs = Resources.LoadAll<NPCSO>("ScriptableObjects/NPCs/");
        items = itemContainer.GetComponentsInChildren<ItemController>();

        SpawnNPC();
    }

    public void SpawnNPC() {
        StartCoroutine(SpawnNPCCoroutine());
    }

    private IEnumerator SpawnNPCCoroutine() {
        yield return new WaitForSeconds(1f);

        foreach (ItemController item in items) {
            item.ToggleSelection(false);
        }

        NPCSO npcSO = npcSOs[Random.Range(0, npcSOs.Length)];
        npcController.InitializeNPC(npcSO);
    }

    public void ToggleSelectedItem(ItemSO item, bool isSelected) {
        npcController.ToggleSelectedItem(item, isSelected);
    }

    public void AddMoney(int addedMoney) {
        money += addedMoney;
        moneyText.text = "$" + money;
    }
}
