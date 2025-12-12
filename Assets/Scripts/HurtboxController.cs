using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    public ActionController owner;

    // This method may have been causing hitboxes to not work
    void OnTriggerEnter2D(Collider2D other)
    {
        // HitboxController hb = other.GetComponent<HitboxController>();
        // if (hb != null && hb.owner != owner)
        // {
        //     owner.TakeDamage(hb.data.damage);
        //     Debug.Log($"{owner.name} hit by {hb.owner.name} for {hb.data.damage} damage.");
        // }
    }
}