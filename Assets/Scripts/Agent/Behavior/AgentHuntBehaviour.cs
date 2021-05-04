using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the hunt behavior of the agent
public class AgentHuntBehaviour : StateMachineBehaviour
{
    Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<AudioSource>().Play();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator.gameObject.GetComponent<Unit>().StartSearchOfPath(target);
        Debug.Log("hit state enter");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<Unit>().canSeePlayer)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (Vector3.Distance(target.position, animator.transform.position) > 0.5f)
            {
                Debug.Log("greater distance");
                if (FindObjectOfType<GridForPath>().NodeFromWorldPoint(animator.gameObject.GetComponent<Unit>().endDest) != FindObjectOfType<GridForPath>().NodeFromWorldPoint(target.position))
                {
                    Debug.Log("start new path");
                    animator.gameObject.GetComponent<Unit>().StopPathing();
                    animator.gameObject.GetComponent<Unit>().StartSearchOfPath(target);
                }
            }
        }
        //kill the player if close
        if (Vector3.Distance(target.position, animator.transform.position) <= 0.4f)
        {
            GameObject.FindGameObjectWithTag("Player").transform.Find("EyesCanvas").GetComponent<Animator>().speed = 0.5f;
            GameObject.FindGameObjectWithTag("Player").transform.Find("EyesCanvas").GetComponent<Animator>().SetTrigger("goSleep");
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("isDead");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<AudioSource>().Stop();
    }

}
