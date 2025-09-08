using UnityEngine;
public enum PlayerInput {
    
}
public class MovementScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded;
    public float moveSpeed = 3.8f;
    public float jumpHeight = 9.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if (isGrounded) {
            float hMove = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveSpeed * hMove, rb.linearVelocity.y);

            if (Input.GetKeyDown(KeyCode.W)) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                isGrounded = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground")) {
            isGrounded = false;
        }
    }
}
