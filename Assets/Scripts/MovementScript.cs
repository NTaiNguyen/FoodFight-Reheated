using UnityEngine;

public enum MoveState {
    STAND, CROUCH, JUMP, WALK, FALL, ARIAL
}
public enum ActionState {
    NONE, SL, SM, SH, CL, CM, CH, JL, JM, JH, SP1, SP2, BLOCK, CBLOCK
}
public enum InputDirection {
    Up = 8, Right = 6, Down = 2, Left = 4,
    DownLeft = 1, DownRight = 3, UpLeft = 7, UpRight = 9, Neutral = 5
}

public class MovementScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D box;
    private Vector2 defaultSize, defaultOffset;
    private float crouchHeight;
    private bool isGrounded;
    public float moveSpeed = 3.8f;
    public float jumpHeight = 9.5f;
    public MoveState sMove { get; private set; }
    private InputDirection direction = InputDirection.Neutral;
    private Vector2 input;
    private ActionController _action;

    // For AI control / input abstraction (public so AIController can write directly)
    public float horizontalInput = 0f;
    public float verticalInput = 0f;
    private InputMapper inputMapper;

    [Header("Player Config")]
    public int playerID = 1;

    private string horizontalAxis;
    private string verticalAxis;

    void Start()
    {
        // get references
        inputMapper = GetComponent<InputMapper>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        _action = GetComponent<ActionController>();

        if (box != null) {
            defaultSize = box.size;
            defaultOffset = box.offset;
            crouchHeight = defaultSize.y * .75f;
        }

        if (playerID == 1)
        {
            horizontalAxis = "Horizontal_P1";
            verticalAxis = "Vertical_P1";
        }
        else if (playerID == 2)
        {
            horizontalAxis = "Horizontal_P2";
            verticalAxis = "Vertical_P2";
        }
        else
        {
            Debug.LogError("MovementScript: Invalid playerID, must be 1 or 2!");
        }
    }

    void Update()
    {
        // Update horizontalInput / verticalInput from human input ONLY if an InputMapper exists and says it's human
        if (inputMapper != null && !inputMapper.isAI)
        {
            horizontalInput = Input.GetAxisRaw(horizontalAxis);
            verticalInput = Input.GetAxisRaw(verticalAxis);
        }
        // If there is no inputMapper at all, we also allow human input (legacy): fallback to axes
        else if (inputMapper == null)
        {
            horizontalInput = Input.GetAxisRaw(horizontalAxis);
            verticalInput = Input.GetAxisRaw(verticalAxis);
        }
        // If inputMapper.isAI == true, we DO NOT read Unity axes; AI or external code sets horizontalInput/verticalInput

        if (_action != null && _action.isAttacking && isGrounded)
        {
            // stop horizontal movement while grounded and attacking
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            HandleMovement();
        }

        UpdateState();
    }

    private void HandleMovement()
    {
        if (_action != null && _action.isAttacking) return;
        if (!isGrounded) return;

        // Compose the input vector from the abstracted values (human or AI)
        input = new Vector2(horizontalInput, verticalInput);

        // apply small deadzone for vertical
        if (Mathf.Abs(input.y) < 0.1f) input.y = 0f;

        direction = GetDirection(input);

        switch (direction)
        {
            case InputDirection.Right:
                rb.linearVelocity = new Vector2(moveSpeed * input.x, rb.linearVelocity.y);
                break;

            case InputDirection.Left:
                rb.linearVelocity = new Vector2(moveSpeed * input.x * 0.8f, rb.linearVelocity.y);
                break;

            case InputDirection.Down:
            case InputDirection.DownLeft:
            case InputDirection.DownRight:
                rb.linearVelocity = Vector2.zero;
                break;

            case InputDirection.Up:
            case InputDirection.UpLeft:
            case InputDirection.UpRight:
                rb.linearVelocity = new Vector2(moveSpeed * input.x, jumpHeight);
                isGrounded = false;
                break;

            case InputDirection.Neutral:
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                break;
        }
    }

    private bool crouchLocked = false;

    private void UpdateState()
    {
        // USE the AI/human abstracted input values — do NOT call Input.GetAxisRaw here
        float vertical = verticalInput;
        float deadzone = 0.1f;
        Vector2 directionalInput = new Vector2(horizontalInput, verticalInput);
        InputDirection dir = GetDirection(directionalInput);

        if (!isGrounded)
        {
            sMove = rb.linearVelocity.y > 0 ? MoveState.JUMP : MoveState.FALL;
        }
        else
        {
            if (vertical < -deadzone)
            {
                sMove = MoveState.CROUCH;
                crouchLocked = true;
            }
            else if (crouchLocked && vertical >= -deadzone)
            {
                crouchLocked = false;
                sMove = Mathf.Abs(rb.linearVelocity.x) > 0 && dir != InputDirection.Neutral ? MoveState.WALK : MoveState.STAND;
            }
            else if (!crouchLocked)
            {
                sMove = Mathf.Abs(rb.linearVelocity.x) > 0 && dir != InputDirection.Neutral ? MoveState.WALK : MoveState.STAND;
            }
        }

        SetCrouch(sMove == MoveState.CROUCH);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            sMove = MoveState.STAND;
        }

        if (collision.gameObject.CompareTag("Wall left") || collision.gameObject.CompareTag("Wall right"))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private InputDirection GetDirection(Vector2 inVec)
    {
        float x = inVec.x;
        float y = inVec.y;

        if (y > 0.5f && Mathf.Abs(x) < 0.5f) return InputDirection.Up;
        if (y < -0.5f && Mathf.Abs(x) < 0.5f) return InputDirection.Down;
        if (x < -0.5f && Mathf.Abs(y) < 0.5f) return InputDirection.Left;
        if (x > 0.5f && Mathf.Abs(y) < 0.5f) return InputDirection.Right;
        if (x < -0.5f && y > 0.5f) return InputDirection.UpLeft;
        if (x > 0.5f && y > 0.5f) return InputDirection.UpRight;
        if (x < -0.5f && y < -0.5f) return InputDirection.DownLeft;
        if (x > 0.5f && y < -0.5f) return InputDirection.DownRight;

        return InputDirection.Neutral;
    }

    private void SetCrouch(bool crouch)
    {
        if (box == null) return;

        if (crouch)
        {
            float heightDiff = defaultSize.y - crouchHeight;
            box.size = new Vector2(defaultSize.x, crouchHeight);
            box.offset = new Vector2(defaultOffset.x, defaultOffset.y - heightDiff / 2);
        }
        else
        {
            box.size = defaultSize;
            box.offset = defaultOffset;
        }
    }
}
