using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
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

    [Header("Upgrade Panels")]
    [SerializeField] private GameObject npc1UpgradePanel; // Panel for NPC 1
    [SerializeField] private GameObject npc2UpgradePanel; // Panel for NPC 2

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
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
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

    public void EnterDialogueMode(TextAsset inkJSON, string startingKnot)
    {
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(startingKnot); // Start at the specified knot
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = ""; // Clear dialogue text
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
            ProcessTags();
        }
        else if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
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

        StartCoroutine(SelectChoice());
    }

    private IEnumerator SelectChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        if (Input.anyKey && !Input.GetMouseButton(0))
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
            if (tag == "open_panel_npc1")
            {
                npc1UpgradePanel.SetActive(true); // Activate NPC 1's upgrade panel

                ExitDialogueMode();
            }
            else if (tag == "open_panel_npc2")
            {
                npc2UpgradePanel.SetActive(true); // Activate NPC 2's upgrade panel

                ExitDialogueMode();
            }
        }
    }
}
