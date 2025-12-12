using UnityEngine;

public class TimeResetter : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}

// This script resets the time at the beginning of every scene so that nothing gets messed up with the slow mo and stuff
// Its not being useds