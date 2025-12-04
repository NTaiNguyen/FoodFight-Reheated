using UnityEngine;

public class GizmoArtist : MonoBehaviour
{
    public CharacterData characterData;  // Drag your CharacterData here

    void OnDrawGizmos() {
        if (characterData == null || characterData.attacks == null) return;

       
        foreach (var attack in characterData.attacks) {
            Gizmos.color = attack.gizmoColor;
            Vector3 center = transform.position + (Vector3)attack.hitboxOffset;
            Gizmos.DrawWireCube(center, attack.hitboxSize);
        }
    }
}
