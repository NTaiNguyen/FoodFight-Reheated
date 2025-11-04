using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour
{
    // Slots for the characters
    public RectTransform[] characterSlots;
    public RectTransform p1Cursor;
    public RectTransform p2Cursor;
    public GameObject[] characterPortraits;

    public int rows = 2;
    public int cols = 3;

    private int p1Index = 0;
    private int p2Index = 1;

    private int p1Row = 0, p1Col = 1;
    private int p2Row = 0, p2Col = 2;

    private bool p1Locked = false;
    private bool p2Locked = false;

    void Start()
    {
        Debug.Log("CharacterSelectController is active.");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {
            Debug.Log("Key pressed this frame.");
        }

        HandlePlayer1Input();
        HandlePlayer2Input();

        int p1Index = Mathf.Clamp(p1Row * cols + p1Col, 0, characterSlots.Length - 1);
        int p2Index = Mathf.Clamp(p2Row * cols + p2Col, 0, characterSlots.Length - 1);

        p1Cursor.anchoredPosition = characterSlots[p1Index].anchoredPosition;
        p2Cursor.anchoredPosition = characterSlots[p2Index].anchoredPosition;


        if (p1Locked && p2Locked)
        {
            GameData.selectedP1 = characterPortraits[p1Index];
            GameData.selectedP2 = characterPortraits[p2Index];

            // Change this later to whatever scene we use
            SceneManager.LoadScene("SampleScene");
        }
    }

    void HandlePlayer1Input()
    {
        if (p1Locked)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W)) p1Row = Mathf.Max(0, p1Row - 1);
        if (Input.GetKeyDown(KeyCode.S)) p1Row = Mathf.Min(rows - 1, p1Row + 1);
        if (Input.GetKeyDown(KeyCode.A)) p1Col = Mathf.Max(0, p1Col - 1);
        if (Input.GetKeyDown(KeyCode.D)) p1Col = Mathf.Min(cols - 1, p1Col + 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            p1Locked = true;
            Debug.Log("Player 1 has selected");
        }
    }

    void HandlePlayer2Input()
    {
        if (p2Locked)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) p2Row = Mathf.Max(0, p2Row - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) p2Row = Mathf.Min(rows - 1, p2Row + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) p2Col = Mathf.Max(0, p2Col - 1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) p2Col = Mathf.Min(cols - 1, p2Col + 1);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            p2Locked = true;
            Debug.Log("Player 2 has selected");
        }
    }
}
