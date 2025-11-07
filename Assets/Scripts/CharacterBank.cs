using UnityEngine;

[CreateAssetMenu(fileName = "CharacterBank", menuName = "FightingGame/CharacterBank")]
public class CharacterBank : ScriptableObject
{
    public CharacterData[] characters;

    public CharacterData GetCharacter(string name) {
        foreach (var c in characters)
            if (c.characterName == name) return c;
        return null;
    }
}
