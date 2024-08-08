using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private SpriteRenderer npcSprite;
    [SerializeField] private AudioSource npcAudioSource;

    [SerializeField] private SpriteRenderer speechSprite;
    [SerializeField] private Canvas speechCanvas;
    [SerializeField] private TMP_Text speechText;

    [SerializeField] private Canvas choiceCanvas;
    [SerializeField] private GameObject choiceButtonPrefab;

    [SerializeField] private ClickAnywhereController clickAnywhereController;

    [Header("Settings")]
    [Tooltip("The amount of seconds to transition entering and leaving NPCs.")]
    [SerializeField] private float transitionTime;

    private NPCSO previousNPC;

    private List<string> memoryTags = new();

    public DialogueSO currentDialogueSO { get; private set; }

    public void InitializeNPC(DialogueSO initialDialogue) {
        currentDialogueSO = initialDialogue;

        previousNPC = null;
        npcSprite.color = new Color(npcSprite.color.r, npcSprite.color.g, npcSprite.color.b, 0);
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
        npcAudioSource.Stop();

        GameManager.Instance.EnableRecap();
    }

    public IEnumerator InitializeDialogueCoroutine() {
        npcAudioSource.Stop();
        speechCanvas.enabled = false;
        speechSprite.enabled = false;

        // Add memory tag for future conditionals.
        if (currentDialogueSO.HasMemoryTag()) {
            memoryTags.Add(currentDialogueSO.GetMemoryTag());
        }

        // Check if the dialogue is a conditional.
        if (currentDialogueSO.IsConditionalTag()) {
            if (memoryTags.Contains((currentDialogueSO as ConditionalTagDialogueSO).conditionalTag)) {
                SelectNextDialogue("True");
            } else {
                SelectNextDialogue("False");
            }
            yield break;
        } else if (currentDialogueSO.IsConditionalMoney()) {
            if (GameManager.Instance.IsEnoughMoney((currentDialogueSO as ConditionalMoneyDialogueSO).minimumMoneyAmount)) {
                SelectNextDialogue("True");
            } else {
                SelectNextDialogue("False");
            }
            yield break;
        }

        // Initial dialogue
        NPCSO npcSO = currentDialogueSO.GetNPC();
        yield return StartCoroutine(SwitchNpcSpriteCoroutine(npcSO));

        speechText.text = $"{npcSO.npcName}: {currentDialogueSO.GetDialogueText()}";
        if (speechText.text.Contains("[moneyLeft]")) {
            speechText.text = speechText.text.Replace("[moneyLeft]", GameManager.Instance.RemainingMoneyOwed().ToString());
        }
        EnableSpeechUI();
        npcAudioSource.clip = currentDialogueSO.GetDialogueAudio();
        npcAudioSource.Play();


        // Check if this is a dialogue that you craft at.
        if (currentDialogueSO.DoesInitiateCrafting()) {
            GameManager.Instance.EnableCrafting();
            yield break;
        }

        // Wait for input to continue
        yield return clickAnywhereController.AwaitInputCoroutine();

        npcAudioSource.Stop();
        GameManager.Instance.ChangeMoney(currentDialogueSO.GetMoneyAmount());

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

    private IEnumerator SwitchNpcSpriteCoroutine(NPCSO npcSO) {
        if (previousNPC == null) {
            npcSprite.sprite = npcSO.npcSprite;

            yield return StartCoroutine(ToggleFadeNPCCoroutine(true));
        } else if (previousNPC != npcSO) {
            if (previousNPC.associatedNPCS.Contains(npcSO)) {
                npcSprite.sprite = npcSO.npcSprite;
            } else {
                yield return StartCoroutine(ToggleFadeNPCCoroutine(false));

                npcSprite.sprite = npcSO.npcSprite;

                yield return StartCoroutine(ToggleFadeNPCCoroutine(true));
            }
        }
        previousNPC = npcSO;
    }

    private IEnumerator ToggleFadeNPCCoroutine(bool isEntering) {
        float elapsedTime = 0f;
        Color initialColor = npcSprite.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, isEntering ? 1 : 0);

        while (elapsedTime < transitionTime) {
            elapsedTime += Time.deltaTime;
            npcSprite.color = Color.Lerp(initialColor, targetColor, elapsedTime / transitionTime);
            yield return null;
        }
    }
}
