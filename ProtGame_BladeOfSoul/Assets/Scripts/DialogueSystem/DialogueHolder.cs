using System.Collections;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    private IEnumerator dialogueSeq;
    private bool dialogueFinished;

    private void OnEnable()
    {
        dialogueSeq = DialogueSequnce();
        StartCoroutine(dialogueSeq);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            Deactivate();
            gameObject.SetActive(false);
            StopCoroutine(dialogueSeq);
        }
    }

    private IEnumerator DialogueSequnce()
    {
        if (!dialogueFinished)
        {
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
        }
        else
        {
            int index = transform.childCount - 1;
            Deactivate();
            transform.GetChild(index).gameObject.SetActive(true);
            yield return new WaitUntil(() => transform.GetChild(index).GetComponent<DialogueLine>().finished);
        }

        dialogueFinished = true;

        gameObject.SetActive(false);
    }

    private void Deactivate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
