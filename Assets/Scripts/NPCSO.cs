using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC")]
public class NPCSO : ScriptableObject {
    public Sprite npcSprite;

    public Dialogue dialogue;

    public ItemSO[] items;
    public int moneyOffer;
}