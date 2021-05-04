using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//The class responsible for the blinking effect, it contains parameters that slowly grow and reset when blink is done
//Their growth effects the post processing object 
public class Blinking : MonoBehaviour
{
    public Animator eyesAnim;
    private FilmGrain filmGrainEffect;
    private MotionBlur motionBlurEffect;
    private float tempFilmGrainIntensity;
    private float tempMotionBlurIntensity;
    public float wateryLevel;
    public float slowdown;
    public float animSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Volume volume = FindObjectOfType<Volume>();
        volume.sharedProfile.TryGet<FilmGrain>(out filmGrainEffect);

        if (filmGrainEffect != null && motionBlurEffect != null)
        {
            tempFilmGrainIntensity = 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        wateryLevel += Time.deltaTime / 15f;
        wateryLevel = Mathf.Clamp(wateryLevel, 0f, 1f);

        if (Input.GetKeyDown(KeyCode.Mouse0) && !FindObjectOfType<TextBubble>().isUIonFocus)
        {
            wateryLevel = 0;
            filmGrainEffect.intensity.SetValue(new NoInterpMinFloatParameter(tempFilmGrainIntensity, 0, true));
            eyesAnim.SetTrigger("blink");
            eyesAnim.speed = Mathf.Clamp(eyesAnim.speed - slowdown, 0.4f, 1f);

        }

        if (eyesAnim.speed < 1f)
        {
            eyesAnim.speed = Mathf.Clamp(eyesAnim.speed + Time.deltaTime / 10f, 0.2f, 1f);
        }

        if (filmGrainEffect != null)
        {
            filmGrainEffect.intensity.SetValue(new NoInterpMinFloatParameter(wateryLevel, 0, true));
        }
    }
}
