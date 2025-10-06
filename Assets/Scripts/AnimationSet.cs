using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSet", menuName = "FightingGame/AnimationSet")]
public class AnimationSet : ScriptableObject {
    [Header("Movement Animations")]
    public AnimationClip standAnim;
    public AnimationClip walkAnim;
    public AnimationClip jumpAnim;
    public AnimationClip crouchAnim;
    public AnimationClip fallAnim;

    [Header("Action Animations")]
    public AnimationClip standLightAnim;
    public AnimationClip standMedAnim;
    public AnimationClip standHeavyAnim;
    public AnimationClip crouchLightAnim;
    public AnimationClip crouchMedAnim;
    public AnimationClip crouchHeavyAnim;
    public AnimationClip jumpLightAnim;
    public AnimationClip jumpMedAnim;
    public AnimationClip jumpHeavyAnim;
    public AnimationClip spec1Anim;
    public AnimationClip spec2Anim;
    public AnimationClip superAnim;
    public AnimationClip blockAnim;
}
