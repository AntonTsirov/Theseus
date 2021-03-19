using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : MonoBehaviour
{
    void Awake()
    {
        GameObject.FindObjectOfType<GameManager>().poi.Add(gameObject);
    }
}
