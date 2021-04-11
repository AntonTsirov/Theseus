using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public bool isCorrupted;

    public Renderer rend1;
    public Renderer rend2;
    void Start()
    {
        isCorrupted = false;//todo
    }
    void Update()
    {
        if (isCorrupted)
        {
            float scaleX = Mathf.PingPong(Time.time / 3, 3);
            float scaleY = Mathf.PingPong(Time.time / 3, 3);
            rend1.material.mainTextureScale = new Vector2(scaleX, scaleY);
            rend2.material.mainTextureScale = new Vector2(scaleX, scaleY);

        }

    }
}
