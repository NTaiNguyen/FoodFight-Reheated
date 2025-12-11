using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public ActionController player1;
    public ActionController player2;

    public GameObject player1VictoryImage;
    public GameObject player2VictoryImage;
    public AudioSource victoryMusic;
    public float returnToMenuDelay = 10f;

    private bool gameOver;

   void Start()
    {
        gameOver = false;

        // Hide both victory screens to start
        if (player1VictoryImage) player1VictoryImage.SetActive(false);
        if (player2VictoryImage) player2VictoryImage.SetActive(false);

        // Auto-assign players by tag if not set
        if (!player1) player1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<ActionController>();
        if (!player2) player2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<ActionController>();
    }

    void Update()
    {
        // Auto-assign players if they spawn after Start
        if (!player1)
            player1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<ActionController>();
        if (!player2)
            player2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<ActionController>();

        if (player1)
            Debug.Log("Found Player1: " + player1.name);

        if (player2)
            Debug.Log("Found Player2: " + player2.name);

        if (!player1 || !player2)
            return; 

        if (gameOver) return;

        // Health check
        if (player1.currentHealth <= 0)
            EndGame(2);
        else if (player2.currentHealth <= 0)
            EndGame(1);
    }

    void EndGame(int winner)
    {
        gameOver = true;

        // // Slow mo finish
        // Time.timeScale = 0.2f;
        // Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (winner == 1 && player1VictoryImage) player1VictoryImage.SetActive(true);
        else if (winner == 2 && player2VictoryImage) player2VictoryImage.SetActive(true);

        if (victoryMusic) victoryMusic.Play();

        Invoke(nameof(ReturnToMainMenu), returnToMenuDelay);

        // Abandonded slow mo
        // so sad
        // StartCoroutine(SlowMoEnd(winner));
    }

    IEnumerator SlowMoEnd(int winner)
    {
        // Wait a bit
        yield return new WaitForSecondsRealtime(0.65f);

        // Restore time
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // From above, show winner screen and play music
        if (winner == 1 && player1VictoryImage) player1VictoryImage.SetActive(true);
        else if (winner == 2 && player2VictoryImage) player2VictoryImage.SetActive(true);
        if (victoryMusic) victoryMusic.Play();

        // Wait to return to menu
        yield return new WaitForSeconds(returnToMenuDelay);

        ReturnToMainMenu();
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}