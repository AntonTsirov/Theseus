using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for defining the type of the agent and effect when he is hunting
public class Agent : MonoBehaviour
{
    public bool isCorrupted;

    public Renderer rend1;
    public Renderer rend2;
    void Start()
    {
        isCorrupted = false;
    }
    void Update()
    {
        if (isCorrupted)
        {
            float scaleX = Mathf.PingPong(Time.time / 2, 3);
            float scaleY = Mathf.PingPong(Time.time / 2, 3);
            rend1.material.mainTextureScale = new Vector2(scaleX, scaleY);
            rend2.material.mainTextureScale = new Vector2(scaleX, scaleY);
        }
    }

}
