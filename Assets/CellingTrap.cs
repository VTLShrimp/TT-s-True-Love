using UnityEngine;
using System.Collections;

public class CellingTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; // when the trap gets triggered
    private bool active; // when the trap is active and can hurt the player
    private bool hasDamaged; // to track if the player has already been damaged

    private PlayerHealth player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            Debug.Log("Player entered trap");
            player = collision.GetComponent<PlayerHealth>();

            if (player == null)
            {
                Debug.LogError("PlayerHealth component not found on the player!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
            Debug.Log("Player exited trap");
        }
    }

    private void Update()
    {
        if (active && player != null && !hasDamaged)
        {
            player.TakeDamage(damage);
            hasDamaged = true; // Set the flag to true after dealing damage
            Debug.Log("Player took damage: " + damage);
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        // Turn the sprite red to notify the player and trigger the trap
        triggered = true;
        spriteRend.color = Color.red;

        // Wait for delay, activate trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white; // Turn the sprite back to its initial color
        active = true;
        anim.SetBool("activated", true);
        Debug.Log("Trap activated");

        // Wait until X seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        hasDamaged = false; // Reset the flag when the trap is deactivated
        anim.SetBool("activated", false);
        Debug.Log("Trap deactivated");
    }
}