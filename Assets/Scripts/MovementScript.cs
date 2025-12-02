using UnityEngine;
public enum MoveState {
    STAND,
    CROUCH,
    JUMP,
    WALK,
    FALL,
    ARIAL
}
public enum ActionState {
    NONE,
    SL, SM, SH,
    CL, CM, CH,
    JL, JM, JH,
    SP1, SP2,
    BLOCK,
    CBLOCK
}
public enum InputDirection {
    Up = 8,
    Right = 6,
    Down = 2,
    Left = 4,
    DownLeft = 1,
    DownRight = 3,
    UpLeft = 7,
    UpRight = 9,
    Neutral = 5
}
public class MovementScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D box;
    // Stores the base size and offset of the character collider

    // Used for collider resizing
    private Vector2 defaultSize, defaultOffset;
    private float crouchHeight;
    private bool isGrounded;
    public float moveSpeed = 3.8f;
    public float jumpHeight = 9.5f;
    public MoveState sMove { get; private set; }
    private InputDirection direction = InputDirection.Neutral;
    private Vector2 input;
    private ActionController _action;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        _action = GetComponent<ActionController>();
        defaultSize = box.size;
        defaultOffset = box.offset;
        crouchHeight = defaultSize.y * .75f;
    }

    // Update is called once per frame
    void Update(){
        HandleMovement();
        UpdateState();

        Debug.Log($"MovementState: {sMove}");
    }


    private void HandleMovement() {
        if (_action.isAttacking || !isGrounded) return;
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction = GetDirection(input);
        
        

        switch (direction) {
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

    private void UpdateState() {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float deadzone = 0.1f;
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        InputDirection direction = GetDirection(directionalInput);
        if (!isGrounded) {
            sMove = rb.linearVelocity.y > 0 ? MoveState.JUMP : MoveState.FALL;
        }
        else {
            if (verticalInput < -deadzone) {
                sMove = MoveState.CROUCH;
                crouchLocked = true; 
            }
            else if (crouchLocked && verticalInput >= -deadzone) {
                crouchLocked = false;
                sMove = Mathf.Abs(rb.linearVelocity.x) > 0 && direction != InputDirection.Neutral ? MoveState.WALK : MoveState.STAND;
            }
            else if (!crouchLocked) {
                sMove = Mathf.Abs(rb.linearVelocity.x) > 0 && direction != InputDirection.Neutral ? MoveState.WALK : MoveState.STAND;
            }
        }

        SetCrouch(sMove == MoveState.CROUCH);
    }



    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
            sMove = MoveState.STAND;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground")) {
            isGrounded = false;
        }
    }

    private InputDirection GetDirection(Vector2 input) {
        float x = input.x;
        float y = input.y;

        // turns input values into enums
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


    private void SetCrouch(bool crouch) {
        if (crouch) {
            // Shrink the collider from the top
            float heightDiff = defaultSize.y - crouchHeight;
            box.size = new Vector2(defaultSize.x, crouchHeight);
            box.offset = new Vector2(defaultOffset.x, defaultOffset.y - heightDiff / 2);
        }
        else {
            // Reset to standing
            box.size = defaultSize;
            box.offset = defaultOffset;
        }
    }


}

// // -----------------------------------------------------------------------------------------------------------------------------------------------------
// // ADDED ON 12/1/25 TO SEE IF IT WORKS, COMMENTED OUT OLD PART   ---------------------------------------------------------------------------------------
// // -----------------------------------------------------------------------------------------------------------------------------------------------------
// using UnityEngine;

// public enum MoveState {
//     STAND,
//     CROUCH,
//     JUMP,
//     WALK,
//     FALL,
//     ARIAL
// }

// public enum InputDirection {
//     Up = 8,
//     Right = 6,
//     Down = 2,
//     Left = 4,
//     DownLeft = 1,
//     DownRight = 3,
//     UpLeft = 7,
//     UpRight = 9,
//     Neutral = 5
// }

// public class MovementScript : MonoBehaviour
// {
//     private Rigidbody2D rb;
//     private BoxCollider2D box;
//     private InputMapper mapper;
//     private ActionController _action;

//     private bool isGrounded;

//     public float moveSpeed = 3.8f;
//     public float jumpHeight = 9.5f;

//     public MoveState sMove { get; private set; }

//     private InputDirection direction = InputDirection.Neutral;

//     private Vector2 defaultSize, defaultOffset;
//     private float crouchHeight;
//     private bool crouchLocked = false;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         box = GetComponent<BoxCollider2D>();
//         mapper = GetComponent<InputMapper>();  
//         _action = GetComponent<ActionController>();

//         defaultSize = box.size;
//         defaultOffset = box.offset;
//         crouchHeight = defaultSize.y * .75f;
//     }

//     void Update()
//     {
//         HandleMovement();
//         UpdateState();
//     }

//     private void HandleMovement()
//     {
//         if (_action.isAttacking) return;

//         Vector2 moveInput = mapper.GetMovement();       
//         direction = GetDirection(moveInput);           

//         // Prevent full air control
//         if (!isGrounded)
//             return;

//         switch (direction)
//         {
//             case InputDirection.Right:
//                 rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
//                 break;

//             case InputDirection.Left:
//                 rb.linearVelocity = new Vector2(-moveSpeed * 0.8f, rb.linearVelocity.y);
//                 break;

//             case InputDirection.Down:
//             case InputDirection.DownLeft:
//             case InputDirection.DownRight:
//                 rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
//                 break;

//             case InputDirection.Up:
//             case InputDirection.UpLeft:
//             case InputDirection.UpRight:
//                 rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
//                 isGrounded = false;
//                 break;

//             case InputDirection.Neutral:
//                 rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
//                 break;
//         }
//     }

//     private void UpdateState()
//     {
//         if (!isGrounded)
//         {
//             sMove = rb.linearVelocity.y > 0 ? MoveState.JUMP : MoveState.FALL;
//             return;
//         }

//         Vector2 moveInput = mapper.GetMovement();
//         InputDirection dir = GetDirection(moveInput);

//         if (dir == InputDirection.Down || dir == InputDirection.DownLeft || dir == InputDirection.DownRight)
//         {
//             sMove = MoveState.CROUCH;
//             crouchLocked = true;
//         }
//         else if (crouchLocked && dir != InputDirection.Down)
//         {
//             crouchLocked = false;
//             sMove = Mathf.Abs(rb.linearVelocity.x) > 0.1f ? MoveState.WALK : MoveState.STAND;
//         }
//         else if (!crouchLocked)
//         {
//             sMove = Mathf.Abs(rb.linearVelocity.x) > 0.1f ? MoveState.WALK : MoveState.STAND;
//         }

//         SetCrouch(sMove == MoveState.CROUCH);
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Ground"))
//         {
//             isGrounded = true;
//             sMove = MoveState.STAND;
//         }
//     }

//     private void OnCollisionExit2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Ground"))
//         {
//             isGrounded = false;
//         }
//     }

//     private InputDirection GetDirection(Vector2 input)
//     {
//         float x = input.x;
//         float y = input.y;

//         if (y > 0.5f && Mathf.Abs(x) < 0.5f) return InputDirection.Up;
//         if (y < -0.5f && Mathf.Abs(x) < 0.5f) return InputDirection.Down;
//         if (x < -0.5f && Mathf.Abs(y) < 0.5f) return InputDirection.Left;
//         if (x > 0.5f && Mathf.Abs(y) < 0.5f) return InputDirection.Right;

//         if (x < -0.5f && y > 0.5f) return InputDirection.UpLeft;
//         if (x > 0.5f && y > 0.5f) return InputDirection.UpRight;
//         if (x < -0.5f && y < -0.5f) return InputDirection.DownLeft;
//         if (x > 0.5f && y < -0.5f) return InputDirection.DownRight;

//         return InputDirection.Neutral;
//     }

//     private void SetCrouch(bool crouch)
//     {
//         if (crouch)
//         {
//             float heightDiff = defaultSize.y - crouchHeight;
//             box.size = new Vector2(defaultSize.x, crouchHeight);
//             box.offset = new Vector2(defaultOffset.x, defaultOffset.y - heightDiff / 2);
//         }
//         else
//         {
//             box.size = defaultSize;
//             box.offset = defaultOffset;
//         }
//     }
// }
