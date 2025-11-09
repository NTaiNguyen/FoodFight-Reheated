using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "FightingGame/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public AnimationSet animationSet;

}
