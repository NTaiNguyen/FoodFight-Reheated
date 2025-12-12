using UnityEditor.PackageManager;
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
public class GameData : MonoBehaviour
{

    // For character spawning
    public static CharacterData selectedP1;
    public static CharacterData selectedP2;
    public static CharacterSelection characterP1, characterP2;
    public CharacterBank bank;
    public static int selectedMapIndex;

    // Change this later when more maps are added
    public static int totalMaps = 6;
    
    public static GameData Instance {get; private set;}


    // To track AI mode
    public bool isP2AI = false;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void ResetGameData()
    {
        selectedP1 = null;
        selectedP2 = null;

        characterP1 = 0;
        characterP2 = 0;

        selectedMapIndex = -1;

        if (Instance != null)
        {
            Instance.isP2AI = false;
        }
    }



}
