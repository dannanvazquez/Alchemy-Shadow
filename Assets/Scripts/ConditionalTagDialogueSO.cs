using UnityEngine;

[CreateAssetMenu(fileName = "ConditionalTagDialogue", menuName = "ScriptableObjects/ConditionalTagDialogue")]
public class ConditionalTagDialogueSO : DialogueSO {
    public string conditionalTag;

    public override bool IsConditionalTag() { return true; }
}
