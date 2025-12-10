using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Called by the P1 vs AI button
    public void OnP1vsAI()
    {
        // Setting the game mode
        PlayerPrefs.SetInt("GameMode", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menus");
    }

    // Called by the P1 vs P2 button
    public void OnP1vsP2()
    {
        // Setting the game mode
        PlayerPrefs.SetInt("GameMode", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menus");
    }
}
