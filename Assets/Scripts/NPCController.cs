using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private SpriteRenderer npcSprite;

    [SerializeField] private SpriteRenderer speechSprite;
    [SerializeField] private Canvas speechCanvas;
    [SerializeField] private TMP_Text speechText;

    [SerializeField] private Canvas choiceCanvas;
    [SerializeField] private GameObject choiceButtonPrefab;

    [SerializeField] private Canvas notepadCanvas;
    [SerializeField] private TMP_Text[] notepadTexts;

    [SerializeField] private ClickAnywhereController clickAnywhereController;

    private NPCSO npcSO;
    private DialogueSO currentDialogueSO;

    private int itemCount = 0;

    public void InitializeNPC(NPCSO newNPCSO) {
        npcSO = newNPCSO;
        currentDialogueSO = newNPCSO.dialogue;

        npcSprite.sprite = newNPCSO.npcSprite;
        npcSprite.enabled = true;

        StartCoroutine(InitializeDialogueCoroutine());

        // Enable the notepad
        for (int i = 0; i < npcSO.items.Length; i++) {
            notepadTexts[i].text = npcSO.items[i].itemName;
        }
        notepadCanvas.enabled = true;
    }

    private void EnableChoiceUI() {
        speechCanvas.enabled = false;
        speechSprite.enabled = false;

        choiceCanvas.enabled = true;
    }

    private void EnableSpeechUI() {
        speechCanvas.enabled = true;
        speechSprite.enabled = true;

        choiceCanvas.enabled = false;
    }

    public void ToggleSelectedItem(ItemSO item, bool isSelected) {
        for (int i = 0; i < npcSO.items.Length; i++) {
            if (item == npcSO.items[i]) {
                if (isSelected) {
                    notepadTexts[i].text = $"<s>{npcSO.items[i].itemName}</s>";

                    itemCount++;
                    if (itemCount == npcSO.items.Length) {
                        GameManager.Instance.AddMoney(npcSO.moneyOffer);

                        GameManager.Instance.isSelectingItems = false;
                        GameManager.Instance.DisableAllItemSelections();

                        SelectNextDialogue();
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

    public IEnumerator InitializeDialogueCoroutine() {
        // Initial dialogue text
        speechText.text = currentDialogueSO.GetDialogueText();
        EnableSpeechUI();

        // Check if this is a dialogue that you craft at.
        if (currentDialogueSO.DoesInitiateCrafting()) {
            GameManager.Instance.isSelectingItems = true;
            yield break;
        }

        // Wait for input to continue
        yield return clickAnywhereController.AwaitInputCoroutine();

        // Get the next dialogue
        if (currentDialogueSO.HasManyPaths()) {
            while (choiceCanvas.transform.childCount > 0) {
                Destroy(choiceCanvas.transform.GetChild(0).gameObject);
                yield return null;
            }

            string[] inputs = currentDialogueSO.GetNextInputs();

            foreach (string input in inputs) {
                GameObject choiceButtonGO = Instantiate(choiceButtonPrefab, choiceCanvas.transform);
                Button choiceButton = choiceButtonGO.GetComponent<Button>();
                TMP_Text choiceText = choiceButtonGO.GetComponentInChildren<TMP_Text>();

                choiceButton.onClick.RemoveAllListeners();
                choiceButton.onClick.AddListener(delegate { SelectNextDialogue(input); });

                choiceText.text = input;
            }

            EnableChoiceUI();
        } else {
            SelectNextDialogue();
        }
    }

    public void SelectNextDialogue(string choice = "") {
        currentDialogueSO = currentDialogueSO.GetNextDialogue(choice);

        if (currentDialogueSO != null) {
            StartCoroutine(InitializeDialogueCoroutine());
        } else {
            DespawnNPC();
        }
    }
}
