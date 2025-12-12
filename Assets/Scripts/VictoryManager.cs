using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public ActionController player1;
    public ActionController player2;

    // Changed to be text mesh pro and not an image
    public GameObject player1VictoryImage;
    public GameObject player2VictoryImage;

    // For background panel
    public GameObject player1VictoryPanel;
    public GameObject player2VictoryPanel;
    public AudioSource victoryMusic;
    public float returnToMenuDelay = 7f;

    private bool gameOver;

   void Start()
    {
        gameOver = false;

        // Hide both victory screens to start
        if (player1VictoryImage){
            player1VictoryImage.SetActive(false);
            // player1VictoryPanel.SetActive(false);
        }

        if (player2VictoryImage)
        {
            player2VictoryImage.SetActive(false);
            // player2VictoryImage.SetActive(false);
        }

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

        if (gameOver)
            return;

        // Health check
        if (player1.currentHealth <= 0)
            EndGame(2);
        else if (player2.currentHealth <= 0)
            EndGame(1);
    }

    void EndGame(int winner)
    {
        gameOver = true;

        player1.DisablePlayer();
        player2.DisablePlayer();

        //gets rid of player manager so character select can work again
        GameObject pm = GameObject.Find("PlayerManager");
        if (pm != null)
        {
            Destroy(pm);
            Debug.Log("Destroyed PlayerManager before loading match scene");
        }



        if (winner == 1 && player1VictoryImage)
        {
            player1VictoryImage.SetActive(true);
            // player1VictoryPanel.SetActive(true);
        }
        else if (winner == 2 && player2VictoryImage)
        {
            player2VictoryImage.SetActive(true);
            // player2VictoryPanel.SetActive(true);
        }
        

        if (victoryMusic) victoryMusic.Play();

        Invoke(nameof(ReturnToMainMenu), returnToMenuDelay);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}