using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Dialogue {
    [SerializeField] private string _dialogueText;
    [SerializeField] private string _inputText;
    [SerializeField] private bool _initiatesCrafting;
    [SerializeField] private Dialogue[] _dialogues;

    public string GetDialogueText() { return _dialogueText; }

    public string GetInputText() { return _inputText; }

    public bool DoesInitiateCrafting() { return _initiatesCrafting; }

    public bool HasManyPaths() { return _dialogues.Length > 1; }

    public string[] GetNextInputs() {
        string[] key = new string[_dialogues.Length];

        for (int i = 0; i < _dialogues.Length; i++) {
            key[i] = _dialogues[i].GetInputText();
        }

        return key;
    }

    public Dialogue GetNextDialogue(string input) {
        if (_dialogues.Length > 1) {
            foreach (Dialogue dialogue in _dialogues) {
                if (dialogue.GetInputText() == input) return dialogue;
            }
        } else if (_dialogues.Length == 1) {
            return _dialogues[0];
        }

        return null;
    }
}
