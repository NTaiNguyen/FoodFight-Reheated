using NUnit.Framework;
using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public Transform player1Spawn;
        public Transform player2Spawn;
        
        public GameObject prefab;
        private GameData _data;

    void Awake()
    {
        _data = GetComponent<GameData>();
        //hard code in spawn point
        if (player1Spawn == null)
        {
            GameObject go = new GameObject("P1Spawn");
            go.transform.position = new Vector3(-5f, 0f, 0f);
            player1Spawn = go.transform;
        }

        if (player2Spawn == null)
        {
            GameObject go = new GameObject("P2Spawn");
            go.transform.position = new Vector3(5f, 0f, 0f);
            player2Spawn = go.transform;
        }
    }

    void Start()
    {
        // Debug messages for the AI mode
        int mode = PlayerPrefs.GetInt("GameMode", -99);
        Debug.Log("GameManager GameMode = " + mode);
        Debug.Log("GameManager selectedP1 = " + (GameData.selectedP1 != null));
        Debug.Log("GameManager selectedP2 = " + (GameData.selectedP2 != null));
        
        // Spawning the players
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        // Checking to see if the gamemode is selected correctly
        Debug.Log("GameManager GameMode = " + PlayerPrefs.GetInt("GameMode", -99));

        if (GameData.selectedP1 == null || GameData.selectedP2 == null)
        {
            Debug.LogError("GameManager: missing selected characters.");
            return;
        }

        // Instantiate P1
        GameObject p1 = Instantiate(prefab, player1Spawn.position, player1Spawn.rotation);
        p1.name = "Player1";
        p1.tag = "Player1";
        var anim1 = p1.GetComponent<AnimationController>();
        if (anim1 != null) anim1.animSet = GameData.selectedP1.animationSet;
        p1.GetComponent<MovementScript>().playerID = 1;
        p1.GetComponent<ActionController>().playerID = 1;

        // Instantiate P2
        GameObject p2 = Instantiate(prefab, player2Spawn.position, player2Spawn.rotation);
        p2.name = "Player2";
        p2.tag = "Player2";
        var anim2 = p2.GetComponent<AnimationController>();
        if (anim2 != null) anim2.animSet = GameData.selectedP2.animationSet;
        p2.GetComponent<MovementScript>().playerID = 2;
        p2.GetComponent<ActionController>().playerID = 2;

        // Ensure InputMapper exists on both
        InputMapper mapperP1 = p1.GetComponent<InputMapper>();
        if (mapperP1 == null) mapperP1 = p1.AddComponent<InputMapper>();
        mapperP1.playerID = 1;
        mapperP1.isAI = false;

        InputMapper mapperP2 = p2.GetComponent<InputMapper>();
        if (mapperP2 == null) mapperP2 = p2.AddComponent<InputMapper>();
        mapperP2.playerID = 2;

        int mode = PlayerPrefs.GetInt("GameMode", 1);

        if (mode == 0) 
        {
            mapperP2.isAI = true;

            // Add AIController and initialize it with a reference to P1
            AIController ai = p2.GetComponent<AIController>();
            if (ai == null) ai = p2.AddComponent<AIController>();

            ai.Initialize(p1.transform, mapperP2);

            Debug.Log("GameManager: configured P2 as AI");
        }
        else
        {
            mapperP2.isAI = false;
        }

        Debug.Log("SpawnPlayers finished.");
    }

}

