using UnityEngine;

public class HitboxController : MonoBehaviour {
    public ActionController owner;
    public HitboxData data;
    private BoxCollider2D box;

    void Awake() {
        box = GetComponent<BoxCollider2D>();

        if (data != null && box != null) {
            // Set the size
            box.size = data.size;

            // Set the offset relative to the hitbox's parent or owner
            box.offset = data.offset;
        }
    }
    void OnTriggerEnter2D(Collider2D col) {
        //stops player from hitting themself
        if (col.gameObject == owner.gameObject) return;
        
        Debug.Log($"{owner.name} hit {col.name} for {data.damage}!");
    }
}
