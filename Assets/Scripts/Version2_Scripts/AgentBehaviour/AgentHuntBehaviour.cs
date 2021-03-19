using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentHuntBehaviour : StateMachineBehaviour
{
    Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        animator.gameObject.GetComponent<Unit>().StartSearchOfPath(target);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<Unit>().canSeePlayer)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (Vector3.Distance(target.position, animator.transform.position) > 0.5f) animator.gameObject.GetComponent<Unit>().StartSearchOfPath(target);
            else
            {
                Debug.Log("GAME OVER");
                FindObjectOfType<GameManager>().RestartGame();
            } // TODO: ANOTHER GAMEOVER STATE WITH DEATH ANIM AND THEN RESTART GAME MENu
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.FindObjectOfType<GameManager>().numberOfEnemies--;
    }
}
