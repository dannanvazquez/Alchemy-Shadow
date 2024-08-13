using UnityEngine;

[CreateAssetMenu(fileName = "DialogueRemainingMoney", menuName = "ScriptableObjects/DialogueRemainingMoney")]
public class DialogueRemainingMoneySO : DialogueSO {
    public override bool IsGivingRemainingMoney() { return true; }
}
