using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour
{
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

    // For spawning
    private GameData gameData;

    // Working on maps swapping
    // Hard coded so change this later
    public int totalMaps = 2;


    void Start()
    {
        gameData = GetComponent<GameData>();
        Debug.Log("CharacterSelectController is active.");

        // Basic validation and attempt to auto-correct cols if sizes mismatch
        if (characterSlots == null || characterSlots.Length == 0)
        {
            Debug.LogError("characterSlots is empty. Assign your RectTransforms in the Inspector.");
            enabled = false;
            return;
        }

        if (characterPortraits == null || characterPortraits.Length == 0)
        {
            Debug.LogWarning("characterPortraits is empty. Make sure you assign portrait GameObjects if used.");
        }

        int expected = rows * cols;
        if (characterSlots.Length != expected)
        {
            if (characterSlots.Length % rows == 0)
            {
                cols = characterSlots.Length / rows;
                Debug.LogWarning($"Adjusted cols -> {cols} to match characterSlots.Length ({characterSlots.Length}).");
            }
            else
            {
                // fallback: choose single row
                rows = 1;
                cols = characterSlots.Length;
                Debug.LogWarning($"Adjusted rows/cols to rows={rows}, cols={cols} to match characterSlots.Length ({characterSlots.Length}).");
            }
        }
    }

    void Update()
    {
        HandlePlayer1Input();
        HandlePlayer2Input();

        // Only update indices for players that are not locked.
        if (!p1Locked)
            p1Index = Mathf.Clamp(p1Row * cols + p1Col, 0, characterSlots.Length - 1);

        if (!p2Locked)
            p2Index = Mathf.Clamp(p2Row * cols + p2Col, 0, characterSlots.Length - 1);

        // Safety guard before accessing arrays
        if (characterSlots == null || characterSlots.Length == 0)
        {
            Debug.LogError("characterSlots not assigned or length 0.");
            return;
        }

        if (p1Index < 0 || p1Index >= characterSlots.Length)
        {
            Debug.LogError($"p1Index out of range: {p1Index} (slots: {characterSlots.Length})");
            return;
        }

        if (p2Index < 0 || p2Index >= characterSlots.Length)
        {
            Debug.LogError($"p2Index out of range: {p2Index} (slots: {characterSlots.Length})");
            return;
        }

        // Move cursors (only if the RectTransforms themselves are assigned)
        if (p1Cursor != null && characterSlots[p1Index] != null)
            p1Cursor.anchoredPosition = characterSlots[p1Index].anchoredPosition;
        if (p2Cursor != null && characterSlots[p2Index] != null)
            p2Cursor.anchoredPosition = characterSlots[p2Index].anchoredPosition;


        if (p1Locked && p2Locked)
{
            Debug.Log("Locked in");

            // // THIS DIDNT WORK
            // // Store character selection
            // if (characterPortraits != null && p1Index < characterPortraits.Length && p2Index < characterPortraits.Length)
            // {
            //     GameData.selectedP1 = characterPortraits[p1Index];
            //     GameData.selectedP2 = characterPortraits[p2Index];
            // }

            // GameData.characterP1 = (CharacterSelection)p1Index;
            // GameData.characterP2 = (CharacterSelection)p2Index;

            CharacterData dataP1 = gameData.bank.characters[p1Index];
            CharacterData dataP2 = gameData.bank.characters[p2Index];

            GameData.selectedP1 = dataP1;
            GameData.selectedP2 = dataP2;


            GameData.totalMaps = totalMaps;

            // HARDCODING THE NUMBER OF MAPS BECAUSE USING A VARIABLE DID NOT WORK.
            GameData.selectedMapIndex = Random.Range(0, 6);
            Debug.Log("Selected map index = " + GameData.selectedMapIndex);

            SceneManager.LoadScene("SampleScene");
        }
    }

    void HandlePlayer1Input()
    {
        if (p1Locked)
            return;

        if (Input.GetKeyDown(KeyCode.W)) p1Row = Mathf.Max(0, p1Row - 1);
        if (Input.GetKeyDown(KeyCode.S)) p1Row = Mathf.Min(rows - 1, p1Row + 1);
        if (Input.GetKeyDown(KeyCode.A)) p1Col = Mathf.Max(0, p1Col - 1);
        if (Input.GetKeyDown(KeyCode.D)) p1Col = Mathf.Min(cols - 1, p1Col + 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // lock index immediately on press to avoid race-frame issues
            p1Index = Mathf.Clamp(p1Row * cols + p1Col, 0, characterSlots.Length - 1);
            p1Locked = true;
            Debug.Log($"Player 1 has selected. p1Row={p1Row}, p1Col={p1Col}, p1Index={p1Index}");
        }
    }

    void HandlePlayer2Input()
    {
        if (p2Locked)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) p2Row = Mathf.Max(0, p2Row - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) p2Row = Mathf.Min(rows - 1, p2Row + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) p2Col = Mathf.Max(0, p2Col - 1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) p2Col = Mathf.Min(cols - 1, p2Col + 1);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // lock index immediately on press to avoid race-frame issues
            p2Index = Mathf.Clamp(p2Row * cols + p2Col, 0, characterSlots.Length - 1);
            p2Locked = true;
            Debug.Log($"Player 2 has selected. p2Row={p2Row}, p2Col={p2Col}, p2Index={p2Index}");
        }
    }
}
