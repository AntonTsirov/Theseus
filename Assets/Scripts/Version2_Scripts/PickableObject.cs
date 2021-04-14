using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [System.Serializable]
    public enum PickableObjectType { Flare, Battery, Note };
    public PickableObjectType item;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (item)
        {
            case PickableObjectType.Battery:
                {
                    gameObject.transform.Find("Battery").gameObject.SetActive(true);
                    break;
                }
            case PickableObjectType.Note:
                {
                    gameObject.transform.Find("Note").gameObject.SetActive(true);
                    break;
                }
            case PickableObjectType.Flare:
                {
                    gameObject.transform.Find("Flare").gameObject.SetActive(true);
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (item)
            {
                case PickableObjectType.Battery:
                    {
                        FindObjectOfType<PlayerInvetory>().flashlightEnergy += 60f;
                        FindObjectOfType<PlayerInvetory>().flashlight.intensity = 1f;
                        Destroy(gameObject);
                        break;
                    }
                case PickableObjectType.Note:
                    {
                        Debug.Log("pick note");
                        Destroy(gameObject);
                        break;
                    }
                case PickableObjectType.Flare:
                    {
                        FindObjectOfType<PlayerInvetory>().numOfFlares++;
                        Destroy(gameObject);
                        break;
                    }
            }
        }
    }


}
