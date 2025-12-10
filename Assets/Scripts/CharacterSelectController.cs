using UnityEngine;
using UnityEngine.SceneManagement;

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

    private GameData gameData;

    // public int totalMaps = 6;

    // New for AI
    private bool isP1vsAI = false;
    private int mode;

    void Start()
    {
        mode = PlayerPrefs.GetInt("GameMode", 1);
        Debug.Log("Mode selected is: (0 is AI and 2 is PVP)" + mode);
        gameData = GetComponent<GameData>();

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
                rows = 1;
                cols = characterSlots.Length;
                Debug.LogWarning($"Adjusted rows/cols to rows={rows}, cols={cols} to match characterSlots.Length ({characterSlots.Length}).");
            }
        }
    }

    void Update()
    {
        HandlePlayer1Input();

        // Handle P2 input only if it is in p1 vs p2 mode
        if (mode != 0)
        {
           HandlePlayer2Input(); 
        } else
        {
            // Randomly selecting character for P2
            if (mode == 0 && !p2Locked)
            {
                p2Row = Random.Range(0, rows);
                p2Col = Random.Range(0, cols);
                p2Index = Mathf.Clamp(p2Row * cols + p2Col, 0, characterSlots.Length - 1);
                p2Locked = true;
                Debug.Log($"P2 auto-selected character index {p2Index} for AI.");
                // Ensuring P2 is AI controlled
                gameData.isP2AI = true;
            }
        }
            

        if (!p1Locked)
            p1Index = Mathf.Clamp(p1Row * cols + p1Col, 0, characterSlots.Length - 1);

        if (!p2Locked)
            p2Index = Mathf.Clamp(p2Row * cols + p2Col, 0, characterSlots.Length - 1);

        if (characterSlots == null || characterSlots.Length == 0)
            return;

        // Move cursors
        if (p1Cursor != null && characterSlots[p1Index] != null)
        {
            p1Cursor.anchoredPosition = characterSlots[p1Index].anchoredPosition;
        }

        if (p2Cursor != null && characterSlots[p2Index] != null)
        {
            p2Cursor.anchoredPosition = characterSlots[p2Index].anchoredPosition;
        }
      

        if (p1Locked && p2Locked)
        {
            Debug.Log("Locked in");

            CharacterData dataP1 = gameData.bank.characters[p1Index];
            CharacterData dataP2 = gameData.bank.characters[p2Index];

            GameData.selectedP1 = dataP1;
            GameData.selectedP2 = dataP2;

            // GameData.totalMaps = totalMaps;

            // choose from available maps
            GameData.selectedMapIndex = Random.Range(0, GameData.totalMaps);
            Debug.Log("Selected map index = " + GameData.selectedMapIndex);

            PlayerPrefs.Save();
            SceneManager.LoadScene("SampleScene");
        }
    }

    void HandlePlayer1Input()
    {
        if (p1Locked) return;

        if (Input.GetKeyDown(KeyCode.W)) p1Row = Mathf.Max(0, p1Row - 1);
        if (Input.GetKeyDown(KeyCode.S)) p1Row = Mathf.Min(rows - 1, p1Row + 1);
        if (Input.GetKeyDown(KeyCode.A)) p1Col = Mathf.Max(0, p1Col - 1);
        if (Input.GetKeyDown(KeyCode.D)) p1Col = Mathf.Min(cols - 1, p1Col + 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            p1Index = Mathf.Clamp(p1Row * cols + p1Col, 0, characterSlots.Length - 1);
            p1Locked = true;
            Debug.Log($"Player 1 has selected. p1Index={p1Index}");
        }
    }

    void HandlePlayer2Input()
    {
        if (p2Locked) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) p2Row = Mathf.Max(0, p2Row - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) p2Row = Mathf.Min(rows - 1, p2Row + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) p2Col = Mathf.Max(0, p2Col - 1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) p2Col = Mathf.Min(cols - 1, p2Col + 1);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            p2Index = Mathf.Clamp(p2Row * cols + p2Col, 0, characterSlots.Length - 1);
            p2Locked = true;
            Debug.Log($"Player 2 has selected. p2Index={p2Index}");
        }
    }
}
