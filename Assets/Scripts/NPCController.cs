using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private SpriteRenderer npcSprite;

    [SerializeField] private SpriteRenderer speechSprite;
    [SerializeField] private Canvas speechCanvas;
    [SerializeField] private TMP_Text speechText;

    [SerializeField] private Canvas choiceCanvas;
    [SerializeField] private GameObject choiceButtonPrefab;

    [SerializeField] private ClickAnywhereController clickAnywhereController;

    private List<string> memoryTags = new();

    public DialogueSO currentDialogueSO { get; private set; }

    public void InitializeNPC(DialogueSO initialDialogue) {
        currentDialogueSO = initialDialogue;

        npcSprite.sprite = initialDialogue.GetNPC().npcSprite;
        npcSprite.enabled = true;

        StartCoroutine(InitializeDialogueCoroutine());
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

    private void DespawnNPC() {
        GameManager.Instance.isSelectingItems = false;

        npcSprite.enabled = false;
        speechCanvas.enabled = false;
        speechSprite.enabled = false;
        choiceCanvas.enabled = false;

        GameManager.Instance.EnableRecap();
    }

    public IEnumerator InitializeDialogueCoroutine() {
        // Add memory tag for future conditionals.
        if (currentDialogueSO.HasMemoryTag()) {
            memoryTags.Add(currentDialogueSO.GetMemoryTag());
        }

        // Check if the dialogue is a conditional.
        if (currentDialogueSO.IsConditional()) {
            if (memoryTags.Contains((currentDialogueSO as ConditionalTagDialogueSO).conditionalTag)) {
                SelectNextDialogue("True");
            } else {
                SelectNextDialogue("False");
            }
            yield break;
        }

        // Initial dialogue
        NPCSO npcSO = currentDialogueSO.GetNPC();
        speechText.text = $"{npcSO.npcName}: {currentDialogueSO.GetDialogueText()}";
        EnableSpeechUI();
        npcSprite.sprite = npcSO.npcSprite;

        // Check if this is a dialogue that you craft at.
        if (currentDialogueSO.DoesInitiateCrafting()) {
            GameManager.Instance.EnableCrafting();
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
