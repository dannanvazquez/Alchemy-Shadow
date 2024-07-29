using UnityEngine;

// TODO: Make a clean BaseDialogueSO that all dialogue will derive from. For example, most of these variables aren't needed in ConditionalDialogueSO.
[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class DialogueSO : ScriptableObject {
    [SerializeField] private NPCSO _npcSO;
    [SerializeField] private string _dialogueText;
    [SerializeField] private AudioClip _dialogueAudio;
    [SerializeField] private string _inputText;
    [SerializeField] private string memoryTag;
    [SerializeField] private DialogueSO[] _dialogues;

    public NPCSO GetNPC() { return _npcSO; }

    public string GetDialogueText() { return _dialogueText; }

    public AudioClip GetDialogueAudio() { return _dialogueAudio; }

    public string GetInputText() { return _inputText; }

    public bool HasMemoryTag() { return memoryTag != string.Empty; }
    public string GetMemoryTag() { return memoryTag; }

    public virtual bool DoesInitiateCrafting() { return false; }
    public virtual bool IsConditionalTag() { return false; }
    public virtual bool IsConditionalMoney() { return false; }

    public bool HasManyPaths() { return _dialogues.Length > 1; }

    public string[] GetNextInputs() {
        string[] key = new string[_dialogues.Length];

        for (int i = 0; i < _dialogues.Length; i++) {
            key[i] = _dialogues[i].GetInputText();
        }

        return key;
    }

    public DialogueSO GetNextDialogue(string input) {
        if (_dialogues.Length > 1) {
            foreach (DialogueSO dialogue in _dialogues) {
                if (dialogue.GetInputText() == input) return dialogue;
            }
        } else if (_dialogues.Length == 1) {
            return _dialogues[0];
        }

        return null;
    }
}
