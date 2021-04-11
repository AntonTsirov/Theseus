using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEffect : MonoBehaviour
{
    public Renderer rend;
    float scaleY;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scaleY += Time.deltaTime / 2f;
        rend.material.mainTextureOffset = new Vector2(0, scaleY);
    }
}
