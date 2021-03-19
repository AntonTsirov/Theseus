using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentExploringBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<Unit>().target == null)
        {
            List<GameObject> poi = GameObject.FindObjectOfType<GameManager>().poi;
            animator.gameObject.GetComponent<Unit>().StartSearchOfPath(poi[Random.Range(0, poi.Count - 1)].transform);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((Vector3.Distance(animator.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 3f && FindObjectOfType<PlayerController>().isRunning) ||
            animator.gameObject.GetComponent<Unit>().canSeePlayer)
        {
            animator.gameObject.GetComponent<Unit>().StopPathing();
            GameObject.FindObjectOfType<GameManager>().numberOfEnemies++;
            animator.SetBool("isHunting", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
