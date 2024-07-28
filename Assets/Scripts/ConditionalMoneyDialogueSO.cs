using UnityEngine;

[CreateAssetMenu(fileName = "ConditionalMoneyDialogue", menuName = "ScriptableObjects/ConditionalMoneyDialogue")]
public class ConditionalMoneyDialogueSO : DialogueSO {
    public int minimumMoneyAmount;

    public override bool IsConditionalMoney() { return true; }
}
