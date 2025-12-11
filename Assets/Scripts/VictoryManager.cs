using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("Players")]
    public ActionController player1;
    public ActionController player2;

    [Header("UI & Audio")]
    public GameObject player1VictoryImage;
    public GameObject player2VictoryImage;
    public AudioSource victoryMusic;     
    [Header("Settings")]
    public float returnToMenuDelay = 10f;

    private bool gameOver = false;

    void Start()
    {
        // Hide both victory images initially
        if (player1VictoryImage != null) player1VictoryImage.SetActive(false);
        if (player2VictoryImage != null) player2VictoryImage.SetActive(false);

        // Auto-find players by tag if not assigned
        if (player1 == null)
            player1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<ActionController>();
        if (player2 == null)
            player2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<ActionController>();

        if (player1 == null || player2 == null)
            Debug.LogWarning("[VictoryManager] Players not assigned or tagged correctly!");
    }

    void Update()
    {
        if (gameOver) return;

        if (player1 == null || player2 == null) return;

        if (player1.currentHealth <= 0)
        {
            EndGame(winner: 2);
        }
        else if (player2.currentHealth <= 0)
        {
            EndGame(winner: 1);
        }
    }

    // Finding players after they spawn
    public void AssignPlayers(GameObject p1, GameObject p2)
    {
        player1 = p1.GetComponent<ActionController>();
        player2 = p2.GetComponent<ActionController>();
        Debug.Log("[VictoryManager] Players dynamically assigned.");
    }

    private void EndGame(int winner)
    {
        gameOver = true;

        Debug.Log($"Player {winner} wins!");

        // Show correct victory image
        if (winner == 1 && player1VictoryImage != null)
            player1VictoryImage.SetActive(true);
        else if (winner == 2 && player2VictoryImage != null)
            player2VictoryImage.SetActive(true);

        // Play victory music
        if (victoryMusic != null)
            victoryMusic.Play();

        // Return to main menu after delay
        StartCoroutine(ReturnToMenuAfterDelay(returnToMenuDelay));
    }

    private System.Collections.IEnumerator ReturnToMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); 
        SceneManager.LoadScene("MainMenu"); 
    }

}
