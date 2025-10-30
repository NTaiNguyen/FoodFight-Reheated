using UnityEngine;

public class ActionController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InputMapper mapper;
    private MovementScript _movement;
    public ActionState sAct { get; private set; }

    void Start()
    {
        mapper = GetComponent<InputMapper>();
        _movement = GetComponent<MovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }


    void UpdateState() {
        var button = mapper.GetPressedButton();
        if (button.HasValue) {
            switch ((_movement.sMove, button)) {
                case (MoveState.STAND, ButtonInput.LIGHT):
                    sAct = ActionState.SL;
                    break;
                case (MoveState.STAND, ButtonInput.MEDIUM):
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
                case (MoveState.JUMP, ButtonInput.LIGHT):
                case (MoveState.FALL, ButtonInput.LIGHT):
                    sAct = ActionState.JL;
                    break;
                case (MoveState.JUMP, ButtonInput.MEDIUM):
                case (MoveState.FALL, ButtonInput.MEDIUM):
                    sAct = ActionState.JM;
                    break;
                case (MoveState.JUMP, ButtonInput.HEAVY):
                case (MoveState.FALL, ButtonInput.HEAVY):
                    sAct = ActionState.JH;
                    break;
            }
        }
    }

    public void Reset() {
        sAct = ActionState.NONE;
    }


}
