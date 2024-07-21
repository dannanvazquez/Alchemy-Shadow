using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC")]
public class NPCSO : ScriptableObject {
    public Sprite npcSprite;

    public string engageDialogue;
    public string acceptDialogue;

    public ItemSO[] items;
    public int moneyOffer;
}