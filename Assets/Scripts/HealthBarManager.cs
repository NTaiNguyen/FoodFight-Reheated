using UnityEngine;
using TMPro;

public class HealthBarManager : MonoBehaviour
{
    public TextMeshProUGUI p1HealthText;
    public TextMeshProUGUI p2HealthText;

    private ActionController player1;
    private ActionController player2;

    public void RegisterPlayers(ActionController p1, ActionController p2)
    {
        player1 = p1;
        player2 = p2;
    }

    // Update is called once per frame
    void Update()
    {
        if (player1 != null)
            p1HealthText.text = $"P1: {player1.currentHealth}";
        
        if (player2 != null)
            p2HealthText.text = $"P2: {player2.currentHealth}";
    }
}
