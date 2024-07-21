using System.Collections;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private SpriteRenderer npcSprite;
    [SerializeField] private SpriteRenderer speechSprite;
    [SerializeField] private Canvas speechCanvas;
    [SerializeField] private TMP_Text speechText;
    [SerializeField] private Canvas choiceCanvas;
    [SerializeField] private Canvas notepadCanvas;
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
            notepadTexts[i].text = "O " + npcSO.items[i].itemName;
        }
        notepadCanvas.enabled = true;

        StartCoroutine(WaitForInitialInputCoroutine());
    }

    private IEnumerator WaitForInitialInputCoroutine() {
        // Wait until the input is true before continuing.
        while (!Input.GetKeyDown(KeyCode.Mouse0)) {
            yield return null;
        }

        ToggleChoice(true);
    }

    private void ToggleChoice(bool isChoosing) {
        speechCanvas.enabled = !isChoosing;
        speechSprite.enabled = !isChoosing;

        choiceCanvas.enabled = isChoosing;
    }

    public void AcceptOffer() {
        GameManager.Instance.isSelectingItems = true;

        speechText.text = npcSO.acceptDialogue;
        ToggleChoice(false);
    }

    public void DeclineOffer() {
        DespawnNPC();
    }

    public void ToggleSelectedItem(ItemSO item, bool isSelected) {
        for (int i = 0; i < npcSO.items.Length; i++) {
            if (item == npcSO.items[i]) {
                if (isSelected) {
                    notepadTexts[i].text = "<color=red>O</color> " + npcSO.items[i].itemName;

                    itemCount++;
                    if (itemCount == npcSO.items.Length) {
                        GameManager.Instance.AddMoney(npcSO.moneyOffer);
                        DespawnNPC();
                    }
                } else {
                    notepadTexts[i].text = "O " + npcSO.items[i].itemName;

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
