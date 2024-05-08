using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class AnimationClass:MonoBehaviourPunCallbacks
{
    public void AnimationMove(Animator animator)
    {
        animator.SetBool("Moving", true);
        animator.SetFloat("Animation Move", 1f);
        animator.SetFloat("Velocity", 1f);
        animator.SetInteger("Trigger Number", 5);
    }
    public void AnimationStopMove(Animator animator)
    {
        animator.SetBool("Moving", false);
        animator.SetFloat("Animation Move", 0f);
        animator.SetFloat("Velocity", 0f);
        animator.SetInteger("Trigger Number", 0);
    }
    public void AnimationMoveFind(Animator animator)
    {
        animator.SetBool("Moving", true);
        animator.SetFloat("Animation Move", 1f);
        animator.SetFloat("Velocity", 1f);
        animator.SetInteger("Trigger Number", 1);
    }
}
