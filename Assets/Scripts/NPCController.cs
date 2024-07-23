using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NPCController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private SpriteRenderer npcSprite;
    [SerializeField] private SpriteRenderer speechSprite;
    [SerializeField] private Canvas speechCanvas;
    [SerializeField] private TMP_Text speechText;
    [SerializeField] private Canvas choiceCanvas;
    [SerializeField] private Canvas notepadCanvas;
    [SerializeField] private ClickAnywhereController clickAnywhereController;
    [SerializeField] private TMP_Text[] notepadTexts;

    private NPCSO npcSO;

    private int itemCount = 0;

    public void InitializeNPC(NPCSO newNPCSO) {
        npcSO = newNPCSO;

        npcSprite.sprite = newNPCSO.npcSprite;
        npcSprite.enabled = true;

        speechText.text = newNPCSO.engageDialogue;
        speechCanvas.enabled = true;
        speechSprite.enabled = true;

        for (int i = 0; i < npcSO.items.Length; i++) {
            notepadTexts[i].text = npcSO.items[i].itemName;
        }
        notepadCanvas.enabled = true;

        UnityEvent tempEvent = new();
        tempEvent.AddListener(EnableChoiceUI);
        clickAnywhereController.AwaitInput(tempEvent);
    }

    private void EnableChoiceUI() {
        speechCanvas.enabled = false;
        speechSprite.enabled = false;

        choiceCanvas.enabled = true;
    }

    private void DisableChoiceUI() {
        speechCanvas.enabled = true;
        speechSprite.enabled = true;

        choiceCanvas.enabled = false;
    }

    public void AcceptOffer() {
        GameManager.Instance.isSelectingItems = true;

        speechText.text = npcSO.acceptDialogue;
        DisableChoiceUI();
    }

    public void DeclineOffer() {
        DespawnNPC();
    }

    public void ToggleSelectedItem(ItemSO item, bool isSelected) {
        for (int i = 0; i < npcSO.items.Length; i++) {
            if (item == npcSO.items[i]) {
                if (isSelected) {
                    notepadTexts[i].text = $"<s>{npcSO.items[i].itemName}</s>";

                    itemCount++;
                    if (itemCount == npcSO.items.Length) {
                        GameManager.Instance.AddMoney(npcSO.moneyOffer);
                        DespawnNPC();
                    }
                } else {
                    notepadTexts[i].text = npcSO.items[i].itemName;

                    itemCount--;
                }
                break;
            }
        }
    }

    private void DespawnNPC() {
        GameManager.Instance.isSelectingItems = false;

        npcSprite.enabled = false;
        speechCanvas.enabled = false;
        speechSprite.enabled = false;
        notepadCanvas.enabled = false;
        choiceCanvas.enabled = false;

        GameManager.Instance.SpawnNPC();
    }
}
