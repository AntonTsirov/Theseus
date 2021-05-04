using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for reseting the triggers of the trap while retracting to prevent reactivation during that time
public class resetTrigger : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("trigger");
        animator.ResetTrigger("directTrigger");
    }
}
