using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for defining the type of trap and time to activation
public class SpikeTrap : MonoBehaviour
{
    public bool isPeriodic;
    public float spikeTriggerTimer = 1f;
    private bool canTriggerAgain = true;

    // Start is called before the first frame update
    void Start()
    {
        isPeriodic = (Random.Range(0, 2) == 0) ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPeriodic && canTriggerAgain) StartCoroutine("activateTrigger");
    }

    IEnumerator activateTrigger()
    {
        canTriggerAgain = false;
        yield return new WaitForSeconds(Random.Range(spikeTriggerTimer, spikeTriggerTimer + 2f));
        gameObject.transform.parent.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("directTrigger");
        canTriggerAgain = true;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isPeriodic)
        {
            gameObject.transform.parent.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("trigger");
        }
    }
}
