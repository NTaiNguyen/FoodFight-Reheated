using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "FightingGame/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public AnimationSet animationSet;
    public int health;
    public GameObject healthbar; // make game object with sprite renderer and progress animation for health as it lowers


}
