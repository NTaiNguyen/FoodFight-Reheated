using UnityEngine;
public enum MoveState {
    STAND,
    CROUCH,
    JUMP,
    WALK,
    FALL
}
public enum ActionState {
    NONE,
    LIGHT,
    MEDIUM,
    HEAVY,
    SPECIAL,
    BLOCK
}
public class MovementScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private AnimationController animControl;
    private BoxCollider2D box;
    public BoxCollider2D crouchBox;
    // Stores the base size and offset of the character collider
    private Vector2 defaultSize, defaultOffset;
    private bool isGrounded;
    public float moveSpeed = 3.8f;
    public float jumpHeight = 9.5f;
    public MoveState sMove { get; private set; }
    public ActionState sAct { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animControl = GetComponent<AnimationController>();
        box = GetComponent<BoxCollider2D>();
        defaultSize = box.size;
        defaultOffset = box.offset;
        
    }

    // Update is called once per frame
    void Update(){
        HandleMovement();
        UpdateState();
    }

    
    private void HandleMovement() {
        float hMove = Input.GetAxisRaw("Horizontal");
        if (isGrounded) {
            //Slower Backwards movement 
            if (hMove > 0) {
                rb.linearVelocity = new Vector2(moveSpeed * hMove, rb.linearVelocity.y);
            }
            else
                rb.linearVelocity = new Vector2(moveSpeed * hMove * 0.8f, rb.linearVelocity.y);


            if (Input.GetKey(KeyCode.W)) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                isGrounded = false;
                sMove = MoveState.JUMP;
            }
        }
    }

    private void UpdateState() {
        if (!isGrounded) {
            sMove = rb.linearVelocity.y > 0 ? MoveState.JUMP : MoveState.FALL;
            RestoreColliderSize();
        }
        else {
            if (Input.GetKey(KeyCode.S)) {
                sMove = MoveState.CROUCH;
                // Changes collider to be shorter for the 
                box.size = new Vector2(defaultSize.x, defaultSize.y * 0.5f);
                box.offset = new Vector2(defaultOffset.x, defaultOffset.y - defaultOffset.y * 0.25f);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0) {
                sMove = MoveState.WALK;
                RestoreColliderSize();
            }
            else {
                sMove = MoveState.STAND;
                RestoreColliderSize();
            }
        }
        Debug.Log("sMove = " + sMove + " | sAct = " + sAct);

    }
    void RestoreColliderSize() {
        // Sets collider back to the standing size
        box.size = defaultSize;
        box.offset = defaultOffset;
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
}
