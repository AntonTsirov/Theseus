using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        float scaleX = Mathf.PingPong(Time.time, 5);
        float scaleY = Mathf.PingPong(Time.time, 5);
        rend.material.mainTextureScale = new Vector2(scaleX, scaleY);

    }
}
