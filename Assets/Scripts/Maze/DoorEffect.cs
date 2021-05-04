using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//very simple but easy effect of the door's texture to move
public class DoorEffect : MonoBehaviour
{
    public Renderer rend;
    float scaleY;

    // Update is called once per frame
    void Update()
    {
        scaleY += Time.deltaTime / 8f;
        rend.material.mainTextureOffset = new Vector2(0, scaleY);
    }
}
