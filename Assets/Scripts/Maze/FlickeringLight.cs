using UnityEngine;
using System.Collections.Generic;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

/// <summary>
/// Component which will flicker a linked light while active by changing its
/// intensity between the min and max values given. The flickering can be
/// sharp or smoothed depending on the value of the smoothing parameter.
///
/// Just activate / deactivate this component as usual to pause / resume flicker
/// </summary>


//found online a script for flickering light that is almost what I needed. 
//Responsible for flickering effect on lights and behaves differently depending on the light (flashlight/flare/lamp)
public class FlickeringLight : MonoBehaviour
{
    public Light lightOb;
    [Tooltip("Minimum random light intensity")]
    private float minIntensity;
    [Tooltip("Maximum random light intensity")]
    private float maxIntensity;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    private int smoothing;
    private bool shouldSwing;
    public bool isFlare;
    public bool isGlobalLight;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;


    /// <summary>
    /// Reset the randomness and start again. You usually don't need to call
    /// this, deactivating/reactivating is usually fine but if you want a strict
    /// restart you can do.
    /// </summary>
    public void Reset()
    {
        smoothQueue.Clear();
        lastSum = 0;
    }

    void Start()
    {
        if (!isFlare)
        {
            minIntensity = 0.2f;
            maxIntensity = 1f;
            smoothing = Random.Range(15, 20);
        }
        else
        {
            if (isGlobalLight)
            {
                minIntensity = 0.03f;
                maxIntensity = 0.1f;
            }
            else
            {
                minIntensity = Random.Range(0.6f, 0.7f);
                maxIntensity = Random.Range(0.8f, 1.1f);
            }
            smoothing = 5;
        }

        shouldSwing = Random.Range(0, 2) == 0 ? true : false;
        if (!shouldSwing && GetComponent<Animator>() != null) GetComponent<Animator>().enabled = false;

        smoothQueue = new Queue<float>(smoothing);
        // External or internal light?
        if (lightOb == null)
        {
            lightOb = GetComponent<Light>();
        }
    }

    void Update()
    {
        if (lightOb == null)
            return;


        if (isFlare & isGlobalLight) smoothing = Random.Range(5, 10);
        // pop off an item if too big
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        lightOb.intensity = lastSum / (float)smoothQueue.Count;
    }

}