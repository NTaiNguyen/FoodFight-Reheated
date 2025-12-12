using UnityEngine;

public class HitboxController : MonoBehaviour {
    public ActionController owner;
    public HitboxData data;
    private BoxCollider2D box;

    void Awake() {
        box = GetComponent<BoxCollider2D>();
        
        // This may be what was breaking the code
        if (data != null && box != null) {
            // // Set the size
            // box.size = data.size;

            // // Set the offset relative to the hitbox's parent or owner
            // box.offset = data.offset;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        //stops player from hitting themself
        if (other.gameObject == owner.gameObject) return;
        
        Debug.Log($"{owner.name} hit {other.name} for {data.damage}!");
        HurtboxController hb = other.GetComponent<HurtboxController>();
        if (hb != null)
        {
            Debug.Log($"[HIT] Hitbox from {owner.gameObject.name} hit {hb.owner.gameObject.name}");

            // Apply damage later
            hb.owner.TakeDamage(data.damage);
            
        }
    }

    // Method to draw hitboxes
    // Only in scene view
    private void OnDrawGizmos()
    {
        if (data == null) return;

        Gizmos.color = new Color(1f, 0, 0, 0.35f);

        Vector3 worldPos = transform.TransformPoint(box.offset);

        Gizmos.DrawCube(
            worldPos,
            new Vector3(box.size.x, box.size.y, 0.01f)
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(
            worldPos,
            new Vector3(box.size.x, box.size.y, 0.01f)
        );
    }

}
