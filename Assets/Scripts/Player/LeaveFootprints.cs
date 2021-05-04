using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The class responsible for leaving left and right footsprint when the player is running
public class LeaveFootprints : MonoBehaviour
{
    public GameObject footprint;
    public float footPrintsInterval;
    private float elapsedTimeForFootPrints = 0f;
    private bool swapFootPrints = false;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerController>().isRunning)
        {
            elapsedTimeForFootPrints += Time.deltaTime;
            if (elapsedTimeForFootPrints >= footPrintsInterval)
            {
                if (swapFootPrints)
                {
                    GameObject ftprint = Instantiate(footprint, transform.position, transform.rotation, GameObject.Find("Footprints").transform);
                    ftprint.GetComponentInChildren<SpriteRenderer>().flipX = true;
                    swapFootPrints = false;
                }
                else
                {
                    GameObject ftprint = Instantiate(footprint, transform.position, transform.rotation, GameObject.Find("Footprints").transform);
                    ftprint.GetComponentInChildren<SpriteRenderer>().flipX = false;
                    swapFootPrints = true;
                }
                elapsedTimeForFootPrints = 0;
            }
        }
        else
        {
            elapsedTimeForFootPrints = 0;
        }
    }
}
