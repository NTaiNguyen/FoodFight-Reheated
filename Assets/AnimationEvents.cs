using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private AnimationController _animation;
    private ActionController _action;
    private MovementScript _movement;
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animation = GetComponentInParent<AnimationController>();
        _action = GetComponentInParent<ActionController>();
        _movement = GetComponentInParent<MovementScript>();
        _animator = GetComponent<Animator>();
    }

    public void OnAttackStart() {
        _action.StartAttack();
    }

    public void OnAttackComplete() {
        _action.Reset();
        
    }


}
