using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple way to store an object as place of interest for the AI to explore
public class POI : MonoBehaviour
{
    void Awake()
    {
        GameObject.FindObjectOfType<GameManager>().poi.Add(gameObject);
    }
}
