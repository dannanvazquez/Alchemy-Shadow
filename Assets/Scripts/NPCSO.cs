using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC")]
public class NPCSO : ScriptableObject {
    public Sprite npcSprite;

    public DialogueSO dialogue;
}