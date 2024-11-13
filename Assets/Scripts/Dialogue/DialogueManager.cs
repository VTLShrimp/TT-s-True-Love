using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Additional UI Panels")]
    [SerializeField] private GameObject yesPanel; // Panel to open on "Yes" selection

    public bool isDialogueActive { get; private set; }
    private Story currentStory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("There is already an instance of DialogueManager in the scene");
        }
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!isDialogueActive)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = ""; // Clear dialogue text
        PlayerAttack.instance.DisableAttacking(0.2f);
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices(); // Display choices after showing the line
            ProcessTags(); // Process any tags within the story
        }
        else if (currentStory.currentChoices.Count > 0)
        {
            // If there are choices to be made, display them and don't exit
            DisplayChoices();
        }
        else
        {
            // If no lines or choices are left, exit the dialogue
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogWarning("There are more choices than the number of choice buttons");
            return;
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }

        // Ensure the first choice is selected when choices appear
        StartCoroutine(SelectChoice());
    }

    private IEnumerator SelectChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        // Set focus to the first choice only if keyboard navigation is detected
        if (Input.anyKey && !Input.GetMouseButton(0)) // Ensures mouse click doesn't trigger selection
        {
            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void ProcessTags()
    {
        foreach (string tag in currentStory.currentTags)
        {
            if (tag == "open_panel")
            {
                yesPanel.SetActive(true); // Activate the panel for the "Yes" choice
            }
        }
    }
}
