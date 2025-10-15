using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private MovementScript _movement;
    private ActionController _action;

    private MoveState newMove;
    private MoveState lastMove;
    private ActionState newAct;
    private ActionState lastAct;
    public AnimationSet animSet;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _movement = GetComponent<MovementScript>();
        _action = GetComponent<ActionController>();
        lastMove = _movement.sMove;

    }

    // Update is called once per frame
    void Update()
    {
        newMove = _movement.sMove;
        newAct = _action.sAct;

        if (newMove != lastMove) {
            lastMove = newMove;
            switch (newMove) {
                case MoveState.STAND: _animator.Play("Idle"); break;
                case MoveState.WALK: _animator.Play("Walk"); break;
                case MoveState.JUMP: _animator.Play("Jump"); break;
                case MoveState.FALL: _animator.Play("Fall"); break;
                case MoveState.CROUCH: _animator.Play("Crouch"); break;
            }
        }

        if (newAct != lastAct) {
            lastAct = newAct;

            switch (newAct) {
                case ActionState.SL: _animator.Play("SL"); break;
                case ActionState.SM: _animator.Play("SM"); break;
                case ActionState.SH: _animator.Play("SH"); break;
                case ActionState.CL: _animator.Play("CL"); break;
                case ActionState.CM: _animator.Play("CM"); break;
                case ActionState.CH: _animator.Play("SH"); break;
                case ActionState.JL: _animator.Play("JL"); break;
                case ActionState.JM: _animator.Play("JM"); break;
                case ActionState.JH: _animator.Play("JH"); break;
            }

            _action.Reset();
        }


    }



}
