using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    // Who the hurtbox belongs to
    public ActionController owner;
    // For debug to tell who has hurtbox
    public int ownerID;   

    private void Awake()
    {
        if (owner == null)
        {
            Debug.LogWarning($"[HURTBOX] Hurtbox on {gameObject.name} has no owner assigned!");
        }
    }

    void Start()
    {
        owner = GetComponentInParent<ActionController>();
        Debug.Log($"Hurtbox created for: {owner.name}, ID: {ownerID}");
    }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     HurtboxController hurtbox = collision.GetComponent<HurtboxController>();

    //     if (hurtbox != null)
    //     {
    //         // Ignore self-hits
    //         if (hurtbox.owner == owner)
    //             return;

    //         Debug.Log($"[HIT] {owner.gameObject.name} hit {hurtbox.owner.gameObject.name}!");
    //     }
    // }
}
