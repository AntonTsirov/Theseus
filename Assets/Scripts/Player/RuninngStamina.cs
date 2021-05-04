using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating the stamina feature that reduces its value when the player is running
public class RuninngStamina : MonoBehaviour
{
    public float currentRunningStamina;
    public float runingStaminaMax;
    public AudioSource breathingSound;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerController>().isRunning && currentRunningStamina > 0)
        {
            currentRunningStamina -= Time.deltaTime;
        }
        else if (!GetComponent<PlayerController>().isRunning && currentRunningStamina < runingStaminaMax && !Input.GetKey(KeyCode.LeftShift))
        {
            currentRunningStamina += Time.deltaTime;
            breathingSound.enabled = true;
        }
        Mathf.Clamp(currentRunningStamina, 0, runingStaminaMax);
        if (currentRunningStamina >= runingStaminaMax) breathingSound.enabled = false;
    }
}
