using UnityEngine;

public class GizmoArtist : MonoBehaviour
{
    public CharacterData characterData;  // Drag your CharacterData here

    void OnDrawGizmos() {
        if (characterData == null || characterData.attacks == null) return;

       
        foreach (var attack in characterData.attacks) {
            foreach (var hitbox in attack.hitboxes) {
                Gizmos.color = hitbox.gizmoColor;
                Vector3 center = transform.position + (Vector3)hitbox.offset;
                Gizmos.DrawWireCube(center, hitbox.size);
            }
        }
    }
}
