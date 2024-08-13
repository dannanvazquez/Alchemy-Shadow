using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConditionalMultiTagDialogue", menuName = "ScriptableObjects/ConditionalMultiTagDialogue")]
public class ConditionalMultiTagDialogueSO : DialogueSO {
    [SerializeField] private string[] conditionalTags;
    [SerializeField] private int minimumTagCount;

    public override bool IsConditionalMultiTag() { return true; }

    public bool MeetsRequirements(List<string> memoryTags) {
        int tagCount = 0;
        foreach (string tag in conditionalTags) {
            if (memoryTags.Contains(tag)) tagCount++;
        }

        return tagCount >= minimumTagCount;
    }
}
