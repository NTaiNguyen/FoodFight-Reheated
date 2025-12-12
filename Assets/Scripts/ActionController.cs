using UnityEngine;
using System.Collections.Generic;

public class ActionController : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerID;
    public CharacterData _charData;

    [Header("References")]
    public Transform spriteRoot;             
    public Transform opponent;                
    [SerializeField] private BoxCollider2D hitboxPrefab;
    [SerializeField] private BoxCollider2D hurtboxPrefab;

    [Header("Health")]
    public int currentHealth { get; private set; }

    // Attack state
    public ActionState sAct { get; private set; }
    private AttackData currentAttack;
    private int currentFrame;
    public bool isAttacking;
    private List<BoxCollider2D> activeHitboxes = new List<BoxCollider2D>();

    // Movement/Input
    private InputMapper mapper;
    private MovementScript _movement;

    void Start()
    {
        // Setting players tags for victory screen managment
        if (playerID == 1)
        {
            gameObject.tag = "Player1";
        } else if (playerID == 2)
        {
            gameObject.tag = "Player2"; 
        }

        // Debug to check if the tags got assigned here
        Debug.Log($"{name} assigned tag: " + gameObject.tag);
           

        mapper = GetComponent<InputMapper>();
        mapper.playerID = playerID;
        _movement = GetComponent<MovementScript>();

        // Find sprite root if not assigned
        if (spriteRoot == null)
            spriteRoot = GetComponentInChildren<SpriteRenderer>()?.transform;
        if (spriteRoot == null)
            spriteRoot = transform.Find("Sprite");

        // Assign health
        currentHealth = _charData.health;

        // Assign hurtbox owner
        HurtboxController hb = GetComponentInChildren<HurtboxController>();
        if (hb != null)
        {
            hb.owner = this;
            Debug.Log($"[ACTION] {gameObject.name} connected pushbox hurtbox.");
        }
    }

    void Update()
    {
        // Flip sprite towards opponent
        UpdateFacing();

        if (isAttacking)
        {
            currentFrame++;
            HandleHitboxSpawning();
        }

        UpdateState();
    }

    void UpdateFacing()
    {
        if (opponent == null || spriteRoot == null) return;

        bool facingRight = opponent.position.x > transform.position.x;
        spriteRoot.localScale = new Vector3(facingRight ? 1f : -1f, spriteRoot.localScale.y, spriteRoot.localScale.z);
    }

    void UpdateState()
    {
        if (isAttacking) return;
        var button = mapper.GetPressedButton();
        if (!button.HasValue) return;

        // Determine movement state
        MoveState currentState = (_movement.sMove == MoveState.JUMP || _movement.sMove == MoveState.FALL)
            ? MoveState.ARIAL : _movement.sMove;

        // Determine action
        switch ((currentState, button))
        {
            case (MoveState.STAND, ButtonInput.LIGHT):
            case (MoveState.WALK, ButtonInput.LIGHT):
                sAct = ActionState.SL; break;
            case (MoveState.STAND, ButtonInput.MEDIUM):
            case (MoveState.WALK, ButtonInput.MEDIUM):
                sAct = ActionState.SM; break;
            case (MoveState.STAND, ButtonInput.HEAVY):
            case (MoveState.WALK, ButtonInput.HEAVY):
                sAct = ActionState.SH; break;
            case (MoveState.CROUCH, ButtonInput.LIGHT):
                sAct = ActionState.CL; break;
            case (MoveState.CROUCH, ButtonInput.MEDIUM):
                sAct = ActionState.CM; break;
            case (MoveState.CROUCH, ButtonInput.HEAVY):
                sAct = ActionState.CH; break;
            case (MoveState.ARIAL, ButtonInput.LIGHT):
                sAct = ActionState.JL; break;
            case (MoveState.ARIAL, ButtonInput.MEDIUM):
                sAct = ActionState.JM; break;
            case (MoveState.ARIAL, ButtonInput.HEAVY):
                sAct = ActionState.JH; break;
        }

        // Assign current attack safely
        int attackIndex = (int)sAct;
        if (_charData.attacks != null && attackIndex >= 0 && attackIndex < _charData.attacks.Length)
            currentAttack = _charData.attacks[attackIndex - 1];
        else
            currentAttack = null;

        if (sAct != ActionState.NONE)
            StartAttack();
    }

    public void StartAttack()
    {
        isAttacking = true;
        currentFrame = 0;
        // if (currentAttack != null)
        //     Debug.Log($"[ATTACK START] {gameObject.name} started {currentAttack.attackName}");
    }

    void HandleHitboxSpawning()
    {
        if (currentAttack == null) return;

        foreach (var hb in currentAttack.hitboxes)
        {
            if (currentFrame == hb.activeFrameStart)
                SpawnHitbox(hb);
            if (currentFrame == hb.activeFrameEnd)
                EndHitbox(hb);
        }
    }

    void SpawnHitbox(HitboxData hbData)
    {
        BoxCollider2D hitbox = Instantiate(hitboxPrefab, spriteRoot);

        Vector3 offset = hbData.offset;
        Vector2 size = hbData.size;

        // Flip X offset if facing opponent on left
        float facing = spriteRoot.localScale.x > 0 ? 1f : -1f;
        offset.x *= facing;

        hitbox.transform.localScale = new Vector3(facing, 1f, 1f);

        hitbox.offset = offset;
        hitbox.size = size;

        HitboxController controller = hitbox.GetComponent<HitboxController>();
        controller.owner = this;
        controller.data = hbData;

        activeHitboxes.Add(hitbox);

        // Un comment this to see if the hitboxes are spawning for each player
        // With it uncommented its a lot in the debug menu
        Debug.Log($"[HITBOX] {gameObject.name} spawned hitbox at frame {currentFrame} for {currentAttack.attackName}");
    }

    void EndHitbox(HitboxData hbData)
    {
        for (int i = activeHitboxes.Count - 1; i >= 0; i--)
        {
            HitboxController c = activeHitboxes[i].GetComponent<HitboxController>();
            if (c.data == hbData)
            {
                Destroy(activeHitboxes[i].gameObject);
                activeHitboxes.RemoveAt(i);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{name} took {damage} damage; currentHealth: {currentHealth}");
    }

    public void Reset()
    {
        sAct = ActionState.NONE;
        isAttacking = false;
    }

    // Method to show what direction players are facing
    private void OnDrawGizmos()
    {
        if (spriteRoot == null) return;

        Gizmos.color = Color.yellow;
        float facing = spriteRoot.localScale.x;

        Gizmos.DrawLine(
            transform.position,
            transform.position + new Vector3(facing * 1f, 0, 0)
        );
    }

}