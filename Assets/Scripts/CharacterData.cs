using UnityEngine;

[System.Serializable]
public class AttackData {
    public string attackName;
    public Vector2 hitboxSize;
    public Vector2 hitboxOffset;
    public int damage;
    public int activeFrameStart;
    public int activeFrameEnd;
    public Color gizmoColor;

}
[CreateAssetMenu(fileName = "CharacterData", menuName = "FightingGame/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public AnimationSet animationSet;
    public int health;
    public GameObject healthbar; // make game object with sprite renderer and progress animation for health as it lowers
    public AttackData[] attacks;

}
