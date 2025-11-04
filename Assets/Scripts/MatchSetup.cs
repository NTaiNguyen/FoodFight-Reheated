using UnityEngine;

public class MatchSetup : MonoBehaviour
{
    public Transform p1Spawn;
    public Transform p2Spawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameData.selectedP1 != null && GameData.selectedP2 != null)
        {
            Instantiate(GameData.selectedP1, p1Spawn.position, Quaternion.identity);
            Instantiate(GameData.selectedP2, p2Spawn.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Character select did not work.");
        }
    }
}
