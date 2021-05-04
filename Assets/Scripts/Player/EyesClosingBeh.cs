using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State machine of the eyes closing animation. The on state exit function is sued to issue the restart or next level of the game
public class EyesClosingBeh : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.enabled = false;
        if (!FindObjectOfType<Exit>().foundExit) GameObject.FindObjectOfType<GameManager>().RestartGame();
        else GameObject.FindObjectOfType<GameManager>().NextLevel();
    }
}
