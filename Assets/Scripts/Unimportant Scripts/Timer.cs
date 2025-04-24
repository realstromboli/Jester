using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private GameObject insanityIndicator; // Add a reference to the GameObject with the Image component

    public int Duration;
    public int remainingDuration;
    public bool Pause;
    public MaskToggle mtScript;
    public DialogueManager dmScript;
    public GameManager gmScript;
    public Timer timerScript;
    public Image obscurity;

    public Coroutine timerCoroutine;

    private void Start()
    {
        Begin(Duration);
        insanityIndicator = GameObject.Find("Insanity");
        mtScript = GameObject.Find("Player").GetComponent<MaskToggle>();
        timerScript = GameObject.Find("MaskIndicator").GetComponent<Timer>();
        obscurity = GameObject.Find("Obscurity").GetComponent<Image>();
        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Update code if needed
        if (dmScript.dialogueActive == true || gmScript.isGameActive == false)
        {
            Pause = true;
        }
        else if (dmScript.dialogueActive == false || gmScript.isGameActive == true)
        {
            Pause = false;
        }

        if (SceneManager.GetActiveScene().name == "Inside Fun House" || SceneManager.GetActiveScene().name == "Mirrored Maze")
        {
            Pause = false;
        }
        else if (SceneManager.GetActiveScene().name != "Inside Fun House" || SceneManager.GetActiveScene().name != "Mirrored Maze")
        {
            Pause = true;
        }
    }

    public void Begin(int seconds)
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        remainingDuration = seconds;
        timerCoroutine = StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            if (!Pause && mtScript.maskStatus)
            {
                uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);

                // Calculate the new alpha value based on the remaining duration
                if (obscurity != null)
                {
                    float alpha = Mathf.Lerp(0, 200f / 255f, (Duration - remainingDuration) / (float)Duration);
                    obscurity.color = new Color(obscurity.color.r, obscurity.color.g, obscurity.color.b, alpha);
                }

                remainingDuration--;
                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
        OnEnd();
    }

    private void OnEnd()
    {
        // End Time, if you want to do something
        print("End");
        if (mtScript.maskStatus == true)
        {
            mtScript.maskToggle();
            mtScript.maskStatus = false;
            mtScript.SetLayerVisibility(false);
            mtScript.playerAnimation.SetTrigger("Mask Off Trigger");
            timerScript.Begin(timerScript.Duration);
            timerScript.Pause = true;
        }
        mtScript.maskStatus = false;
        StartCoroutine(InsanityCooldown());
    }

    public IEnumerator InsanityCooldown()
    {
        float cooldownDuration = 10f;
        float elapsedTime = 0f;

        // Enable the Image component of the insanityIndicator GameObject
        Image insanityImage = insanityIndicator.GetComponent<Image>();

        while (elapsedTime < cooldownDuration)
        {
            mtScript.readyToPress = false;
            if (insanityImage != null)
            {
                insanityImage.enabled = true;
            }
            // Set the alpha of obscurity to be slightly transparent (e.g., 0.78)
            obscurity.color = new Color(obscurity.color.r, obscurity.color.g, obscurity.color.b, 0.78f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Disable the Image component of the insanityIndicator GameObject
        if (insanityImage != null)
        {
            insanityImage.enabled = false;
        }

        // Set the alpha of obscurityImage to 0
        if (obscurity != null)
        {
            obscurity.color = new Color(obscurity.color.r, obscurity.color.g, obscurity.color.b, 0);
        }

        mtScript.readyToPress = true;
        Debug.Log("Mask Ready!");
    }
}
