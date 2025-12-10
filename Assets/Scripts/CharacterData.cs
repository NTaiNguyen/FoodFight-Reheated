using UnityEngine;

[System.Serializable]
public class HitboxData {
    public Vector2 size;
    public Vector2 offset;
    public int activeFrameStart;
    public int activeFrameEnd;
    public int damage;
    public float hitStun;
    public Color gizmoColor = new Color(1f, 0f, 0f, 0f);
}

[System.Serializable]
public class HurtboxData {
    public Vector2 size;
    public Vector2 offset;
    public int invincibilityFrames;
    public Color gizmoColor = new Color(0f, 0f, 1f, 0f);
}
[System.Serializable]
public class AttackData {
    public string attackName;
    public BoxCollider2D hitboxPrefab;
    public BoxCollider2D hurtboxPrefab;
    public HitboxData[] hitboxes;
    public HurtboxData[] hurtboxes;
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
