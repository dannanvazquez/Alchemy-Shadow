using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private NPCController npcController;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private ClickAnywhereController clickAnywhereController;
    [SerializeField] private Canvas dayCanvas;
    [SerializeField] private TMP_Text dayText;

    private NPCSO[] npcSOs;
    private ItemController[] items;

    [Header("Settings")]
    [Tooltip("The amount of NPCs that spawn every day.")]
    [SerializeField] private int npcAmountPerDay;

    [HideInInspector] public bool isSelectingItems = false;

    private int money = 0;
    private int day = 0;
    private int todaysNpcAmount = 0;

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

        StartCoroutine(NewDayCoroutine());
    }

    private IEnumerator NewDayCoroutine() {
        day++;
        todaysNpcAmount = 0;

        dayText.text = "Day " + day;
        dayCanvas.enabled = true;

        yield return clickAnywhereController.AwaitInputCoroutine();

        dayCanvas.enabled = false;
        SpawnNPC();
    }

    public void SpawnNPC() {
        if (todaysNpcAmount >= npcAmountPerDay) {
            StartCoroutine(NewDayCoroutine());
        } else {
            todaysNpcAmount++;
            StartCoroutine(SpawnNPCCoroutine());
        }
    }

    private IEnumerator SpawnNPCCoroutine() {
        yield return new WaitForSeconds(2f);

        NPCSO npcSO = npcSOs[Random.Range(0, npcSOs.Length)];
        npcController.InitializeNPC(npcSO);
    }

    public void DisableAllItemSelections() {
        foreach (ItemController item in items) {
            item.ToggleSelection(false);
        }
    }

    public void ToggleSelectedItem(ItemSO item, bool isSelected) {
        npcController.ToggleSelectedItem(item, isSelected);
    }

    public void AddMoney(int addedMoney) {
        money += addedMoney;
        moneyText.text = "$" + money;
    }
}
