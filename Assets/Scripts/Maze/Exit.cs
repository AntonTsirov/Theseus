using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for the exit object
public class Exit : MonoBehaviour
{
    public bool foundExit = false;
    private static bool isTipShown6 = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foundExit = true;
            GameObject.FindGameObjectWithTag("Player").transform.Find("EyesCanvas").GetComponent<Animator>().speed = 0.5f;
            GameObject.FindGameObjectWithTag("Player").transform.Find("EyesCanvas").GetComponent<Animator>().SetBool("goSleep", true);
            if (!isTipShown6)
            {
                FindObjectOfType<TextBubble>().showTutorial(6);
                isTipShown6 = true;
            }
        }
    }
}
