using UnityEngine;
using System.Collections.Generic;

public class ActionController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InputMapper mapper;
    private MovementScript _movement;
    public ActionState sAct { get; private set; }
    public bool isAttacking;
    private MoveState currentState;
    public int playerID;
    public CharacterData _charData;
    private AttackData currentAttack;
    private int currentFrame;
    private List<BoxCollider2D> activeHitboxes = new List<BoxCollider2D>();


    [SerializeField] private BoxCollider2D hitboxPrefab;
    [SerializeField] private BoxCollider2D hurtboxPrefab;

    void Start()
    {
        mapper = GetComponent<InputMapper>();
        // Added by Cyler on 12/1/25
        mapper.playerID = playerID;
        _movement = GetComponent<MovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking) {
            currentFrame++;

            HandleHitboxSpawning();
        }
        UpdateState();
        //Debug.Log($"Current Action: {sAct}");
        //Debug.Log("isAttacking = " + isAttacking);

    }


    void UpdateState() {
        // if already attacking exit
        if (isAttacking) return;
        var button = mapper.GetPressedButton();

        if (button.HasValue) {
            //Combines the jump and fall state into arial so animations dont cancel when switching
            if (_movement.sMove == MoveState.JUMP || _movement.sMove == MoveState.FALL) {
                currentState = MoveState.ARIAL;
            }
            else {currentState = _movement.sMove;}


            switch ((currentState, button)) {
                case (MoveState.STAND, ButtonInput.LIGHT):
                case (MoveState.WALK, ButtonInput.LIGHT):
                    sAct = ActionState.SL;
                    break;
                case (MoveState.STAND, ButtonInput.MEDIUM):
                case (MoveState.WALK, ButtonInput.MEDIUM):
                    sAct = ActionState.SM;
                    break;
                case (MoveState.STAND, ButtonInput.HEAVY):
                    sAct = ActionState.SH;
                    break;
                case (MoveState.CROUCH, ButtonInput.LIGHT):
                    sAct = ActionState.CL;
                    break;
                case (MoveState.CROUCH, ButtonInput.MEDIUM):
                    sAct = ActionState.CM;
                    break;
                case (MoveState.CROUCH, ButtonInput.HEAVY):
                    sAct = ActionState.CH;
                    break;
                case (MoveState.ARIAL, ButtonInput.LIGHT):
                    sAct = ActionState.JL;
                    break;
                case (MoveState.ARIAL, ButtonInput.MEDIUM):
                    sAct = ActionState.JM;
                    break;
                case (MoveState.ARIAL, ButtonInput.HEAVY):
                    sAct = ActionState.JH;
                    break;
                }

        }
    }

    void SpawnHitbox(HitboxData hbData) {
        var prefab = currentAttack.hitboxPrefab;

        BoxCollider2D hitbox = Instantiate(prefab, transform);

        HitboxController controller = hitbox.GetComponent<HitboxController>();
        controller.owner = this;
        controller.data = hbData;

        activeHitboxes.Add(hitbox);
    }


    void HandleHitboxSpawning() {
        if (currentAttack == null) return;

        foreach (var hb in currentAttack.hitboxes) {
            if (currentFrame == hb.activeFrameStart) {
                SpawnHitbox(hb);
            }
            if (currentFrame == hb.activeFrameEnd) {
                EndHitbox(hb);
            }
        }
    }

    void EndHitbox(HitboxData hbData) {
        for (int i = activeHitboxes.Count - 1; i >= 0; i--) {
            HitboxController c = activeHitboxes[i].GetComponent<HitboxController>();

            if (c.data == hbData) {
                Destroy(activeHitboxes[i].gameObject);
                activeHitboxes.RemoveAt(i);
            }
        }
    }
 
    public void Reset() {
        sAct = ActionState.NONE;
        isAttacking = false;
    }
    
    public void StartAttack() {
        isAttacking = true;

        // find the correct attack data
        // currentAttack = _charData.attacks[(int)sAct];

        // currentFrame = 0;
    }
}
