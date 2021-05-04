using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: split class into smaller features in their own classes
//responsible for the management and use of the flashlight and flares, their use and display

public class PlayerInvetory : MonoBehaviour
{
    public GameObject flarePref;
    public Camera playerCamera;
    public Light flashlight;
    public static int numOfFlares = 1;
    public static float flashlightEnergy = 30f;
    public GameObject bar1;
    public GameObject bar2;
    public GameObject bar3;
    public GameObject flare1;
    public GameObject flare2;
    public GameObject flare3;
    private static bool isTipShown4 = false;
    private static bool isTipShown3 = false;

    // Start is called before the first frame update
    void Start()
    {
        if (numOfFlares < 1) numOfFlares = 1;
        if (flashlightEnergy < 30f) flashlightEnergy = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        //shoot flare to light map
        if (Input.GetKeyDown(KeyCode.E) && numOfFlares > 0 && !FindObjectOfType<TextBubble>().isUIonFocus)
        {
            numOfFlares--;
            Instantiate(flarePref, GameObject.Find("Player/Head/Main Camera/Flashlight/Light").transform.position, playerCamera.transform.rotation);
            if (!isTipShown4)
            {
                FindObjectOfType<TextBubble>().showTutorial(4);
                isTipShown4 = true;
            }
        }

        //Flashlight energy mechanic
        if (flashlightEnergy > 0f)
        {
            flashlightEnergy -= Time.deltaTime;
            flashlight.enabled = true;
            checkHitEnemy();
        }
        else if (flashlightEnergy == 0)
        {
            flashlight.enabled = false;
        }
        else flashlightEnergy = 0f;

        //flickering of flashlight
        if (flashlightEnergy < 7f && flashlightEnergy > 0f)
        {
            flashlight.gameObject.GetComponent<FlickeringLight>().enabled = true;
        }
        else flashlight.gameObject.GetComponent<FlickeringLight>().enabled = false;

        //flashlight energy display
        switch (Mathf.CeilToInt(flashlightEnergy / 30f))
        {
            case 0:
                {
                    bar1.SetActive(false);
                    bar2.SetActive(false);
                    bar3.SetActive(false);
                    break;
                }
            case 1:
                {
                    bar1.SetActive(true);
                    bar2.SetActive(false);
                    bar3.SetActive(false);
                    break;
                }
            case 2:
                {
                    bar1.SetActive(true);
                    bar2.SetActive(true);
                    bar3.SetActive(false);
                    break;
                }
            default:
                {
                    bar1.SetActive(true);
                    bar2.SetActive(true);
                    bar3.SetActive(true);
                    break;
                }
        }

        //flares display
        switch (numOfFlares)
        {
            case 0:
                {
                    flare1.SetActive(false);
                    flare2.SetActive(false);
                    flare3.SetActive(false);
                    break;
                }
            case 1:
                {
                    flare1.SetActive(true);
                    flare2.SetActive(false);
                    flare3.SetActive(false);
                    break;
                }
            case 2:
                {
                    flare1.SetActive(true);
                    flare2.SetActive(true);
                    flare3.SetActive(false);
                    break;
                }
            default:
                {
                    flare1.SetActive(true);
                    flare2.SetActive(true);
                    flare3.SetActive(true);
                    break;
                }
        }
    }

    void checkHitEnemy()
    {
        Transform lightTransf = GetComponentInChildren<Light>().transform;

        RaycastHit hitInfo;
        if (Physics.Raycast(lightTransf.position, lightTransf.forward, out hitInfo, 50f))
        {
            if (hitInfo.transform.GetComponent<Unit>() != null && hitInfo.transform.GetComponent<Agent>().isCorrupted)
            {
                hitInfo.transform.GetComponent<Unit>().hitByFlashlight();
                if (!isTipShown3)
                {
                    FindObjectOfType<TextBubble>().showTutorial(3);
                    isTipShown3 = true;
                }
            }
        }
    }
}
