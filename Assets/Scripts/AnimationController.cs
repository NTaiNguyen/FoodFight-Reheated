using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private MovementScript _movement;

    private MoveState newMove;
    private MoveState lastMove;
    private ActionState newAct;
    public AnimationSet animSet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _movement = GetComponent<MovementScript>();
        lastMove = _movement.sMove;

    }

    // Update is called once per frame
    void Update()
    {
        newMove = _movement.sMove;
        newAct = _movement.sAct;

        if (newMove != lastMove) {
            lastMove = newMove;
            switch (newMove) {
                case MoveState.STAND:
                    _animator.Play("Idle");
                    break;
                case MoveState.WALK:
                    _animator.Play("Walk");
                    break;
                case MoveState.JUMP:
                    _animator.Play("Jump");
                    break;
                case MoveState.FALL:
                    _animator.Play("Fall");
                    break;
                case MoveState.CROUCH:
                    _animator.Play("Crouch");
                    break;
            }
        }


    }



}
