using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC")]
public class NPCSO : ScriptableObject {
    public string npcName;
    public Sprite npcSprite;
    public NPCSO[] associatedNPCS;
}