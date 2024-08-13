using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConditionalTagDialogue", menuName = "ScriptableObjects/ConditionalTagDialogue")]
public class ConditionalTagDialogueSO : DialogueSO {
    [SerializeField] private string conditionalTag;

    public override bool IsConditionalTag() { return true; }

    public bool MeetsRequirements(List<string> memoryTags) {
        return memoryTags.Contains(conditionalTag);
    }
}
