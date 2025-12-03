using UnityEngine;
using System.Collections.Generic;
public class AnimationController : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController baseController;
    [SerializeField] private CharacterData data;
    private Animator _animator;
    private MovementScript _movement;
    private ActionController _action;
    public AnimationSet animSet;

    private AnimatorOverrideController _override;

    private MoveState newMove;
    private MoveState lastMove;
    private ActionState newAct;
    private ActionState lastAct;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _override = new AnimatorOverrideController(baseController);
        _animator.runtimeAnimatorController = _override;  // assign before Start

    }
    void Start()
    {
        _movement = GetComponent<MovementScript>();
        _action = GetComponent<ActionController>();
        lastMove = _movement.sMove;

        ApplyAnimationSet(animSet);


    }

    // Update is called once per frame
    void Update()
    {
        newMove = _movement.sMove;
        newAct = _action.sAct;

        if (newAct != ActionState.NONE) {
            if (newAct != lastAct) {
                lastAct = newAct;
                PlayActionAnimation(newAct);
            }
            return; // Don't update movement during attack
        }

        // --- Resume movement after attack ends ---
        if (!_action.isAttacking) {
            // Only play if movement state changed OR coming out of an attack
            if (newMove != lastMove || lastAct != ActionState.NONE) {
                lastMove = newMove;
                lastAct = ActionState.NONE;
                PlayMoveAnimation(newMove);
            }
        }
    }



    private void ApplyAnimationSet(AnimationSet set) {
        
        if (set == null) return;

        void SafeAssign(string stateName, AnimationClip clip) {
            if (clip != null) {
                // Directly assign the clip to the state in the override controller
                _override[stateName] = clip;
                //Debug.Log($"Overridden '{stateName}' with clip '{clip.name}'");
            }
        }

        // Movement
        


        SafeAssign("Idle", set.standAnim);
        SafeAssign("Walk", set.walkAnim);
        SafeAssign("Jump", set.jumpAnim);
        SafeAssign("Crouch", set.crouchAnim);
        SafeAssign("Fall", set.fallAnim);
        SafeAssign("HitStun", set.takingDamageAnim);

        // Actions
        SafeAssign("SL", set.standLightAnim);
        SafeAssign("SM", set.standMedAnim);
        SafeAssign("SH", set.standHeavyAnim);
        SafeAssign("CL", set.crouchLightAnim);
        SafeAssign("CM", set.crouchMedAnim);
        SafeAssign("CH", set.crouchHeavyAnim);
        SafeAssign("JL", set.jumpLightAnim);
        SafeAssign("JM", set.jumpMedAnim);
        SafeAssign("JH", set.jumpHeavyAnim);

        // Specials
        SafeAssign("Spec1", set.spec1Anim);
        SafeAssign("Spec2", set.spec2Anim);
        SafeAssign("Super", set.superAnim);

        // Defense
        SafeAssign("StandBlock", set.standingBlockAnim);
        SafeAssign("CrouchBlock", set.crouchingBlockAnim);

    }

    private void PlayActionAnimation(ActionState act) {
        switch (act) {
            case ActionState.SL: _animator.Play("SL", 0, 0f); break;
            case ActionState.SM: _animator.Play("SM", 0, 0f); break;
            case ActionState.SH: _animator.Play("SH", 0, 0f); break;
            case ActionState.CL: _animator.Play("CL", 0, 0f); break;
            case ActionState.CM: _animator.Play("CM", 0, 0f); break;
            case ActionState.CH: _animator.Play("CH", 0, 0f); break;
            case ActionState.JL: _animator.Play("JL", 0, 0f); break;
            case ActionState.JM: _animator.Play("JM", 0, 0f); break;
            case ActionState.JH: _animator.Play("JH", 0, 0f); break;
        }
    }

    private void PlayMoveAnimation(MoveState move) {
        switch (move) {
            case MoveState.STAND: _animator.Play("Idle", 0, 0f); break;
            case MoveState.WALK: _animator.Play("Walk", 0, 0f); break;
            case MoveState.JUMP: _animator.Play("Jump", 0, 0f); break;
            case MoveState.FALL: _animator.Play("Fall", 0, 0f); break;
            case MoveState.CROUCH: _animator.Play("Crouch", 0, 0f); break;
        }
    }



}
