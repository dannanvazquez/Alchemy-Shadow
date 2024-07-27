using UnityEngine;

[CreateAssetMenu(fileName = "CraftingDialogue", menuName = "ScriptableObjects/CraftingDialogue")]
public class CraftingDialogueSO : DialogueSO {
    public ItemSO[] items;
    public int moneyOffer;

    public override bool DoesInitiateCrafting() { return true; }
}
