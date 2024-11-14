using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string startingKnot; // New field for the starting knot name

    private bool PlayerInRange;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        visualCue.SetActive(false);
        PlayerInRange = false;
    }

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found in the scene.");
        }
    }

    private void Update()
    {
        if (PlayerInRange)
        {
            visualCue.SetActive(true);
            DialogueManager dialogueManager = DialogueManager.GetInstance();
            if (dialogueManager != null && !dialogueManager.isDialogueActive && Input.GetKeyDown(KeyCode.E) && playerMovement != null && playerMovement.IsStandingStill())
            {
                // Pass the inkJSON and startingKnot to start the dialogue at the specific conversation
                dialogueManager.EnterDialogueMode(inkJSON, startingKnot);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}
