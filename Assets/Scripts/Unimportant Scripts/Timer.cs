using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private GameObject insanityIndicator; // Add a reference to the GameObject with the Image component

    public int Duration;
    public int remainingDuration;
    public bool Pause;
    public MaskToggle mtScript;
    public Timer timerScript;

    private Coroutine timerCoroutine;

    private void Start()
    {
        Begin(Duration);
        insanityIndicator = GameObject.Find("Insanity");
        mtScript = GameObject.Find("Player").GetComponent<MaskToggle>();
        timerScript = GameObject.Find("MaskIndicator").GetComponent<Timer>();
    }

    void Update()
    {
        // Update code if needed
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
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Disable the Image component of the insanityIndicator GameObject
        if (insanityImage != null)
        {
            insanityImage.enabled = false;
        }

        mtScript.readyToPress = true;
        Debug.Log("Mask Ready!");
    }
}
