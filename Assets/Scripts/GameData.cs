using UnityEngine;
public enum CharacterSelection {
    Pizza,
    Taco,
    Pancake,
    Burger,
    Ramen,
    IceCream
}

// Stores what characters the players have selected so they can then be placed into the match
// Also stores the maps
public static class GameData
{
    public static GameObject selectedP1;
    public static GameObject selectedP2;
    public static CharacterSelection characterP1, characterP2;
    public static CharacterBank bank;
    public static int selectedMapIndex;
    
    // Change this later when more maps are added
    public static int totalMaps = 2;


}
