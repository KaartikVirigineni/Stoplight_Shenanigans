using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDiolouge : MonoBehaviour
{
    public Text dialogueText;
    public Button continueButton;
    public string[] dialogues;
    public List<string> buttonTexts;
    public Image[] dialogueImages;
    public GameObject objectToEnable;
    public GameObject objectToDisable;
    public Canvas dialogueCanvas;
    private int currentDialogueIndex = 0;

    void Start()
    {
       objectToEnable.SetActive(false);
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(DisplayNextDialogue);
        }
        DisplayNextDialogue();
    }

    void DisplayNextDialogue()
    {
        if (dialogueCanvas != null && !dialogueCanvas.enabled)
        {
            dialogueCanvas.enabled = true;
        }

        foreach (Image img in dialogueImages)
        {
            img.enabled = false;
        }

        if (currentDialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentDialogueIndex];

            if (currentDialogueIndex < buttonTexts.Count)
            {
                continueButton.GetComponentInChildren<Text>().text = buttonTexts[currentDialogueIndex];
            }
            else
            {
                Debug.LogWarning($"No button text found for dialogue index {currentDialogueIndex}");
            }

            if (currentDialogueIndex < dialogueImages.Length && dialogueImages[currentDialogueIndex] != null)
            {
                dialogueImages[currentDialogueIndex].enabled = true;
            }

            currentDialogueIndex++;
        }
        else
        {
            dialogueText.text = "";
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true); 
            }
            objectToDisable.SetActive(false);
        }
    }
}
