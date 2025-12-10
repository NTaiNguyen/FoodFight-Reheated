    using UnityEngine;
using UnityEngine.Rendering;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public GameObject[] maps;
    
    // For music
    public AudioSource musicSource;

    void Awake()
    {
        // Basic singleton setup
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public int GetMapCount()
    {
        return (maps != null) ? maps.Length : 0;
    }

    void Start()
    {
        if (maps == null || maps.Length == 0)
        {
            Debug.LogError("No maps assigned in MapManager!");
            return;
        }

        // Disable all maps first
        foreach (GameObject map in maps)
            map.SetActive(false);

        int index = GameData.selectedMapIndex;

        // Safety
        index = Mathf.Clamp(index, 0, maps.Length - 1);
        
        GameObject chosenMap = maps[index];
        chosenMap.SetActive(true);

        Debug.Log("Activated map at index: " + index);
        Debug.Log("Maps found: " + index);

        MapInfo info = chosenMap.GetComponent<MapInfo>();

        if (info != null && info.musicTrack != null)
        {
            musicSource.clip = info.musicTrack;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("No musicTrack found on" + chosenMap.name);
        }

        Debug.Log("Activated map index: " + index);
    }
}
