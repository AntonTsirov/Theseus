using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the rest behavior of the agent
public class AgentRestBehaviour : StateMachineBehaviour
{
    float timer;
    public float minTime;
    public float maxTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
        if (!animator.gameObject.GetComponent<Agent>().isCorrupted)
        {
            int currentCorr = 0;
            foreach (Agent agentObj in GameObject.FindObjectsOfType<Agent>())
            {
                if (agentObj.isCorrupted) currentCorr++;
            }
            //if there is room for corrupted; become corrupted with a x% chance
            if (currentCorr < GameObject.FindObjectOfType<GameManager>().numberOfEnemies)
            {
                animator.gameObject.GetComponent<Agent>().isCorrupted = true;
            }
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0)
        {
            animator.SetBool("isExploring", true);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}