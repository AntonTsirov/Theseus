using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvetory : MonoBehaviour
{
    public GameObject flarePref;
    public Camera playerCamera;
    public Light flashlight;
    public int numOfFlares;
    public float flashlightEnergy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //shoot flare to light map
        if (Input.GetKeyDown(KeyCode.Mouse0) && numOfFlares > 0)
        {
            numOfFlares--;
            Instantiate(flarePref, GameObject.Find("Player/Head/Main Camera/Flashlight/Light").transform.position, playerCamera.transform.rotation);
        }

        //Flashlight energy mechanic
        if (flashlightEnergy > 0f)
        {
            flashlightEnergy -= Time.deltaTime;
            flashlight.enabled = true;
        }
        if (flashlightEnergy < 0f) flashlightEnergy = 0f;

        if (flashlightEnergy == 0) flashlight.enabled = false;

        if (flashlightEnergy < 10f && flashlightEnergy > 0f)
        {
            flashlight.gameObject.GetComponent<FlickeringLight>().enabled = true;
        }
        else flashlight.gameObject.GetComponent<FlickeringLight>().enabled = false;


    }
}
