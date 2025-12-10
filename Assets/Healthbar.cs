using UnityEngine;

public class HealthBar : MonoBehaviour {
    [SerializeField] private CharacterData characterData; // Assign in inspector
    [SerializeField] private Transform bar; // The green rectangle sprite

    private float maxHealth;

    void Start() {
        if (characterData == null) {
            Debug.LogError("CharacterData not assigned!");
            return;
        }

        maxHealth = characterData.health;
        UpdateBar();
    }

    public void TakeDamage(int damage) {
        characterData.health -= damage;
        characterData.health = (int)Mathf.Clamp(characterData.health, 0, maxHealth);
        UpdateBar();
    }

    private void UpdateBar() {
        if (bar != null) {
            float ratio = (float)characterData.health / maxHealth;
            bar.localScale = new Vector3(ratio, 1f, 1f);
        }
    }
}
