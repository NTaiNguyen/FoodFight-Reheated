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
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        if (GameData.selectedP1 == null || GameData.selectedP2 == null)
        {
            Debug.LogError("GameManager: One or both player character prefabs are missing from GameData.");
            return;
        }

        // Instantiate Player 1
        GameObject p1 = Instantiate(prefab, player1Spawn.position, player1Spawn.rotation);
        AnimationController anim = p1.GetComponent<AnimationController>();
        anim.animSet = GameData.selectedP1.animationSet;
        p1.GetComponent<MovementScript>().playerID = 1;
        p1.GetComponent<ActionController>().playerID = 1;
        p1.name = "Player1";
        p1.tag = "Player1";


        // Instantiate Player 2
        GameObject p2 = Instantiate(prefab, player2Spawn.position, player2Spawn.rotation);
        anim = p2.GetComponent<AnimationController>();
        anim.animSet = GameData.selectedP2.animationSet;
        p2.GetComponent<MovementScript>().playerID = 2;
        p2.GetComponent<ActionController>().playerID = 2;
        p2.name = "Player2";
        p2.tag = "Player2";

        

        Debug.Log("Spawned Player 1 and Player 2 into the battle scene.");
    }
}