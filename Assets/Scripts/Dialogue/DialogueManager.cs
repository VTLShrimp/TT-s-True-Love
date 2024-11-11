using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    public bool isDialogueActive { get; private set; }
    private Story currentstory;

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
        currentstory = new Story(inkJSON.text);
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }
    private void ExitDialogueMode()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentstory.canContinue)
        {
            dialogueText.text = currentstory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentstory.currentChoices;
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
    }
}
