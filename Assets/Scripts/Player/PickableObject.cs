using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for definying the type of pickable object and what would happen when it is picked up
public class PickableObject : MonoBehaviour
{
    [System.Serializable]
    public enum PickableObjectType { Flare, Battery };
    public PickableObjectType item;
    private static bool isTipShown2 = false;

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
                        PlayerInvetory.flashlightEnergy = Mathf.Clamp(PlayerInvetory.flashlightEnergy + 30f, 0f, 90f);
                        FindObjectOfType<PlayerInvetory>().flashlight.intensity = 1f;
                        other.GetComponents<AudioSource>()[other.GetComponents<AudioSource>().Length - 1].Play();
                        Destroy(gameObject);
                        break;
                    }
                case PickableObjectType.Flare:
                    {
                        if (PlayerInvetory.numOfFlares < 3)
                        {
                            PlayerInvetory.numOfFlares++;
                            other.GetComponents<AudioSource>()[other.GetComponents<AudioSource>().Length - 1].Play();
                            Destroy(gameObject);
                        }
                        break;
                    }
            }
            if (!isTipShown2)
            {
                FindObjectOfType<TextBubble>().showTutorial(2);
                isTipShown2 = true;
            }
        }
    }


}
