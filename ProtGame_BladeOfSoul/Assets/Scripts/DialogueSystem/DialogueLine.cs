using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueLine : DialogueBaseClass
{
    [SerializeField] private string input;
    [SerializeField] private Color textColor;
    [SerializeField] private Font textFont;

    [SerializeField] private float delay;
    [SerializeField] private float delayBetweenLines;

    [Header("Audio")]
    [SerializeField] private AudioClip sound;

    private Text textHolder;
    private IEnumerator lineAppear;

    private void OnEnable()
    {
        ResetLine();
        lineAppear = WriteText(input, textHolder, textColor, textFont, delay, sound, delayBetweenLines);
        StartCoroutine(lineAppear);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (textHolder.text != input)
            {
                StopCoroutine(lineAppear);
                textHolder.text = input;
            }
            else
                Finished = true;
        }
    }

    private void ResetLine()
    {
        textHolder = GetComponent<Text>();
        textHolder.text = "";
        Finished = false;
    }
}
