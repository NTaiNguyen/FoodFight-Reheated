using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Called by the P1 vs AI button
    public void OnP1vsAI()
    {
        PlayerPrefs.SetInt("GameMode", 0); // optional, if you want to store game mode
        SceneManager.LoadScene("menus");
    }

    // Called by the P1 vs P2 button
    public void OnP1vsP2()
    {
        PlayerPrefs.SetInt("GameMode", 1); // optional
        SceneManager.LoadScene("Menus");
    }
}
