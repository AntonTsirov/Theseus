using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This class is responsible for displaying text on the screen with a typing animation
public class TextBubble : MonoBehaviour
{
    private TextMeshProUGUI textBuble;
    [TextArea(5, 10)]
    public string[] paragraphs;
    public float typingSpeed;
    private int totalVisChar;
    [HideInInspector]
    public bool isUIonFocus = false;
    public GameObject tutorialBox;

    public AudioSource openCloseSound;
    public AudioSource writingSound;
    void Start()
    {
        textBuble = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>();
        tutorialBox.SetActive(false);
    }

    void Update()
    {
        if (gameObject.GetComponentInChildren<TextMeshProUGUI>() != null) totalVisChar = textBuble.textInfo.characterCount;

        if (Input.GetKeyDown(KeyCode.Space) && isUIonFocus == true)
        {
            closeTutorialWindow();
        }
    }

    private IEnumerator Type(int paraIndex)
    {
        tutorialBox.SetActive(true);
        openCloseSound.Play();
        isUIonFocus = true;
        Time.timeScale = 0;
        textBuble.text = paragraphs[paraIndex];
        int visCount = 0;
        writingSound.Play();
        while (visCount <= totalVisChar && isUIonFocus == true)
        {
            textBuble.maxVisibleCharacters = visCount;
            visCount += 1;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        writingSound.Stop();
    }

    public void closeTutorialWindow()
    {
        openCloseSound.Play();
        tutorialBox.SetActive(false);
        isUIonFocus = false;
        Time.timeScale = 1;
    }

    public void showTutorial(int id)
    {
        StartCoroutine(Type(id));
    }
}
