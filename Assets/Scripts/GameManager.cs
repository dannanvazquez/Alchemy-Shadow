using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private NPCController npcController;

    [SerializeField] private Transform itemContainer;

    [SerializeField] private TMP_Text moneyText;

    [SerializeField] private TimerController timerController;
    [SerializeField] private ClickAnywhereController clickAnywhereController;

    [SerializeField] private Canvas dayCanvas;
    [SerializeField] private TMP_Text dayText;

    [SerializeField] private SpriteRenderer notepadSprite;
    [SerializeField] private Canvas notepadCanvas;
    [SerializeField] private TMP_Text[] notepadTexts;

    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private RecapUIController recapUIController;
    [SerializeField] private EndRecapUIController endRecapUIController;

    [SerializeField] private DialogueSO[] startingDialogueDays;

    [SerializeField] private AudioSource moneyUpAudio;
    [SerializeField] private AudioSource moneyDownAudio;
    [SerializeField] private GameObject moneyChangeUIPrefab;
    [SerializeField] private Vector3 moneyChangeSpawnPosition;

    [Header("Settings")]
    [Tooltip("The amount of money the player starts with.")]
    [SerializeField] private int startingMoney;
    [Tooltip("The amount of money the player is owed by the end of the game.")]
    [SerializeField] private int moneyOwed;

    private ItemController[] items;

    [HideInInspector] public bool isSelectingItems = false;

    private int money = 0;
    private int day = 0;
    private int itemCount = 0;

    // TODO: Properly change how client stats are determined after making a crafting SO derived from dialogue.
    private int clientsServedStat = 0;
    private int moneySpentStat = 0;
    private int moneyEarnedStat = 0;

    public static GameManager Instance { get; private set; }

    public bool IsPaused() { return pauseManager.isPaused; }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        items = itemContainer.GetComponentsInChildren<ItemController>();

        money = startingMoney;
        moneyText.text = "$" + startingMoney;

        NextDay();
    }

    public void NextDay() {
        StartCoroutine(NewDayCoroutine());
    }

    private IEnumerator NewDayCoroutine() {
        day++;

        clientsServedStat = 0;
        moneySpentStat = 0;
        moneyEarnedStat = 0;

        dayText.text = "Day " + day;
        dayCanvas.enabled = true;

        yield return clickAnywhereController.AwaitInputCoroutine();

        dayCanvas.enabled = false;
        StartCoroutine(SpawnNPCCoroutine());
    }

    public void EnableRecap() {
        if (day == startingDialogueDays.Length) {
            endRecapUIController.EnableCanvas(money >= moneyOwed);
        } else {
            string[] stats = { clientsServedStat.ToString(), (5 - clientsServedStat).ToString(), moneySpentStat.ToString(), moneyEarnedStat.ToString(), Mathf.Max(RemainingMoneyOwed(), 0).ToString() };
            recapUIController.EnableCanvas(day, stats);
        }
    }

    private IEnumerator SpawnNPCCoroutine() {
        yield return new WaitForSeconds(1f);

        npcController.InitializeNPC(startingDialogueDays[day-1]);
    }

    public void DisableAllItemSelections() {
        foreach (ItemController item in items) {
            item.ToggleSelection(false);
        }
    }

    public void ToggleSelectedItem(ItemSO item, bool isSelected) {
        CraftingDialogueSO craftingDialogueSO = npcController.currentDialogueSO as CraftingDialogueSO;
        for (int i = 0; i < craftingDialogueSO.items.Length; i++) {
            if (item == craftingDialogueSO.items[i]) {
                if (isSelected) {
                    notepadTexts[i].text = $"<s>{craftingDialogueSO.items[i].itemName}</s>";

                    itemCount++;
                    BuyItem(item.price);
                    if (itemCount == craftingDialogueSO.items.Length) {
                        timerController.DisableCountDown();
                        notepadCanvas.enabled = false;
                        notepadSprite.enabled = false;

                        ChangeMoney(craftingDialogueSO.GetMoneyAmount());

                        isSelectingItems = false;
                        DisableAllItemSelections();

                        npcController.SelectNextDialogue("Successful");
                    }
                } else {
                    notepadTexts[i].text = craftingDialogueSO.items[i].itemName;

                    itemCount--;
                    RefundItem(item.price);
                }
                break;
            }
        }
    }

    public void EnableCrafting() {
        CraftingDialogueSO craftingDialogueSO = npcController.currentDialogueSO as CraftingDialogueSO;
        for (int i = 0; i < craftingDialogueSO.items.Length; i++) {
            notepadTexts[i].text = craftingDialogueSO.items[i].itemName;
        }
        notepadCanvas.enabled = true;
        notepadSprite.enabled = true;


        isSelectingItems = true;

        timerController.StartTimer(FailedCrafting, 10f);
    }

    private void FailedCrafting() {
        notepadCanvas.enabled = false;
        notepadSprite.enabled = false;

        isSelectingItems = false;
        DisableAllItemSelections();

        npcController.SelectNextDialogue("Failed");
    }

    private void BuyItem(int price) {
        if (!isSelectingItems) return;

        money -= price;
        moneyText.text = "$" + money;
        moneySpentStat += price;
        moneyDownAudio.Play();
        GameObject moneyChangeUIGO = Instantiate(moneyChangeUIPrefab, moneyChangeSpawnPosition, Quaternion.identity);
        moneyChangeUIGO.GetComponent<MoneyChangeUIController>().MoneyDown(price);
    }

    private void RefundItem(int price) {
        if (!isSelectingItems) return;

        money += price;
        moneyText.text = "$" + money;
        moneySpentStat -= price;
        moneyUpAudio.Play();
        GameObject moneyChangeUIGO = Instantiate(moneyChangeUIPrefab, moneyChangeSpawnPosition, Quaternion.identity);
        moneyChangeUIGO.GetComponent<MoneyChangeUIController>().MoneyUp(price);
    }

    public void AddMoney(int addedMoney) {
        money += addedMoney;
        moneyText.text = "$" + money;
        moneyEarnedStat += addedMoney;
        clientsServedStat++;
        moneyUpAudio.Play();
        GameObject moneyChangeUIGO = Instantiate(moneyChangeUIPrefab, moneyChangeSpawnPosition, Quaternion.identity);
        moneyChangeUIGO.GetComponent<MoneyChangeUIController>().MoneyUp(addedMoney);
    }

    public bool IsEnoughMoney(int requirementAmount) { return money >= requirementAmount; }

    public int RemainingMoneyOwed() { return moneyOwed - money; }

    public void GiveRemainingMoney() { ChangeMoney(moneyOwed - money); }

    public void ChangeMoney(int moneyAmount) {
        if (moneyAmount == 0) return;

        if (moneyAmount < 0) {
            money += moneyAmount;
            moneyText.text = "$" + money;
            moneyUpAudio.Play();
            GameObject moneyChangeUIGO = Instantiate(moneyChangeUIPrefab, moneyChangeSpawnPosition, Quaternion.identity);
            moneyChangeUIGO.GetComponent<MoneyChangeUIController>().MoneyUp(moneyAmount);
        } else {
            AddMoney(moneyAmount);
        }
    }
}
