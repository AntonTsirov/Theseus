using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The class is responsible for firing the flare in the sky and altering the global lights value for a given time
public class Flare : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float speed;
    public float lifespan;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponentInChildren<Rigidbody>();

        rigidBody.AddForce(rigidBody.transform.forward * speed);
        Destroy(gameObject, lifespan);
        StartCoroutine("resetGlobalLight");

        GameObject.Find("GlobalLight").GetComponent<FlickeringLight>().enabled = true;
        GameObject.Find("GlobalLight").GetComponent<Light>().color = Color.red;
    }

    IEnumerator resetGlobalLight()
    {
        yield return new WaitForSeconds(lifespan - 0.1f);
        GameObject.Find("GlobalLight").GetComponent<FlickeringLight>().enabled = false;
        GameObject.Find("GlobalLight").GetComponent<Light>().color = Color.white;
        GameObject.Find("GlobalLight").GetComponent<Light>().intensity = 0.003f;
    }

    private void OnCollisionEnter(Collision other)
    {

        Destroy(gameObject);
        GameObject.Find("GlobalLight").GetComponent<FlickeringLight>().enabled = false;
        GameObject.Find("GlobalLight").GetComponent<Light>().color = Color.white;
        GameObject.Find("GlobalLight").GetComponent<Light>().intensity = 0.003f;

    }
}
