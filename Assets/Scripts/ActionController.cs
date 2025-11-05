using UnityEngine;

public class ActionController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InputMapper mapper;
    private MovementScript _movement;
    public ActionState sAct { get; private set; }
    public bool isAttacking { get; private set; }
    private MoveState currentState;

    void Start()
    {
        mapper = GetComponent<InputMapper>();
        _movement = GetComponent<MovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        Debug.Log($"Current Action: {sAct}");

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

            //isAttacking = true;

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

    public void Reset() {
        Debug.Log($"[ActionController] Reset called on {gameObject.name}. sAct before: {sAct}");
        sAct = ActionState.NONE;
        isAttacking = false;
        Debug.Log($"[ActionController] sAct after: {sAct}, isAttacking: {isAttacking}");
    }

    public void StartAttack() {
        isAttacking = true;
    }
}
