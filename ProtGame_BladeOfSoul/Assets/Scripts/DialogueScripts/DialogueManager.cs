using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    [SerializeField] GameObject objectDialogBox;
    [SerializeField] Animator animDialog;

    private Queue<string> sentences;


    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        objectDialogBox.SetActive(true);
        animDialog.SetBool("isOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence); //Добавление в конец очереди
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            animDialog.SetBool("isOpen", false);
            Invoke("EndDialogue", 1.0f);
            return;
        }

        string sentence = sentences.Dequeue(); //Возвращение первого элемента в очереди
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        Debug.Log("End");
        //animDialog.SetBool("isOpen", false);
        objectDialogBox.SetActive(false);
    }
}
