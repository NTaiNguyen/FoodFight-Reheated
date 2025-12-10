using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform targetPlayer;
    public float moveSpeed = 3.5f;
    public float attackDistance = 1.5f;
    public float decisionRate = 0.4f;

    private float nextDecision = 0f;
    private InputMapper inputMapper;
    private MovementScript movementScript;
    private Rigidbody2D rb;

    // Initialize immediately after you instantiate the players
    public void Initialize(Transform target, InputMapper mapper)
    {
        targetPlayer = target;
        inputMapper = mapper;
        if (inputMapper != null)
        {
            inputMapper.isAI = true;
            inputMapper.playerID = 2;
        }

        movementScript = GetComponent<MovementScript>();
        rb = GetComponent<Rigidbody2D>();

        Debug.Log($"AIController initialized on {gameObject.name}, target={target?.name}, mapper={(inputMapper!=null)}");
    }

    void Start()
    {
        if (inputMapper == null)
            inputMapper = GetComponent<InputMapper>();

        if (movementScript == null)
            movementScript = GetComponent<MovementScript>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (targetPlayer == null)
        {
            var p1go = GameObject.FindWithTag("Player1");
            if (p1go) targetPlayer = p1go.transform;
        }
    }

    void Update()
    {
        if (targetPlayer == null || movementScript == null) return;

        Vector2 toTarget = targetPlayer.position - transform.position;
        float dist = Mathf.Abs(toTarget.x);

        float desiredHorizontal = 0f;
        if (dist > attackDistance + 0.2f)
            desiredHorizontal = Mathf.Sign(toTarget.x) * moveSpeed;
        else if (dist < attackDistance - 0.2f)
            desiredHorizontal = -Mathf.Sign(toTarget.x) * (moveSpeed * 0.3f);

        float normalized = 0f;
        if (moveSpeed != 0)
            normalized = Mathf.Clamp(desiredHorizontal / moveSpeed, -1f, 1f);

        // Directly write into MovementScript's input fields
        movementScript.horizontalInput = normalized;
        movementScript.verticalInput = 0f;

        // Attack decisions
        if (Time.time >= nextDecision)
        {
            nextDecision = Time.time + decisionRate;

            if (dist <= attackDistance + 0.5f)
            {
                int r = Random.Range(0, 3);
                ButtonInput choice = (r == 0) ? ButtonInput.LIGHT : (r == 1) ? ButtonInput.MEDIUM : ButtonInput.HEAVY;

                if (inputMapper != null)
                {
                    inputMapper.ForcedButtonPress(choice);
                    Debug.Log($"AI forced button {choice}");
                }
            }
        }
    }
}
