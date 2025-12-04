using UnityEngine;

public class HitboxController : MonoBehaviour {
    public int damage;
    public ActionController owner;
    public AttackData data;

    void OnTriggerEnter2D(Collider2D col) {
        //stops player from hitting themself
        if (col.gameObject == owner.gameObject) return;

        Debug.Log($"{owner.name} hit {col.name} for {damage}!");
    }
}
